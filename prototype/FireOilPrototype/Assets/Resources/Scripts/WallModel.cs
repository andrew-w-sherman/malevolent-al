using UnityEngine;
using System.Collections;

public class WallModel : MonoBehaviour {

    public GameController demo;
    public Wall owner;
    private Material mat;

    public void init(Wall owner, GameController demo)
    {
        this.owner = owner;
        this.demo = demo;

        transform.parent = owner.transform;                 // Set the model's parent to the gem.
        transform.localPosition = new Vector3(0, 0, 0);     // Center the model on the parent. 
        name = "wall-model";

        mat = GetComponent<Renderer>().material;
        mat.shader = Shader.Find("Transparent/Diffuse");
        mat.mainTexture = Resources.Load<Texture2D>("Textures/tileWall");
        mat.color = new Color(1, 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
