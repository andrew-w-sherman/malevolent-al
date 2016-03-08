using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

    float explosionTime;
    CircleCollider2D coll;
    OilBall b;
    float clock;

    public void init(OilBall b, float explosionTime)
    {
        transform.parent = b.demo.transform;

        this.explosionTime = explosionTime;
        this.b = b;
        var modelObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
        modelObject.tag = "Explosion";
        ExplosionModel model = modelObject.AddComponent<ExplosionModel>();
        model.init(this, explosionTime);

<<<<<<< HEAD:prototype/FireOilPrototype/Assets/Resources/Scripts/Explosion.cs

		coll = gameObject.GetComponent<BoxCollider2D> ();
		/*
        CircleCollider2D coll = gameObject.AddComponent<CircleCollider2D>();
=======
        coll = gameObject.AddComponent<CircleCollider2D>();
>>>>>>> 69928509f7c6c8c36ba7667515ff317c33bf9456:prototype/FireOilPrototype/Assets/Resources/Scripts/Joel's scripts/Explosion.cs
        coll.radius = (float).33;
        coll.isTrigger = true;
        clock = 0f;
		*/
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
