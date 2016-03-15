using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

    // LINKING JUNK
    //neighbors (north, east, south, west)
    public Tile north, south, east, west;
    public int linkTag = 0;
    public bool on;
    public bool turningOn;
    public bool stopTurningOn;

    public GameController controller;
    public SpriteRenderer sr;
    public Collider2D coll;

    public const int NO_TOGGLE = 0;
    public const int WALL = 1;
    public const int PIT = 2;

    public int type = NO_TOGGLE;

    // Use this for initialization
    public virtual void init(GameController gc) {
        controller = gc;
        tag = "plain tile";
        turningOn = false;
        stopTurningOn = false;

        coll = gameObject.AddComponent<BoxCollider2D>();
        coll.isTrigger = true;
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");

        Sprite[] tileSp = Resources.LoadAll<Sprite>("Sprite Sheets/env-tile");
        DestroyImmediate(GetComponent<MeshFilter>());
        DestroyImmediate(GetComponent<MeshRenderer>());
        gameObject.AddComponent<SpriteRenderer>();
        sr = GetComponent<SpriteRenderer>();
        sr.sortingOrder = 0;
        sr.sprite = tileSp[3];
    }

    public void turnOff()
    {
        on = false;
        sr.enabled = false;
        coll.enabled = false;
    }

    public void destroyOilPatches(Collider2D other)
    {

        if (other.gameObject.tag == "OilPatch_Spreading" ||
                       other.gameObject.tag == "OilPatch" ||
                       other.gameObject.tag == "OilPatch_OnFire")
        {
            Destroy(other.gameObject);
        }
       
    }

    public void turnOn()
    {
        on = true;
        turningOn = true;
        sr.enabled = true;
        coll.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (turningOn == true)
        {
            if (stopTurningOn == true)
            {
                turningOn = false;
                stopTurningOn = false;
            }
            else
            {
                stopTurningOn = true;
            }
        }
    }
 

    // links tiles to neighbors, usually does nothing
    public virtual void link() { }

    public const int LINK_PIT = 1;
    public const int LINK_WALL = 2;
}
