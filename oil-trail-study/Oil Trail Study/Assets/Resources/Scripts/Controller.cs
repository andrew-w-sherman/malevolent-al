using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {

    public OilPatch[] oilList;
    GameObject oilBlobObject;
	void Start () {
        oilList = new OilPatch[10];

        oilBlobObject = new GameObject();
        oilBlobObject.tag = "ball";
        BoxCollider2D coll = oilBlobObject.AddComponent<BoxCollider2D>();
        coll.size = new Vector2(1, 1);
        OilBall oilball = oilBlobObject.AddComponent<OilBall>();
        oilball.transform.position = new Vector3(0, 0, 0);
        oilball.init(this);
	}
	
	// Update is called once per frame
	void Update () {
	    
	}
}
