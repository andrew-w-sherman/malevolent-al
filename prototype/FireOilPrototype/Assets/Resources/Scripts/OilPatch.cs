using UnityEngine;
using System.Collections;

public class OilPatch : MonoBehaviour {

    public float clock;
    public float onFireTimer;
    public bool onFire;
    public bool spreading;
    float spreadLimit;
    float fireLimit;

    OilBall b;
    int indexIAm;
    OilModel model;

    public void init(OilBall b, int i)
    {
        this.b = b;
        indexIAm = i;

        var coll = gameObject.AddComponent<CircleCollider2D>();
        coll.radius = (float).33;
        coll.isTrigger = true;

        var modelObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
        model = modelObject.AddComponent<OilModel>();
        model.init(false, null, this);

        clock = 0f; onFireTimer = 0f;
        onFire = false; spreading = false;
        spreadLimit = 0.5f;
        fireLimit = 2f;
    }

	// Use this for initialization
	void Start () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "fire ball")
        {
            setOnFire();
        }
    }

    void setOnFire()
    {
        if (!onFire)
        {
            onFire = true;
            model.putOnFire();
        }
    }
    
    void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "spreading")
        {
            setOnFire();
            /*
            if (coll.gameObject.GetComponent<OilPatch>().spreading)
            {
                setOnFire();
            }

        */
        }
        if (spreading && coll.gameObject.tag == "ball")
        {
            Destroy(this.gameObject);
        }
    }

	// Update is called once per frame
	void Update () {
        clock = clock + Time.deltaTime;
        if (onFire)
        {
            onFireTimer += Time.deltaTime;
            if (onFireTimer > spreadLimit)
            {
                spreading = true;
                this.tag = "spreading";
                if (onFireTimer > fireLimit + spreadLimit)
                {
                    Destroy(this.gameObject);
                }
            }
        }
	}
}
