using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spikes : MonoBehaviour {

	public GameController demo;
	public SpikesModel model;
	public float collDimensions = 1f;
	public BoxCollider2D coll;
	public List<Collider2D> charsToDamage;

	public FireBall f;
	public OilBall o;
	public bool damagingFire;
	public bool damagingOil;

	// Use this for initialization
	public void init(GameController demo, OilBall o, FireBall f)
	{

		this.demo = demo;
		this.o = o;
		this.f = f;
		damagingFire = false;
		damagingOil = false;
		gameObject.tag = "Spikes";

		var modelObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
		model = modelObject.AddComponent<SpikesModel>();
		model.init(this, demo);

		gameObject.tag = "Spikes";

		coll = gameObject.AddComponent<BoxCollider2D>();
		coll.isTrigger = true;
		coll.enabled = true;
		coll.size = new Vector2(collDimensions, collDimensions);

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
			f.damage (1); }
		if (damagingOil) {
			o.damage (1);
		}
	}
}
