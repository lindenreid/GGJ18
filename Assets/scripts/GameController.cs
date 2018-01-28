using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	// directed graph star --n--> star
	// add connection: map lastClickedStar ----> currentClickedStar
	// remove connection: check all stars in starGraph for
	//                    a connectedion to removedStar
	//                    and remove those connections

	// tuning
	public float winMargin;
	private Color goalColor;
	public int numStars;
	public Vector3 maxStarLoc;
	public Vector3 minStarLoc;
	public Vector3 smallStarSize = new Vector3(1.0f, 1.0f, 1.0f);
	public Vector3 largeStarSize = new Vector3(1.5f, 1.5f, 1.5f);
	public Vector3 passengerStart;
	public Vector3 dockStart;
	public float dockFinish;
	public int minColors = 2;
	public int maxColors = 5;
	public Vector3 passengerOnBoatPos;

	// prefabs
	public GameObject dockPrefab;
	public GameObject passengerPrefab;
	public GameObject starPrefab;
	public GameObject connectionPrefab;

	// general scene references
	public PlayAnimation reaperAnim;
	public Passenger passenger;
	public GameObject newPassenger;
	public Lamp lamp;
	public Renderer soul;
	public List<Loop> loopers;
	public List<Float> floaters;
	public Boat boat;

	// UI references
	public GameObject PauseMenu;

	// constellation stuff
	private List<Star> starGraph;
	private Star lastClickedStar;

	// stuff to be cleaned up at reset
	private List<Star> allStars;
	public GameObject dock;

	// pause
	public bool gameplay;

	void Start ()
	{
		allStars = new List<Star>();
		StartGame();
	}

	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			ClearGame();
			StartGame();
		}
		else if (Input.GetKeyDown(KeyCode.Escape))
		{
			Pause();
		}
		else if (Input.GetKeyDown(KeyCode.Tab))
		{
			ColorSuccess();
		}
	}

	private void ClearGame ()
	{
		foreach (Star star in allStars)
		{
			Destroy(star.gameObject);
		}
		allStars = new List<Star>();

		starGraph = new List<Star>();
		lastClickedStar = null;

		if (dock)
			Destroy(dock);

		boat.Reset();
		lamp.Reset();
		passenger.Reset();

		foreach (Loop looper in loopers)
		{
			looper.Reset();
		}
	}

	private void StartGame ()
	{
		// create remaining random colors
		for (int i = 0; i < numStars; i++)
		{
			Color color = new Color(Random.value, Random.value, Random.value);
			CreateStar(color);
		}

		// pick n random colors to create average
		int n = (int)Mathf.Floor(Random.Range(minColors, maxColors + 1));
		List<Color> colors = new List<Color>();
		
		for (int i = 0; i < n; i++)
		{
			int j = (int)Mathf.Floor(Random.Range(0, allStars.Count));
			Color c = allStars[j].color;
			while (colors.Contains(c))
			{
				j = (int)Mathf.Floor(Random.Range(0, allStars.Count));
				c = allStars[j].color;
			}
			colors.Add(c);
			colors[i] = c;
		}

		// compute average of colors
		Color avgColor = new Color(0,0,0,0);
		foreach(Color c in colors)
			avgColor += c;
		avgColor = avgColor / n;

		// set average to goal color
		goalColor = avgColor;
		soul.material.SetColor("_Color", avgColor);

		gameplay = true;
	}

	private void CreateStar (Color color)
	{
		float x = Random.Range(minStarLoc.x, maxStarLoc.x);
		float y = Random.Range(minStarLoc.y, maxStarLoc.y);

		GameObject starObj = Instantiate(starPrefab, new Vector3(x, y, 0), Quaternion.identity);

		Star star = starObj.GetComponent<Star>();
		
		star.GameController = this;
		star.AnimateRandom();
		star.SetColor(color);
		allStars.Add(star);

		floaters.Add(star.GetComponent<Float>());
	}

	public void Pause ()
	{
		if (gameplay)
		{
			gameplay = false;
			Time.timeScale = 0;
			PauseMenu.SetActive(true);
			foreach (Float f in floaters)
				f.Pause(true);
		}
		else
		{
			gameplay = true;
			Time.timeScale = 1;
			PauseMenu.SetActive(false);
			foreach (Float f in floaters)
				f.Pause(false);
		}
	}

	public void Exit ()
	{
		Application.Quit();
	}

	public void AddStar (Star star)
	{
		if (!gameplay) return;

		// TEMP: just set size to large to show selected
		star.SetSelected(true, largeStarSize);

		// TODO: connection stuff
		/*
		if (lastClickedStar)
		{
			GameObject cObj = Instantiate(connectionPrefab) as GameObject;
			Connector c = cObj.GetComponent<Connector>();
			c.SetConnection(lastClickedStar.transform.position, star.transform.position);

			lastClickedStar.transform.localScale = largeStarSize;
			star.transform.localScale = largeStarSize;

			lastClickedStar.AddConnection(star, c);
		}
		lastClickedStar = star;
		 */

		lamp.AddColor(star.color);
		reaperAnim.Play(false);
		CheckColor();
	}

	public void RemoveStar (Star star)
	{
		if (!gameplay) return;

		// TEMP: just set size to large to show selected
		star.SetSelected(false, smallStarSize);

		// TODO: connection stuff

		lamp.RemoveColor(star.color);
		reaperAnim.Play(false);
		CheckColor();
	}

	public void DeckAnimFinished ()
	{
		Destroy(passenger.gameObject);
		passenger = null;

		foreach (Loop looper in loopers)
		{
			looper.Stop();
		}

		newPassenger.GetComponent<Passenger>().PlayDissolveAnim(this, true);
	}

	public void PassengerAnimFinished ()
	{
		// put passenger on boat
		Destroy(newPassenger.gameObject);
		newPassenger = null;
		
		GameObject po = Instantiate(passengerPrefab, passengerOnBoatPos, Quaternion.identity) as GameObject;
		passenger = po.GetComponent<Passenger>();
		passenger.transform.SetParent(boat.gameObject.transform);

		ClearGame();
		StartGame();
	}

	private void CheckColor ()
	{
		if (!gameplay) return;

		Vector3 diff = new Vector3(
			Mathf.Abs(lamp.color.r - goalColor.r),
			Mathf.Abs(lamp.color.g - goalColor.g),
			Mathf.Abs(lamp.color.b - goalColor.b)
		);

		if (diff.magnitude < winMargin)
			ColorSuccess();
	}

	private void ColorSuccess ()
	{
		if (!gameplay) return;

		dock = Instantiate(dockPrefab, dockStart, Quaternion.identity) as GameObject;
		dock.GetComponent<Scroll>().Move(this, dockFinish);
		passenger.PlayDissolveAnim(this, false);
		gameplay = false;

		foreach (Star star in allStars)
		{
			star.PlayDissolveAnim();
		}

		newPassenger = Instantiate(passengerPrefab, passengerStart, Quaternion.identity) as GameObject;
		newPassenger.transform.SetParent(dock.transform, false);
	}

}
