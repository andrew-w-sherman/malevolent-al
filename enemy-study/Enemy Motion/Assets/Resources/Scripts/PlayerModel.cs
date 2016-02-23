using UnityEngine;
using System.Collections;

public class PlayerModel : MonoBehaviour {

    private MotionDemo demo;
    private Player owner;
    private Material mat;

    // Use this for initialization
    public void init(Player owner, MotionDemo board)
    {
        transform.parent = owner.transform;
        transform.localPosition = new Vector3(0, 0, 0);
        name = "p1-model";

        mat = owner.gameObject.GetComponent<MeshRenderer>().material;
        mat.shader = Shader.Find("Transparent/Diffuse");
        mat.mainTexture = Resources.Load<Texture2D>("Textures/marble");
        mat.color = new Color(1, 1, 1);
    }

    // Update is called once per frame
    
}
