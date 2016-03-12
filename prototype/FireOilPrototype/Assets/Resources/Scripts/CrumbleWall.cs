using UnityEngine;
using System.Collections;

public class CrumbleWall : Tile {

    GameController controller;
    BoxCollider2D myCollider;
    Rigidbody2D body;
    CrumbleWallModel model;
    public bool destroyNextFrame;
    

    // Use this for initialization
    public override void init(GameController gc)
    {
        controller = gc;
        myCollider = gameObject.AddComponent<BoxCollider2D>();
        body = gameObject.AddComponent<Rigidbody2D>();
        body.isKinematic = true;

        gameObject.tag = "CrumbleWall";

        var modelObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
        model = modelObject.AddComponent<CrumbleWallModel>();
        model.init(this, controller);

        destroyNextFrame = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (destroyNextFrame)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        print("oncolburnwall " + coll.gameObject.tag);

        if (coll.gameObject.tag == "OilBall_Speeding")
        {
            if(controller.oil.speeding)
            {
                tag = "CrumbleWallImpact";
                destroyNextFrame = true;
            }
        }
        if (coll.gameObject.tag == "CrumbleWallImpact")
        {
            Destroy(gameObject);
        }
    }



    //void OnTriggerEnter2D(Collider2D col)
    //{

    //    if (col.gameObject.tag == "BurnWall")
    //    {
    //        col.gameObject.GetComponent<BurnWall>().startBurn();
    //    }
    //    if (col.gameObject.tag == "Explosion")
    //    {
    //        startBurn();
    //    }
    //}
}
