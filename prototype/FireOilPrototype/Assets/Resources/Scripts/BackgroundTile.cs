using UnityEngine;
using System.Collections;

public class BackgroundTile : MonoBehaviour {

    // LINKING JUNK
    //neighbors (north, east, south, west)
    public Tile north, south, east, west;
    public int linkTag = 0;

    public GameController controller;
    SpriteRenderer sr;
    BoxCollider2D coll;

    public const int NO_TOGGLE = 0;
    public const int WALL = 1;
    public const int PIT = 2;

    public int type = NO_TOGGLE;

    // Use this for initialization
    public virtual void init(GameController gc) {
        controller = gc;
        tag = "plain tile";

        Sprite[] tileSp = Resources.LoadAll<Sprite>("Sprite Sheets/env-tile");
        gameObject.AddComponent<SpriteRenderer>();
        sr = GetComponent<SpriteRenderer>();
        sr.sortingOrder = 0;
        sr.sprite = tileSp[3];
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    
    public void replaceWithTile()
    {
        Vector3 pos = transform.position;
        controller.addTile(pos.x, pos.y);
    }

    // links tiles to neighbors, usually does nothing
    public virtual void link() { }

    public const int LINK_PIT = 1;
    public const int LINK_WALL = 2;
}
