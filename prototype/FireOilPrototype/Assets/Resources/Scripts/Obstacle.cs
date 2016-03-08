using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour {

    public const int FIRE = 0; //A firewall is passable only to fire
    public const int OIL = 1;  //An oilwall is passable only to oil
    GameController control;
    int wallType;
    ObstacleModel model;
    BoxCollider2D myCollider;

	// Use this for initialization
	public void init (GameController gc, int t) {
        control = gc;
        myCollider=gameObject.AddComponent<BoxCollider2D>();
       
        wallType = t;
        
        var modelObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
        model = modelObject.AddComponent<ObstacleModel>();
        model.init(this, control, wallType);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    
    void OnCollisionEnter2D(Collision2D coll)
    {
        print("Collided");
        if(wallType == FIRE)
        {
            if (coll.gameObject.tag == "FireBall")
            {
                myCollider.isTrigger = true;
            }

        }
        else
        {
            if (coll.gameObject.tag == "OilBall" || coll.gameObject.tag == "OilBall_Speeding") 
            {
                myCollider.isTrigger = true;
            }
        }

    }
    
    void OnTriggerExit2D(Collider2D coll)
    {
        print("exit");
        if (wallType == FIRE)
        {
            if (coll.gameObject.tag == "FireBall")
            {
                myCollider.isTrigger = false;
            }

        }
        else
        {
            if (coll.gameObject.tag == "OilBall" || coll.gameObject.tag == "OilBall_Speeding")
            {
                myCollider.isTrigger = false;
            }
        }
    }

    
}
