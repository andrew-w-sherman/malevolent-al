using UnityEngine;
using System.Collections;

public class BossModel : MonoBehaviour {
	Material mat;
	// Use this for initialization
	void Start () {
	
	}

	public void init (Boss b){
		
		transform.parent = b.transform;	
		transform.localPosition = new Vector3 (0, 0, 0);

		mat = GetComponent<Renderer> ().material;

		mat.color = new Color (1, 1, 1);
		mat.shader = Shader.Find ("Transparent/Diffuse");

		mat.mainTexture = Resources.Load<Texture2D> ("Textures/marble");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}