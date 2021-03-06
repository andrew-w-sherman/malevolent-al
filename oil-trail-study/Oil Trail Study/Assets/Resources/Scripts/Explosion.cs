﻿using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

    float explosionTime;
    BoxCollider2D coll;
    float clock;

    public void init(float explosionTime)
    {
        this.explosionTime = explosionTime;
        var modelObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
        modelObject.tag = "Explosion";
        ExplosionModel model = modelObject.AddComponent<ExplosionModel>();
        model.init(this, explosionTime);

        coll = gameObject.GetComponent<BoxCollider2D>();
        clock = 0f;
    }

	// Update is called once per frame
	void Update () {
        clock = clock + Time.deltaTime;
        coll.size = new Vector2(clock * 5f, clock * 5f);
        if (clock > explosionTime)
        {
            Destroy(this.gameObject);
        }
    }
}
