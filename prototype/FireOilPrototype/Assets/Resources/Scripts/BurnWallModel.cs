using UnityEngine;
using System.Collections;

public class BurnWallModel : MonoBehaviour {

    BurnWall owner;
    GameController controller;
    SpriteRenderer sr;
    float timer;

   

  
    public void init(BurnWall owner, GameController gc) {
        this.owner = owner;
        controller = gc;
        timer=0f;

        DestroyImmediate(GetComponent<MeshFilter>());
        DestroyImmediate(GetComponent<MeshRenderer>());
        sr = gameObject.AddComponent<SpriteRenderer>();
        sr.sortingOrder = 2;

        // Material mat = GetComponent<Renderer>().material;
        //mat.shader = Shader.Find("Transparent/Diffuse");
        transform.parent = owner.transform;
        transform.localPosition = new Vector3(0, 0, 0);
        name = "BurnWall Model";
        sr.sprite = Resources.LoadAll<Sprite>("Sprite Sheets/env-tile")[2];
    }
	
	// Update is called once per frame
	void Update () {
        if (owner.burning)
        {
            timer += Time.deltaTime;
        }
        if (timer > 2)
        {
            timer = 0f;
            owner.notBurning();
            
            sr.sprite = Resources.LoadAll<Sprite>("Sprite Sheets/env-tile")[3];
        }
	
	}

    public void burn()
    {
        //switch to fire sprite and set a timer, when time is up in update change it to a ground sprite and remove collider
        sr.sprite = Resources.LoadAll<Sprite>("Sprite Sheets/char-front")[11];
        timer = 0f;

    }
}
