using UnityEngine;
using System.Collections;

public class SwitchModel : MonoBehaviour {

    Switch owner;
    GameController controller;
    SpriteRenderer sr;
    

	public void init (GameController controller, Switch s) {
        this.owner = s;
        this.controller = controller;
        transform.parent = owner.transform;                 
        transform.localPosition = new Vector3(0, 0, 0);     

        sr = gameObject.AddComponent<SpriteRenderer>();
        sr.sortingOrder = 1;

        sr.sprite = Resources.LoadAll<Sprite>("Sprite Sheets/env-tile")[13];

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void switchOwnSprite()
    {
        if (owner.on) sr.sprite = Resources.LoadAll<Sprite>("Sprite Sheets/env-tile")[14];
        else sr.sprite = Resources.LoadAll<Sprite>("Sprite Sheets/env-tile")[13];
    }

    public void switchSprite(Tile t)
    {
        SpriteRenderer tileSr;
        tileSr = t.gameObject.GetComponentsInChildren<SpriteRenderer>()[0];
        if (!owner.on)
        {
            tileSr.sortingOrder = 2;
            tileSr.sprite = getSprite(t.type);
        }
        else
        {
            tileSr.sortingOrder = 0;
            tileSr.sprite = Resources.LoadAll<Sprite>("Sprite Sheets/env-tile")[3];
        }

    }
    //returns the sprite given a wall type
    Sprite getSprite(int type)
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprite Sheets/env-tile");

        switch (type)
        {
            case 1:
                return sprites[1];
            case 2:
                return sprites[5];
            default:
                return null;

        }
    }
}
