using UnityEngine;
using System.Collections;

public class SpikesModel : MonoBehaviour {

	public GameController demo;
	public Spikes owner;
	public Material mat;
	public SpriteRenderer sr;

	public void init(Spikes owner, GameController demo)
	{
		this.owner = owner;
		this.demo = demo;

		transform.parent = owner.transform;
		transform.localPosition = new Vector3(0, 0, 0);    
		name = "spikes-model";

		DestroyImmediate(GetComponent<MeshFilter>());
		DestroyImmediate(GetComponent<MeshRenderer>());
		gameObject.AddComponent<SpriteRenderer>();
		sr = GetComponent<SpriteRenderer>();
		sr.sortingOrder = 2;
		sr.sprite = Resources.LoadAll<Sprite> ("Sprite Sheets/env-tile") [12];
	}

	// Update is called once per frame
	void Update()
	{

	}
}
