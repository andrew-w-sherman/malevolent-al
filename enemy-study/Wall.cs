using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour {

    public MotionDemo demo;
    public WallModel model;

	// Use this for initialization
	public void init (MotionDemo demo) {

        this.demo = demo;
        gameObject.tag = "wall";

        var modelObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
        model = modelObject.AddComponent<WallModel>();
        model.init(this, demo);

        //var body = gameObject.AddComponent<Rigidbody2D>();
        //body.gravityScale = 0;
        //body.velocity = Vector3.zero;
        //body.isKinematic = false;

        var boxCollider = gameObject.AddComponent<BoxCollider2D>();
        boxCollider.isTrigger = false;
        boxCollider.enabled = true;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
