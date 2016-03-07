using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {

    public Vector3 startPosition;
    public float clock = 0f;

    public int falling;
    public Collider2D fallingInto;
    public float initialDistance;
    public float whenFell = 0;
    float currentScale = 1f; // your scale factor

    void Start () {
        falling = 0;
	}

    public void pitHit(Collider2D other)
    {
        if (other.gameObject.tag == "Pit")
        {
            if (other.gameObject.GetComponent<Pit>().filled == 0)
            {
                fallingInto = other;
                initialDistance = Vector2.Distance(transform.position, other.transform.position);
                whenFell = clock;
                falling = 1;
            }
        } 
    }

    public void fallSequence()
    {
        if (clock < whenFell + 1)
        {
            currentScale = currentScale - Time.deltaTime;
            transform.localScale = new Vector3(currentScale, currentScale, currentScale);
            transform.position = Vector2.MoveTowards(transform.position, fallingInto.transform.position, initialDistance * Time.deltaTime);
        }
        else
        {
            currentScale = 1f;
            transform.localScale = new Vector3(currentScale, currentScale, currentScale);
            transform.position = startPosition;
            falling = 0;
        }
    }

    // Update is called once per frame
    void Update () {
        clock = clock + Time.deltaTime;
    }
}
