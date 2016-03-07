using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pit : MonoBehaviour {

    public GameController demo;
    public PitModel model;
    public int on;
    public float collDimensions = 1f;
    public BoxCollider2D coll;
    

    // Use this for initialization
    public void init(GameController demo)
    {

        this.demo = demo;
        gameObject.tag = "Pit";
        on = 1;

        var modelObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
        model = modelObject.AddComponent<PitModel>();
        model.init(this, demo);

        //var body = gameObject.AddComponent<Rigidbody2D>();
        //body.gravityScale = 0;
        //body.velocity = Vector3.zero;
        //body.isKinematic = false;

        coll = gameObject.AddComponent<BoxCollider2D>();
        coll.isTrigger = true;
        coll.enabled = true;
        coll.size = new Vector2(collDimensions, collDimensions);
        

    }

    public void turnOff()
    {
        if (on == 1)
        {
            on = 0;
            model.rend.enabled = false;
            coll.enabled = false;
        }
    }

    public void turnOn()
    {
        if (on == 0)
        {
            on = 1;
            model.rend.enabled = true;
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
