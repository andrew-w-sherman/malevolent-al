using UnityEngine;
using System.Collections;

public class BossHelmet : MonoBehaviour {

	SpriteRenderer sr;
	Sprite the_helmet;
	float c;

	public void init (Boss b){

		transform.parent = b.transform;	
		transform.localPosition = new Vector3 (0, 0, 0);

		b.gameObject.AddComponent<SpriteRenderer> ();
		sr = b.GetComponent<SpriteRenderer> ();

		the_helmet = Resources.Load<Sprite> ("Sprite Sheets/helmet");
		sr.sprite = the_helmet;
		//sr.sortingLayerName = "helmet";

		this.gameObject.name = "BOSS HEMLET";
		c = 0;
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