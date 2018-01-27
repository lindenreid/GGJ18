using UnityEngine;

public class Connector : MonoBehaviour {

	private Vector3 startPoint;
	private Vector3 endPoint;
	private Vector3 scale = new Vector3(1, 1, 1);

	public void SetConnection (Vector3 start, Vector3 end)
	{
		startPoint = start;
		endPoint = end;
		float dist = Vector3.Distance(startPoint, endPoint);
		transform.localScale = new Vector3(dist, 1, 1);
	}
	
}
