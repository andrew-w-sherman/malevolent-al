using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spikes : Tile {

	public GameController demo;
	public SpikesModel model;
	public float collDimensions = 1f;
	public BoxCollider2D boxColl;
	public List<Collider2D> charsToDamage;

	public bool damagingFire;
	public bool damagingOil;

	// Use this for initialization
	public override void init(GameController demo)
	{
		this.demo = demo;
		damagingFire = false;
		damagingOil = false;
		gameObject.tag = "Spikes";

		var modelObject = new GameObject();
		model = modelObject.AddComponent<SpikesModel>();
		model.init(this, demo);

		gameObject.tag = "Spikes";

		boxColl = gameObject.AddComponent<BoxCollider2D>();
		boxColl.isTrigger = true;
		boxColl.enabled = true;
		boxColl.size = new Vector2(collDimensions, collDimensions);
        coll = boxColl;

		var body = gameObject.AddComponent<Rigidbody2D>();
		body.gravityScale = 0;
		body.isKinematic = false;


	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.tag == "OilBall" ||
		    coll.gameObject.tag == "OilBall_Speeding") {
			damagingOil = true;
		}
		if (coll.gameObject.tag == "FireBall") {
			damagingFire = true;
		}
	}

	void OnTriggerExit2D(Collider2D coll)
	{
		if (coll.gameObject.tag == "OilBall" ||
			coll.gameObject.tag == "OilBall_Speeding") {
			damagingOil = false;
		}
		if (coll.gameObject.tag == "FireBall") {
			damagingFire = false;
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (damagingFire){ 
			demo.fire.damage (2); }
		if (damagingOil) {
			demo.oil.damage (2);
		}
	}
}
