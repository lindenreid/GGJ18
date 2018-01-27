using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour {

	public float speed = 1.0f;
	private bool move = false;
	private float destX;
	private GameController GameController;

	void Update ()
	{
		if (move)
		{
			transform.Translate(Vector3.right * speed * Time.deltaTime);

			if (transform.position.x >= destX)
			{
				GameController.DeckAnimFinished();
				move = false;
			}
		}
	}

	public void Move (GameController gc, float dest)
	{
		move = true;
		GameController = gc;
		destX = dest;
	}
}
