using UnityEngine;
using System.Collections;

public class Fire : MonoBehaviour {

    public int curDir;

	// Use this for initialization
	void Start () {
	    //will be able to pre-set the direction 0-7
        //clicking the ball will launch it in that direction
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseDown()
    {
        float thrust = 325;
        Vector3 outd = Vector3.right;    

        switch (curDir)
        {
            case 0:
                outd = Vector3.up;
                break;
            case 1:
                outd = new Vector3(1,1, 0);
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


        GetComponent<Rigidbody>().AddForce(new Vector3(outd.x * thrust, outd.y * thrust, outd.z * thrust));
    }

    
}
