using UnityEngine;
using System.Collections;

public class BossModel : MonoBehaviour {

	SpriteRenderer spr;
	Sprite[] faces;

	public const int ANGRY  = 0;
	public const int DAZED  = 1;
	public const int NORMAL = 2;

	public void init (Boss b){

		transform.parent = b.transform;	
		transform.localPosition = new Vector3 (0, 0, 0);

		b.gameObject.AddComponent<SpriteRenderer>();
		spr = b.GetComponent<SpriteRenderer>();


		faces = Resources.LoadAll<Sprite> ("Sprite Sheets/boss");
		spr.sprite = faces [NORMAL];
		spr.sortingOrder = 3;
		//spr.sortingLayerName = "boss";

		this.gameObject.name = "BOSS MODEL";
	}

	public void changeFace(int index){
		spr.sprite = faces [index];

	}
}