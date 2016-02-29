using UnityEngine;
using System.Collections;

public class ExplosionModel : MonoBehaviour {

    Material mat;
    float clock;
    float explosionTime;

    public void init(Explosion e, float explosionTime) {

        transform.parent = e.transform;
        transform.localPosition = new Vector3(0, 0, 0);
        name = "Explosion Model";
        mat = GetComponent<Renderer>().material;
        mat.renderQueue = 5000;
        mat.color = new Color(1, 1, 1);
        mat.shader = Shader.Find("Transparent/Diffuse");
        mat.mainTexture = Resources.Load<Texture2D>("Textures/explosion");
        clock = 0f;
        this.explosionTime = explosionTime;

    }
	
	void Update () {
        clock = clock + Time.deltaTime;
        transform.localScale = new Vector3(clock * 5f, clock * 5f, 1);
        if(clock > explosionTime)
        {
            Destroy(this.gameObject);
        }
	}
}