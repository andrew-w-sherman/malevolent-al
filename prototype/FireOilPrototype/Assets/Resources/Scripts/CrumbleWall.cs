using UnityEngine;
using System.Collections;

public class CrumbleWall : Tile {
    
    BoxCollider2D myCollider;
    Rigidbody2D body;
    CrumbleWallModel model;
    

    // Use this for initialization
    public override void init(GameController gc)
    {
        controller = gc;
        myCollider = gameObject.AddComponent<BoxCollider2D>();
        body = gameObject.AddComponent<Rigidbody2D>();
        body.isKinematic = true;

        gameObject.tag = "CrumbleWall";

        var modelObject = new GameObject();
        model = modelObject.AddComponent<CrumbleWallModel>();
        model.init(this, controller);
        
    }
    

    void Update()
    {
       
    }


    void OnCollisionEnter2D(Collision2D coll)
    {

        if (coll.gameObject.tag == "OilBall_Speeding")
        {
            Destroy(gameObject);
        }
    }

}
