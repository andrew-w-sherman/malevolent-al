using UnityEngine;
using System.Collections;

public class SwitchModel : MonoBehaviour {

    Switch owner;
    GameController controller;
    Material mat;  
    

	public void init (GameController controller, Switch s) {
        this.owner = s;
        this.controller = controller;
        transform.parent = owner.transform;                 // Set the model's parent to the gem.
        transform.localPosition = new Vector3(0, 0, 0);     // Center the model on the parent.   
        mat = GetComponent<Renderer>().material;
        mat.shader = Shader.Find("Transparent/Diffuse");
        mat.mainTexture = Resources.Load<Texture2D>("Textures/marble"); //dummy texture for now        
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void switchSprite(Tile t)
    {
        SpriteRenderer tileSr;
        tileSr = t.gameObject.GetComponentsInChildren<SpriteRenderer>()[0];
        if (owner.on)
        {
            
            tileSr.sprite = getSprite(t.type);
        }
        else
        {
            tileSr.sprite = Resources.LoadAll<Sprite>("Sprite Sheets/env-tile")[3];
        }

    }
    //returns the sprite given a wall type
    Sprite getSprite(int type)
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprite Sheets/env-tile");

        switch (type)
        {
            case 0:
                return sprites[0];
            case 1:
                return sprites[10];
            case 2:
                return sprites[11];
            case 3:
                return sprites[4];
            default:
                return null;

        }
    }
}
