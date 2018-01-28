using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;

public class Passenger : MonoBehaviour {

	public Vector3 originalPos;

	public float floatUpSpeed = 1.0f;
	public float dissolveDuration = 1.0f;
	public float fadeDuration = 1.0f;
	public float boardYPos = 1.57f;
	public Vector3 boardDest;
	public float boardingSpeed;
	
	private float dissolveRemaining;
	private bool dissolve;
	private Renderer rend;
	private GameController GameController;

	private bool board;
	private Vector3 boardTrans;

	public AnimationClip boardingAnim;

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
			}
			else
			{
				transform.Translate(0, floatUpSpeed*Time.deltaTime, 0);
			}
		}
		else if (board)
		{
			transform.Translate(boardTrans * boardingSpeed * Time.deltaTime);
			Vector3 distRemaining = transform.position - boardDest;
			float dist = distRemaining.magnitude;
			print("dist:" + dist);
			if (dist <= 0.1)
			{
				board = false;
				GameController.PassengerBoardingFinished();
			}
		}
	}

	public void SetColor (Color color)
	{
		float a = rend.material.GetColor("_Color").a;
		rend.material.SetColor("_Color", new Color(color.r, color.g, color.b, a));
	}

	public void PlayDissolveAnim ()
	{
		if (!rend)
			rend = GetComponent<Renderer>();
		dissolveRemaining = dissolveDuration;
		dissolve = true;
		rend.material.SetInt("_Dissolve", 1);
		rend.material.SetFloat("_StartTime", Time.time);
	}

	public void PlayBoardingAnim (GameController controller)
	{
		GameController = controller;
		transform.localPosition = new Vector3(transform.localPosition.x, boardYPos, transform.localPosition.z);
		GetComponent<PlayAnimation>().PlayClip(boardingAnim, false);
		board = true;
		boardTrans = boardDest - transform.position;
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
