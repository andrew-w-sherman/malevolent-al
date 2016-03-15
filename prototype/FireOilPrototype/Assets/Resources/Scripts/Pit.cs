using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pit : Tile {

    public GameController demo;
    public PitModel model;
    public float collDimensions = 1f;
    public CircleCollider2D cirColl;
    

    // Use this for initialization
    public override void init(GameController demo)
    {
        type = PIT;
        linkTag = LINK_PIT;
        this.demo = demo;
        gameObject.tag = "Pit";
        on = true;

        cirColl = gameObject.AddComponent<CircleCollider2D>();
        cirColl.radius = (float).4;
        cirColl.isTrigger = true;
        coll = cirColl;

        var modelObject = new GameObject();
        model = modelObject.AddComponent<PitModel>();
        model.init(this, demo);
        

    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (turningOn == true)
        {
            destroyOilPatches(coll);
        }

        stopTurningOn = true;
    }

    
}
