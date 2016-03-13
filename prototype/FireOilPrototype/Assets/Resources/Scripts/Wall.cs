using UnityEngine;
using System.Collections;

public class Wall : Tile {

    public GameController demo;
    public WallModel model;
    public bool on;
    public Collider2D coll;

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
       

        coll = gameObject.AddComponent<BoxCollider2D>();
        coll.isTrigger = false;
        coll.enabled = true;
    }

    public void turnOff()
    {
        if (on == true)
        {
            on = false;
            model.sr.enabled = false;
            coll.enabled = false;
        }
    }

    public void turnOn()
    {
        if (on == false)
        {
            on = true;
            model.sr.enabled = true;
            coll.enabled = true;
        }
    }

    // Update is called once per frame
    void Update () {
	
	}
}
