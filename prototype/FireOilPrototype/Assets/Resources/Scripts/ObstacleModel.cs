using UnityEngine;
using System.Collections;

public class ObstacleModel : MonoBehaviour {

    int wallType;
    Obstacle owner;
    GameController controller;
    SpriteRenderer sr;

    // Use this for initialization
    public void init(Obstacle owner, GameController gc, int wallType ) {
        this.wallType = wallType;
        this.owner = owner;
        controller = gc;

        //this.sr = owner.gameObject.AddComponent<SpriteRenderer>();
        Material mat = GetComponent<Renderer>().material;
        mat.shader = Shader.Find("Transparent/Diffuse");
        transform.parent = owner.transform;
        transform.localPosition = new Vector3(0, 0, 0);
        name = "Obstacle Model";

        if(wallType == 0)
        {
            
            mat.mainTexture = Resources.Load<Texture2D>("Textures/gem3");
        }
        else
        {
            mat.mainTexture = Resources.Load<Texture2D>("Textures/gem2");
        }


    }
	
	// Update is called once per frame
	void Update () {
	
	}

}
