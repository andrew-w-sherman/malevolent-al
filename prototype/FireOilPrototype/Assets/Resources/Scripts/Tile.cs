using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

    const int WALL = 0;
    const int FWALL = 1;
    const int OWALL = 2;
    const int PIT = 3;

    public int type;
    public Vector2 position;
    GameController controller;
    SpriteRenderer sr;

	// Use this for initialization
	public void init(GameController gc, int type, Vector2 pos) {
        this.type = type;
        position = pos;
        controller = gc;
        //must attach a script to the current gameobject for a wall whether it is a "Wall" or "Obstacle" or "Pit"
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
