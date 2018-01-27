using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	// directed graph star --n--> star
	// add connection: map lastClickedStar ----> currentClickedStar
	// remove connection: check all stars in starGraph for
	//                    a connectedion to removedStar
	//                    and remove those connections

	public Lamp lamp;
	public Renderer soul;
	public float winMargin;
	private Color goalColor;
	public List<Loop> loopers;

	public Boat boat;
	public GameObject dockPrefab;
	public Vector3 dockStart;

	public GameObject connectionPrefab;
	public GameObject starPrefab;
	public Vector3 smallStarSize = new Vector3(1.0f, 1.0f, 1.0f);
	public Vector3 largeStarSize = new Vector3(1.5f, 1.5f, 1.5f);

	private List<Star> starGraph;
	private Star lastClickedStar;

	void Start ()
	{
		Color color = new Color(Random.value, Random.value, Random.value);
		goalColor = color;
		soul.material.SetColor("_Color", goalColor);
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
		GameObject dock = Instantiate(dockPrefab, dockStart, Quaternion.identity) as GameObject;

		foreach (Loop loop in loopers)
			loop.Slow();

		boat.Move();
	}

}
