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
    bool isTimed = false; //this gets set to true in overloaded init method
    bool canSwitch;
    float duration;
    float timer;
    

	// Use this for initialization
	public void init(GameController gc, List<Tile> tileList ) {
        controller = gc;
        this.tileList = tileList;
        on = false;
        tag = "Switch";
       
        timer = 0f;
        canSwitch = true;

        coll = gameObject.AddComponent<BoxCollider2D>();
        coll.isTrigger = true;

        var modelObject = new GameObject();
        model = modelObject.AddComponent<SwitchModel>();
        model.init(controller, this);
    }

    public void init(GameController gc, List<Tile> tileList, float duration)
    {
        this.isTimed = true;
        this.duration = duration;
        init(gc, tileList);
    }

    // Update is called once per frame
    void Update () {

        if (!canSwitch)
        {
            timer += Time.deltaTime;
            if (timer > duration)
            {
                canSwitch = true;
                timer = 0;
                doSwitch();
            }
        }
	
	}
    

    public void switchTime()
    {
        if (canSwitch)
        {
            if (isTimed)
            {                
                canSwitch = false;           
                
            }
            doSwitch();
        }
    }
    
    //Where the real switch happens
    void doSwitch()
    {
        on = !on;
        model.switchOwnSprite();
        if (!on)
        {
            //add back the box collider
            foreach (Tile t in tileList)
            {

                t.gameObject.GetComponent<Collider2D>().enabled = true;
                model.switchSprite(t);
                updateTag();

            }
        }
        else
        {
            foreach (Tile t in tileList)
            {
                t.gameObject.GetComponent<Collider2D>().enabled = false;
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
