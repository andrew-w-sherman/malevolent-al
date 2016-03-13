using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Switch : Tile {
    //Switches between a regular wall and the desired special wall type
    //Check Tile class for the different types of walls we have

    public bool on; //false for off, true for on (default)
    GameController controller;
    List<Tile> tileList;
    SwitchModel model;
    public Collider2D coll;

	// Use this for initialization
	public void init(GameController gc, List<Tile> tileList) {
        controller = gc;
        this.tileList = tileList;
        on = true;
        tag = "Switch";

        coll = gameObject.AddComponent<BoxCollider2D>();
        coll.isTrigger = true;

        var modelObject = new GameObject();
        model = modelObject.AddComponent<SwitchModel>();
        model.init(controller, this);
    }
	
	// Update is called once per frame
	void Update () {

        
	
	}
    
    void OnTriggerEnter2D(Collision2D coll)
    {
        //Right now it switches after hitting shoot button
        
        ////if(coll.gameObject.tag=="FireBall" || coll.gameObject.tag=="OilBall" )
        //{
        //    //print("switch hit");
        //    switchTime();
        //}        
    }

    public void switchTime()
    {
        on = !on;
        model.switchOwnSprite();
        if (on)
        {
            //add back the box collider
            foreach(Tile t in tileList)
            {
                t.gameObject.GetComponent<BoxCollider2D>().enabled = true;
                model.switchSprite(t);
                updateTag();                
            }
        }
        else
        {
            foreach (Tile t in tileList)
            {
                t.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                model.switchSprite(t);
                updateTag();
            }
        }

    }

    void updateTag()
    {
        //updates the tag depending  on type
    }
    
        


}
