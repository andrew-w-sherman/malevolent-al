using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spikes : MonoBehaviour {

	public GameController demo;
	public SpikesModel model;
	public float collDimensions = 1f;
	public BoxCollider2D coll;


	// Use this for initialization
	public void init(GameController demo)
	{

		this.demo = demo;
		gameObject.tag = "Spikes";

		var modelObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
		model = modelObject.AddComponent<SpikesModel>();
		model.init(this, demo);

		gameObject.tag = "Spikes";

		coll = gameObject.AddComponent<BoxCollider2D>();
		coll.isTrigger = true;
		coll.enabled = true;
		coll.size = new Vector2(collDimensions, collDimensions);


	}

	void OnTriggerStay2D(Collider2D coll)
	{
		if (coll.gameObject.tag == "OilBall" || coll.gameObject.tag == "OilBall_Speeding") {
			coll.gameObject.GetComponent<OilBall> ().damage (1);
		}
		if (coll.gameObject.tag == "FireBall") {
			coll.gameObject.GetComponent<FireBall> ().damage (1);
		}
	}

	// Update is called once per frame
	void Update()
	{

	}
}
