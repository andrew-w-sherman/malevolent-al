using UnityEngine;
using System.Collections;

public class Wall : Tile {

    public GameController demo;
    public WallModel model;
    public BoxCollider2D boxColl;

	// Use this for initialization
	public override void init (GameController demo) {

        type = WALL;
        linkTag = LINK_WALL;
        this.demo = demo;
        gameObject.tag = "wall";
        

        var modelObject = new GameObject();
        model = modelObject.AddComponent<WallModel>();
        model.init(this, demo);

        //var body = gameObject.AddComponent<Rigidbody2D>();
        //body.gravityScale = 0;
        //body.velocity = Vector3.zero;
        //body.isKinematic = false;
       
        boxColl = gameObject.AddComponent<BoxCollider2D>();
        boxColl.isTrigger = false;
        boxColl.enabled = true;
        coll = boxColl;
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (turningOn == true)
        {
            destroyOilPatches(coll);
        }
    }

   

    public override void link() {
        float x = 1f;
        float y = 1f;
        if (north != null && south != null) x = 1.1f;
        if (east != null && west != null) y = 1.1f;
        boxColl.size = new Vector2(x, y);
    }
}
