using UnityEngine;
using System.Collections;

public class Oil : MonoBehaviour {

    int curDir;
    Vector3 acceleration;
    Vector3 lastVelocity;

	// Use this for initialization
	void Start () {
        //if it gets hit by fire it gets the current direction of the oil 
        //it heads off in the same direction of the oil
        //if it hits a wall: the wall will control the bounce off
        //else if it hits an enemy: it will destroy the enemyobject
        acceleration = Vector3.zero;
        lastVelocity = new Vector3(0, 0, 0);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        acceleration = (GetComponent<Rigidbody>().velocity - lastVelocity) / Time.deltaTime;
	}

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "fire")
        {
            Fire fire = col.gameObject.GetComponent<Fire>();
            curDir = fire.curDir;
            GetComponent<Rigidbody>().AddForce(col.relativeVelocity);
            print(col.relativeVelocity);
            col.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

    public int getDir()
    {
        return curDir;
    }
    public Vector3 getAcceleration()
    {
        return acceleration;
    }
    public void setDir(int dir)
    {
        curDir = dir;
    }
}
