using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

    const int WALL = 0;
    const int FWALL = 1;
    const int OWALL = 2;
    const int PIT = 3;

    //neighbors (north, east, south, west)
    public Tile[] neighbors = new Tile[4];

    public int type;
    public Vector2 position;
    GameController controller;
    SpriteRenderer sr;

	// Use this for initialization
	public void init(GameController gc, int type, Vector2 pos) {
        this.type = type;
        position = pos;
        controller = gc;

        Sprite[] tileSp = Resources.LoadAll<Sprite>("Sprite Sheets/env-tile");
        DestroyImmediate(GetComponent<MeshFilter>());
        DestroyImmediate(GetComponent<MeshRenderer>());
        gameObject.AddComponent<SpriteRenderer>();
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = tileSp[3];
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
