using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Float : MonoBehaviour {

	public float AnimSpeed = 2.5f;
	public float AnimAmp = 0.01f;
	public bool randomSpeed = true;
	private Vector3 originalPos;

	void Start () {
		originalPos = transform.position;

		if (randomSpeed)
			AnimSpeed *= Random.value + 0.2f;
	}
	
	void Update () {
		float yMove = Mathf.Sin(Time.time*AnimSpeed)*AnimAmp;
		transform.Translate(new Vector3(0, yMove, 0));
	}
}
