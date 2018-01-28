using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour {

	public float speed = 1.0f;
	public float destroyX;
	private bool destroy = false;
	private bool move = false;
	private float destX;
	private GameController GameController;

	void Update ()
	{
		if (move)
		{
			transform.Translate(Vector3.right * speed * Time.deltaTime);
			if (!destroy && transform.position.x >= destX)
			{
				GameController.DeckAnimFinished();
				move = false;
			}
			else if (destroy && transform.position.x >= destroyX)
			{
				Destroy(this.gameObject);
			}
		}
	}

	public void Move (GameController gc, float dest)
	{
		move = true;
		GameController = gc;
		destX = dest;
	}

	public void MoveToDestroy ()
	{
		move = true;
		destroy = true;
	}
}
