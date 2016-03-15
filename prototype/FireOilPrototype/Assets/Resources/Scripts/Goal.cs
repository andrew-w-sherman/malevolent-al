using UnityEngine;
using System.Collections;

public class Goal : Tile {
    
    public CircleCollider2D cirColl;
    public float collDimensions = 0.3f;

    // Use this for initialization
    public override void init(GameController gc)
    {
        controller = gc;

        gameObject.tag = "Goal";
        cirColl = gameObject.AddComponent<CircleCollider2D>();
        cirColl.radius = collDimensions;
        cirColl.isTrigger = true;
        coll = cirColl;

        Sprite[] tileSp = Resources.LoadAll<Sprite>("Sprite Sheets/env-tile"); 
        sr = gameObject.AddComponent<SpriteRenderer>();
        sr.sortingOrder = 1;
        sr.sprite = tileSp[14];
    }

    // Update is called once per frame
    void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "OilPatch_Spreading" || coll.gameObject.tag == "OilPatch")
        {
            Destroy(coll.gameObject);
        }
    }
}
