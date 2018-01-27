using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : MonoBehaviour {

	public GameController GameController;

	public float speed = 1.0f;
	public float acceleration = 0.01f;
	public bool moving;
	public float xPad;
	private float destX;

	void Start ()
	{
		destX = GameController.dockStart.x + xPad;
	}

	void Update ()
	{
		if (moving)
		{
			transform.Translate(Vector3.left * speed * Time.deltaTime);
			if (transform.position.x <= destX)
				moving = false;

			speed += acceleration;
		}
	}

	public void Move ()
	{
		moving = true;
	}

}
