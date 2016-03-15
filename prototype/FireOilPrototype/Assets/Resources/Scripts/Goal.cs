using UnityEngine;
using System.Collections;

public class Goal : Tile {
    
    SpriteRenderer sr;
    public CircleCollider2D coll;
    public float collDimensions = 0.3f;

    // Use this for initialization
    public override void init(GameController gc)
    {
        controller = gc;

        gameObject.tag = "Goal";
        coll = gameObject.AddComponent<CircleCollider2D>();
        coll.radius = collDimensions;
        coll.isTrigger = true;

        Sprite[] tileSp = Resources.LoadAll<Sprite>("Sprite Sheets/env-tile");
        DestroyImmediate(GetComponent<MeshFilter>());
        DestroyImmediate(GetComponent<MeshRenderer>());
        gameObject.AddComponent<SpriteRenderer>();
        sr = GetComponent<SpriteRenderer>();
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
