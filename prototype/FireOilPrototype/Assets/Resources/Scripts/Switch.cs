using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Switch : Tile {
    //Switches between a regular wall and the desired special wall type
    //Check Tile class for the different types of walls we have
    
    List<Tile> tileList;
    SwitchModel model;
	AudioSource audioS;
	AudioClip clickingSound;

	// Use this for initialization
	public void init(GameController gc, List<Tile> tileList) {
        controller = gc;
        this.tileList = tileList;
        on = false;
        tag = "Switch";

        coll = gameObject.AddComponent<BoxCollider2D>();
        coll.isTrigger = true;

        var modelObject = new GameObject();
        model = modelObject.AddComponent<SwitchModel>();
        model.init(controller, this);

		audioS = this.gameObject.AddComponent<AudioSource> ();
		clickingSound = Resources.Load<AudioClip>("Sound/switch");
		audioS.spatialBlend = 0.0f;
    }
	
	// Update is called once per frame
	void Update () {

        
	
	}
    

    public void switchTime()
    {
        on = !on;
        model.switchOwnSprite();
        if (!on)
        {
            //add back the box collider
            foreach(Tile t in tileList)
            {
                t.turnOn();                
            }
        }
        else
        {
            foreach (Tile t in tileList)
            {
                t.turnOff();
            }
        }
		audioS.PlayOneShot (clickingSound);
    }

    void updateTag()
    {
        //updates the tag depending  on type
    }
    
        


}
