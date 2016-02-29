using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {

    GameObject oilBallObject;
    GameObject fireBallObject;

	void Start () {
        
        oilBallObject = new GameObject();
        oilBallObject.tag = "OilBall";
        BoxCollider2D coll = oilBallObject.AddComponent<BoxCollider2D>();
        coll.size = new Vector2(1, 1);
        coll.isTrigger = false;
        Rigidbody2D rig = oilBallObject.AddComponent<Rigidbody2D>();
        oilBallObject.SetActive(true);
        rig.mass = 10;
        rig.gravityScale = 0f;
        rig.isKinematic = false;
        OilBall oilball = oilBallObject.AddComponent<OilBall>();
        oilball.transform.position = new Vector3(0, 0, 0);
        oilball.init(this);
               
        fireBallObject = new GameObject();
        fireBallObject.tag = "FireBall";
        BoxCollider2D coll2 = fireBallObject.AddComponent<BoxCollider2D>();
        coll2.size = new Vector2(1, 1);
        coll2.isTrigger = true;
        Rigidbody2D rig2 = fireBallObject.AddComponent<Rigidbody2D>();
        fireBallObject.SetActive(true);
        rig2.mass = 10;
        rig2.gravityScale = 0f;
        rig2.isKinematic = false;
        FireBall fireball = fireBallObject.AddComponent<FireBall>();
        fireball.transform.position = new Vector3(-2, -2, 0);
        fireball.init(this);
        
	}
	
	// Update is called once per frame
	void Update () {
	    
	}
}
