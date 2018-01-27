using UnityEngine;

public class Loop : MonoBehaviour {

	public float speed = 1.0f;
    public float dec = 0.1f;
    public Vector3 startPos;
    public float maxPosX;
    public float endPosX;

    private bool slowing;

	void Update () {
        if (slowing) 
        {
            speed -= dec;
            if (speed <= 0)
                speed = 0;
        }

        transform.Translate(Vector3.right * speed * Time.deltaTime);
        
        if(!slowing && transform.position.x >= maxPosX)
        {
            transform.position = startPos;
        }
    }

    public void Slow() {
        slowing = true;
    }
}