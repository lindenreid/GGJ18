using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Subtitles : MonoBehaviour {

	public Text text;
	public float fadeSpeed;
	public float lingerTime;
	public List<string> subtitles;
	public GameController controller;

	private int currentTitle;
	private bool fade;
	private bool linger;
	private float lingerLeft;
	private bool init = false;
	
	void Update () {
		if (fade)
		{
			float a = text.color.a - fadeSpeed * Time.deltaTime;
			text.color = new Color(text.color.r, text.color.g, text.color.b, a);
			if (a <= 0)
			{
				PlayNextTitle();
			}
		}
		else if (linger)
		{
			if (text.color.a < 1.0f)
			{
				// fade in
				float a = text.color.a + fadeSpeed * Time.deltaTime;
				text.color = new Color(text.color.r, text.color.g, text.color.b, a);
			}
			
			lingerLeft -= Time.deltaTime;
			if (lingerLeft <= 0 )
			{
				linger = false;
				fade = true;
			}
		}
	}

	public void PlaySubtitles () {
		currentTitle = -1;
		PlayNextTitle();
	}

	private void PlayNextTitle ()
	{
		fade = false;
		currentTitle += 1;
		Debug.Log("title:" + currentTitle);
		if (currentTitle < subtitles.Count)
		{
			text.text = subtitles[currentTitle];
			lingerLeft = lingerTime;
			linger = true;
			fade = false;
		}
		else
		{
			linger = false;
			controller.SubtitlesDone();
			gameObject.SetActive(false);
		}
	}
}
