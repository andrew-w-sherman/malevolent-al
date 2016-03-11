using UnityEngine;
using System.Collections;

public class Goal : Tile {

    GameController controller;
    SpriteRenderer sr;

    // Use this for initialization
    public void init(GameController gc)
    {
        controller = gc;

        Sprite[] tileSp = Resources.LoadAll<Sprite>("Sprite Sheets/env-tile");
        DestroyImmediate(GetComponent<MeshFilter>());
        DestroyImmediate(GetComponent<MeshRenderer>());
        gameObject.AddComponent<SpriteRenderer>();
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = tileSp[14];
    }

    // Update is called once per frame
    void Update () {
	
	}
}
