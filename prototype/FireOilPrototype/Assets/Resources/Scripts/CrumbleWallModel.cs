using UnityEngine;
using System.Collections;

public class CrumbleWallModel : MonoBehaviour {

    CrumbleWall owner;
    GameController controller;
    SpriteRenderer sr;
    float timer;

    public void init(CrumbleWall owner, GameController gc)
    {
        this.owner = owner;
        controller = gc;
        timer = 0f;
        
        sr = gameObject.AddComponent<SpriteRenderer>();
        sr.sortingOrder = 2;

        // Material mat = GetComponent<Renderer>().material;
        //mat.shader = Shader.Find("Transparent/Diffuse");
        transform.parent = owner.transform;
        transform.localPosition = new Vector3(0, 0, 0);
        name = "CrumbleWall Model";
        sr.sprite = Resources.LoadAll<Sprite>("Sprite Sheets/env-tile")[2];
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void burn()
    {

    }
}
