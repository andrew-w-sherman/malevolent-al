using UnityEngine;
using System.Collections;

public class FireModel : MonoBehaviour {

    Material mat;

	public void init (FireBall b) {
        transform.parent = b.transform;
        transform.localPosition = new Vector3(0, 0, 0);
        name = "Fire Ball Model";
        mat = GetComponent<Renderer>().material;
        mat.renderQueue = 5001;
        mat.color = new Color(1, 1, 1);
        mat.shader = Shader.Find("Transparent/Diffuse");
        mat.mainTexture = Resources.Load<Texture2D>("Textures/fireball");
    }
}
