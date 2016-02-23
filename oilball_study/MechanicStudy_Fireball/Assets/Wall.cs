using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Wall : MonoBehaviour {

    int offWallDir;
    public int type;    //There are 8 types of walls {0 degrees, ~20deg, 45deg, ~70deg, 90deg, ~110deg, 135deg, ~160deg, 180deg == 0,1,2,3,4,5,6,7}
    Dictionary<int, int> inOutDir;
    

	// Use this for initialization
	void Start () {
        setupDirections();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision col)
    {
        int direction;
        if(col.gameObject.tag == "oil")
        {
            Oil oball = col.gameObject.GetComponent<Oil>();
            //take the direction and figure out which new direction to travel in
            inOutDir.TryGetValue(oball.getDir(), out direction);
            Vector3 v = getVector(direction);
            print(v.ToString() + " AAA " + col.relativeVelocity);
            float newx = v.x;
            float newy = v.y;

            //determine the speed            
            float c2 = (col.relativeVelocity.x * col.relativeVelocity.x) + (col.relativeVelocity.y * col.relativeVelocity.y);
            if (col.relativeVelocity.x != 0)
            {
                newx = v.x * ((float) Math.Sqrt(c2/2));
            }
            if(col.relativeVelocity.y != 0)
            {
                newy = v.y * ((float)Math.Sqrt(c2 / 2));
            }

            Rigidbody rg = col.gameObject.GetComponent<Rigidbody>();
            print("newx: "+newx + " newy: " + newy+" y: "+ v.y+ " x: "+v.x);

            //rg.AddForce(new Vector3(newx, newy, 0), ForceMode.VelocityChange);
            rg.velocity = new Vector3(newx, newy, 0);
            oball.setDir(direction);   


        }
    }

    void setOffWallDir(int inDir)
    {       
        //TODO: build one more fire that launches oil in a diagonal
            //then build walls of diff types
            //just have a restart button, work on it in the morning

        //Here we want to ta
        

        

    }

    void setupDirections()
    {
        //take type number and add it to whatever calculations being done
        int begin = 20;

        inOutDir= new Dictionary<int, int>();
        for(int i = 0; i < 8; i++)
        {
            inOutDir.Add(i, (begin - type - i) % 8);
            print(i + " : " + (begin - type - i)% 8);

        }

    }

    Vector3 getVector(int curDir)
    {
        Vector3 outd = Vector3.zero;
        switch (curDir)
        {
            case 0:
                outd = Vector3.up;
                break;
            case 1:
                outd = new Vector3(1, 1, 0);
                break;
            case 2:
                outd = Vector3.right;
                break;
            case 3:
                outd = new Vector3(1, -1, 0);
                break;
            case 4:
                outd = Vector3.down;
                break;
            case 5:
                outd = new Vector3(-1, -1, 0);
                break;
            case 6:
                outd = Vector3.left;
                break;
            case 7:
                outd = new Vector3(-1, 1, 0);
                break;
            default:
                break;

        }
        return outd;
    }

}
