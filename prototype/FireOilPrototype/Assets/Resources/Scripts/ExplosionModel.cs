using UnityEngine;
using System.Collections;

public class ExplosionModel : MonoBehaviour {

    // Use this for 
    Material mat;
    float clock;
    float maxTime;

    public void init(OilBall b) {
        transform.parent = b.transform;
        transform.localPosition = new Vector3(0, 0, 0);
        name = "Explosion Model";
        mat = GetComponent<Renderer>().material;
        mat.renderQueue = 5001;
        mat.color = new Color(1, 1, 1);
        mat.shader = Shader.Find("Transparent/Diffuse");
        mat.mainTexture = Resources.Load<Texture2D>("Textures/explosion");
        clock = 0f;
        maxTime = 1.2f;
    }
	
	// Update is called once per frame
	void Update () {
        clock = clock + Time.deltaTime;
        transform.localScale = new Vector3(clock * 5f, clock * 5f, 1);
        if(clock > maxTime)
        {
            Destroy(this.gameObject);
        }
	}
}