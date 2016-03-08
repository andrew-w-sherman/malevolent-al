using UnityEngine;
using System.Collections;

public class PitModel : MonoBehaviour {

    public GameController demo;
    public Pit owner;
    public Material mat;
    public Renderer rend;

    public void init(Pit owner, GameController demo)
    {
        this.owner = owner;
        this.demo = demo;

        transform.parent = owner.transform;                 // Set the model's parent to the gem.
        transform.localPosition = new Vector3(0, 0, 0);     // Center the model on the parent. 
        name = "pit-model";

        mat = GetComponent<Renderer>().material;
        mat.shader = Shader.Find("Transparent/Diffuse");
        mat.mainTexture = Resources.Load<Texture2D>("Textures/Solid_black");
        mat.color = new Color(1, 1, 1);

        rend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
