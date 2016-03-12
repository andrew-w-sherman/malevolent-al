using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

    float explosionTime;
    CircleCollider2D coll;
    Rigidbody2D body;
    OilBall b;
    float clock;

    public void init(OilBall b, float explosionTime)
    {
        transform.parent = b.controller.transform;

        this.explosionTime = explosionTime;
        this.b = b;
        var modelObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
        tag = "Explosion";
        ExplosionModel model = modelObject.AddComponent<ExplosionModel>();
        model.init(this, explosionTime);


        coll = gameObject.AddComponent<CircleCollider2D>();
        coll.radius = (float).33;
        coll.isTrigger = true;
        clock = 0f;
        body = gameObject.AddComponent<Rigidbody2D>();
        body.isKinematic = true;
        

    }

	// Update is called once per frame
	void Update () {
        transform.position = b.transform.position;
        clock = clock + Time.deltaTime;
        coll.radius = 2.5f * clock;
        if (clock > explosionTime)
        {
            Destroy(gameObject);
        }
    }

   
}
