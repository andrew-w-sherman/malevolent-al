using UnityEngine;
using System.Collections;

public class Obstacle : Tile {

    public const int FIRE = 0; //A firewall is passable only to fire
    public const int OIL = 1;  //An oilwall is passable only to oil
    int wallType;
    bool physicsUpdated = false;
    ObstacleModel model;
    BoxCollider2D myCollider;

	// Use this for initialization
	public void init (GameController gc, int t) {
        controller = gc;
        myCollider = gameObject.AddComponent<BoxCollider2D>();
       
        wallType = t;
        
        var modelObject = new GameObject();
        model = modelObject.AddComponent<ObstacleModel>();
        model.init(this, controller, wallType);
    }
	
	// Update is called once per frame
	void Update () {
        if (!physicsUpdated)
        {
            if (wallType == FIRE)
            {
                Debug.Log(controller);
                Physics2D.IgnoreCollision(myCollider, controller.fire.coll);
            }
            if (wallType == OIL)
            {
                Physics2D.IgnoreCollision(myCollider, controller.oil.coll);
            }
            physicsUpdated = true;
        }
    }
    
    void OnCollisionEnter2D(Collision2D coll)
    {
        print("Collided");
        //if(wallType == FIRE)
        //{
        //    if (coll.gameObject.tag == "FireBall")
        //    {
        //        myCollider.isTrigger = true;
        //    }

        //}
        //else
        //{
        //    if (coll.gameObject.tag == "OilBall" || coll.gameObject.tag == "OilBall_Speeding") 
        //    {
        //        myCollider.isTrigger = true;
        //    }
        //}

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
