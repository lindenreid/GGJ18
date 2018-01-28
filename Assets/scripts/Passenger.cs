using UnityEngine;

public class Passenger : MonoBehaviour {

	public Vector3 originalPos;

	public float floatUpSpeed = 1.0f;
	public float dissolveDuration = 1.0f;
	public float fadeDuration = 1.0f;
	
	private float dissolveRemaining;
	private bool dissolve;
	private bool alert;
	private Renderer rend;
	private GameController GameController;

	void Start () 
	{
		rend = GetComponent<Renderer>();
	}

	void Update ()
	{
		if (dissolve)
		{
			dissolveRemaining -= Time.deltaTime;
			if (dissolveRemaining <= 0.0f)
			{
				dissolve = false;
				if (alert)
					GameController.PassengerAnimFinished();
			}
			else
			{
				transform.Translate(0, floatUpSpeed*Time.deltaTime, 0);
			}
		}
	}

	public void PlayDissolveAnim (GameController controller, bool alert)
	{
		if (!rend)
			rend = GetComponent<Renderer>();
		dissolveRemaining = dissolveDuration;
		dissolve = true;
		rend.material.SetInt("_Dissolve", 1);
		rend.material.SetFloat("_StartTime", Time.time);
		GameController = controller;
		this.alert = alert;
	}

	public void Reset ()
	{
		if (!rend)
			rend = GetComponent<Renderer>();
		dissolve = false;
		transform.localPosition = originalPos;
		rend.material.SetInt("_Dissolve", 0);
	}


}
