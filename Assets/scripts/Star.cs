using UnityEngine; 
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine.Animations;

public class Star : MonoBehaviour {

	public GameController GameController;
	public Color color = new Color(1,1,1,1);
	public List<AnimationClip> animations;
	public GameObject circle;
	private Renderer rend;

	public class Connection {
		public Star next;
		public Connector viz;

		public Connection(Star n, Connector v) { next = n; viz = v; }
	}
	private List<Connection> connections;

	public bool selected = false;

	void Start ()
	{
		connections = new List<Connection>();
		rend = GetComponent<Renderer>();
	}

	public void SetColor (Color c)
	{
		color = c;
		GetComponent<Renderer>().material.SetColor("_Color", color);
	}

	public void AnimateRandom ()
	{
		int i = (int)Mathf.Floor(Random.Range(0, animations.Count));
		AnimationClip clip = animations[i];

		PlayAnimation anim = GetComponent<PlayAnimation>();
		anim.PlayClip(clip, true);
	}

	void OnMouseDown ()
	{
		if (selected)
			GameController.RemoveStar(this);
		else 
			GameController.AddStar(this);
	}

	public void PlayDissolveAnim ()
	{
		circle.SetActive(false);
		rend.material.SetInt("_Dissolve", 1);
		rend.material.SetFloat("_StartTime", Time.time);
	}

	public void AddConnection(Star next, Connector c)
	{
		connections.Add(new Connection(next, c));
	}

	public void SetSelected(bool selected, Vector3 size)
	{
		this.selected = selected;
		transform.localScale = size;
		circle.SetActive(selected);
	}

}
