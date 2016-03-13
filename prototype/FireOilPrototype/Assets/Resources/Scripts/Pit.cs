using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pit : Tile {

    public GameController demo;
    public PitModel model;
    public bool on;
    public float collDimensions = 1f;
    public CircleCollider2D coll;
    

    // Use this for initialization
    public override void init(GameController demo)
    {
        type = PIT;
        linkTag = LINK_PIT;
        this.demo = demo;
        gameObject.tag = "Pit";
        on = true;

        GameObject uselessQuad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        var filter = gameObject.AddComponent<MeshFilter>();
        filter.mesh = uselessQuad.GetComponent<MeshFilter>().mesh;
        Destroy(uselessQuad);

        var renderer = gameObject.AddComponent<MeshRenderer>();
        renderer.enabled = true;

        coll = gameObject.AddComponent<CircleCollider2D>();
        coll.radius = (float).5;
        coll.isTrigger = true;

        var modelObject = new GameObject();
        model = modelObject.AddComponent<PitModel>();
        model.init(this, demo);
        

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

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (on == true && coll.gameObject.tag == "OilPatch_Spreading" || coll.gameObject.tag == "OilPatch")
        {
            Destroy(coll.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
