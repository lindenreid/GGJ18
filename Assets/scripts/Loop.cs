using UnityEngine;

public class Loop : MonoBehaviour {

    public Vector3 resetPos;
    public float startSpeed = 1.0f;
    public float dec = 0.1f;
    public Vector3 startPos;
    public float maxPosX;
    public float endPosX;

    private float speed;
    private bool slowing;

    void Start () {
        Reset();
    }

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

    public void Reset() {
        speed = startSpeed;
        slowing = false;
        transform.position = resetPos;
    }
}