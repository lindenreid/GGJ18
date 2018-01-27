using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Lamp : MonoBehaviour {

	public Renderer rend;
	public Color defaultColor;
	public List<Color> colors;
	public Color color;

	void Start ()
	{
		colors = new List<Color>();
		rend.material.SetColor("_Color", defaultColor);
	}

	public void AddColor (Color color)
	{
		colors.Add(color);
		RecalculateColor();
	}

	public void RemoveColor (Color color)
	{
		colors.Remove(color);
		RecalculateColor();
	}

	private void RecalculateColor ()
	{
		Color avgColor = new Color(0,0,0,0);
		foreach(Color c in colors)
			avgColor += c;
		avgColor = avgColor / colors.Count;

		rend.material.SetColor("_Color", avgColor);
		color = avgColor;
	}
	
}
