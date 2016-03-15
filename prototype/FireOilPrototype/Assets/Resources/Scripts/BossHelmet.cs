using UnityEngine;
using System.Collections;

public class BossHelmet : MonoBehaviour {

	SpriteRenderer sr;
	Sprite the_helmet;

	Material mat;
	float c;

	public void init (Boss b){

		//transform.parent = b.transform;	
		transform.position = new Vector3 (0, 0, 0);

		b.gameObject.AddComponent<SpriteRenderer> ();
		sr = b.GetComponent<SpriteRenderer> ();

		the_helmet = Resources.Load<Sprite> ("Sprite Sheets/helmet");
		sr.sprite = the_helmet;
		sr.sortingOrder = 5;
		//sr.sortingLayerName = "helmet";

		this.gameObject.name = "BOSS HEMLET";
		c = 0;
		/*
		mat = GetComponent<Renderer>().material;
		mat.shader = Shader.Find("Transparent/Diffuse");
		mat.mainTexture = Resources.Load<Texture2D>("Sprite Sheets/helmet");
		mat.color = new Color(1, 1, 1);
		mat.renderQueue = 1000000;*/
	}


	/*
	public bool moveTowards(Vector3 goal){
		
	}


	void Update(){
		c += Time.deltaTime;
		sr.sprite = the_helmet;
		transform.localPosition = new Vector3(c * 2, 0, 0);
	}
	*/
}