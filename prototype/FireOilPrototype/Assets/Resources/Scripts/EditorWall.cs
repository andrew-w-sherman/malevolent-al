using UnityEngine;
using System.Collections;

public class EditorWall : MonoBehaviour {

    public GameController demo;
    public EditorWallModel model;

	// Use this for initialization
	void Start() {
        
        gameObject.tag = "wall";

        var modelObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
        model = modelObject.AddComponent<EditorWallModel>();
        model.init(this);

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
