using UnityEngine;
using System.Collections;

public class FireModel : MonoBehaviour {

    private GameController demo;
    private FireBall owner;
    private Material mat;

    // Use this for initialization
    public void init(FireBall owner, GameController board)
    {
        transform.parent = owner.transform;
        transform.localPosition = new Vector3(0, 0, 0);
        name = "fire model";

        mat = owner.gameObject.GetComponent<MeshRenderer>().material;
        mat.shader = Shader.Find("Transparent/Diffuse");
        mat.mainTexture = Resources.Load<Texture2D>("Textures/fireball");
        mat.color = new Color(1, 1, 1);
    }
}
