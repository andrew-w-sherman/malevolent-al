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

    public void init(OilBall b)
    {
        this.b = b;
        var modelObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
        model = modelObject.AddComponent<OilModel>();
        model.init(false, null, this);

        clock = 0f; onFireTimer = 0f;
        onFire = false; spreading = false;
        spreadLimit = 0.5f;
        fireLimit = 2f;
    }

    void OnMouseUpAsButton()
    {
        setOnFire();
    }

    void setOnFire()
    {
        if (!onFire)
        {
            onFire = true;
            this.gameObject.tag = "OilPatch_OnFire";
            model.putOnFire();
        }
    }
    
    void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "OilPatch_Spreading")
        {
            setOnFire();
            /*
            if (coll.gameObject.GetComponent<OilPatch>().spreading)
            {
                setOnFire();
            }

        */
        }
        if (spreading && coll.gameObject.tag == "OilBall")
        {
            Destroy(this.gameObject);
        }
        if (coll.gameObject.tag == "FireBall") {
            setOnFire();
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
                this.gameObject.tag = "OilPatch_Spreading";
                if (onFireTimer > fireLimit + spreadLimit)
                {
                    Destroy(this.gameObject);
                }
            }
        }
	}
}
