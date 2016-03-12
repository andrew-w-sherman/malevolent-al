using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pit : Tile {

    public GameController demo;
    public PitModel model;
    public int on;
    public float collDimensions = 1f;
    public CircleCollider2D coll;
    

    // Use this for initialization
    public override void init(GameController demo)
    {
        type = PIT;
        linkTag = LINK_PIT;
        this.demo = demo;
        gameObject.tag = "Pit";
        on = 1;

        GameObject uselessQuad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        var filter = gameObject.AddComponent<MeshFilter>();
        filter.mesh = uselessQuad.GetComponent<MeshFilter>().mesh;
        Destroy(uselessQuad);

        var renderer = gameObject.AddComponent<MeshRenderer>();
        renderer.enabled = true;

        //var body = gameObject.AddComponent<Rigidbody2D>();
        //body.gravityScale = 0;
        //body.isKinematic = false;

        coll = gameObject.AddComponent<CircleCollider2D>();
        coll.radius = (float).5;
        coll.isTrigger = true;

        var modelObject = new GameObject();
        model = modelObject.AddComponent<PitModel>();
        model.init(this, demo);

        //var body = gameObject.AddComponent<Rigidbody2D>();
        //body.gravityScale = 0;
        //body.velocity = Vector3.zero;
        //body.isKinematic = false;

        //coll = gameObject.AddComponent<BoxCollider2D>();
        //coll.isTrigger = true;
        //coll.enabled = true;
        //coll.size = new Vector2(collDimensions, collDimensions);
        

    }

    public void turnOff()
    {
        if (on == 1)
        {
            on = 0;
            model.sr.enabled = false;
            coll.enabled = false;
        }
    }

    public void turnOn()
    {
        if (on == 0)
        {
            on = 1;
            model.sr.enabled = true;
            coll.enabled = true;
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (on == 1 && coll.gameObject.tag == "OilPatch_Spreading" || coll.gameObject.tag == "OilPatch")
        {
            Destroy(coll.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
