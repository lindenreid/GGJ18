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
	public Vector3 dockStart;

	// prefabs
	public GameObject dockPrefab;
	public GameObject starPrefab;
	public GameObject connectionPrefab;

	// general scene references
	public Lamp lamp;
	public Renderer soul;
	public List<Loop> loopers;
	public Boat boat;

	// constellation stuff
	private List<Star> starGraph;
	private Star lastClickedStar;

	// stuff to be cleaned up at reset
	private List<Star> allStars;
	public GameObject dock;

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

		foreach (Loop looper in loopers)
		{
			looper.Reset();
		}
	}

	private void StartGame ()
	{
		// set goal color
		Color color = new Color(Random.value, Random.value, Random.value);
		goalColor = color;
		soul.material.SetColor("_Color", goalColor);

		// create star selection
		for (int i = 0; i < numStars; i++)
		{
			color = new Color(Random.value, Random.value, Random.value);
			float x = Random.Range(minStarLoc.x, maxStarLoc.x);
			float y = Random.Range(minStarLoc.y, maxStarLoc.y);
			
			GameObject starObj = Instantiate(starPrefab, new Vector3(x, y, 0), Quaternion.identity);
			Star star = starObj.GetComponent<Star>();
			star.GameController = this;
			star.SetColor(color);
			allStars.Add(star);
		}
	}

	public void AddStar (Star star)
	{
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
		CheckColor();
	}

	public void RemoveStar (Star star)
	{
		// TEMP: just set size to large to show selected
		star.SetSelected(false, smallStarSize);

		// TODO: connection stuff

		lamp.RemoveColor(star.color);
		CheckColor();
	}

	private void CheckColor ()
	{
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
		dock = Instantiate(dockPrefab, dockStart, Quaternion.identity) as GameObject;

		foreach (Loop loop in loopers)
			loop.Slow();

		boat.Move();
	}

}
