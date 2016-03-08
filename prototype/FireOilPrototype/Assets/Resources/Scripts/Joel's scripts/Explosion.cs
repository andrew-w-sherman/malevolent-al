using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

    float explosionTime;
    BoxCollider2D coll;
    float clock;

    public void init(OilBall b, float explosionTime)
    {
        transform.parent = b.transform;

        this.explosionTime = explosionTime;
        var modelObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
        modelObject.tag = "Explosion";
        ExplosionModel model = modelObject.AddComponent<ExplosionModel>();
        model.init(this, explosionTime);

        CircleCollider2D coll = gameObject.AddComponent<CircleCollider2D>();
        coll.radius = (float).33;
        coll.isTrigger = false;
        clock = 0f;
    }

	// Update is called once per frame
	void Update () {
        clock = clock + Time.deltaTime;
        coll.size = new Vector2(clock * 5f, clock * 5f);
        if (clock > explosionTime)
        {
            Destroy(gameObject);
        }
    }
}
