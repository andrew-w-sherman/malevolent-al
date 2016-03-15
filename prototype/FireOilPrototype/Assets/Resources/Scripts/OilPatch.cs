using UnityEngine;
using System.Collections;

public class OilPatch : MonoBehaviour {


    public GameController controller;
    public float clock;
    public float onFireTimer;
    public bool onFire;
    public bool spreading;
    float spreadLimit;
    float fireLimit;

    OilBall b;
    OilModel model;

    public void init(OilBall b, GameController controller)
    {
        this.b = b;
        this.controller = controller;

        var coll = gameObject.AddComponent<BoxCollider2D>();
        coll.isTrigger = true;

        var modelObject = new GameObject();
        model = modelObject.AddComponent<OilModel>();
        model.init(false, null, this, controller);

        clock = 0f;
        onFireTimer = 0f;
        onFire = false; spreading = false;
        spreadLimit = 0.7f;
        fireLimit = 2f;

        gameObject.tag = "OilPatch";
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "FireBall")
        {
            setOnFire();
        }
    }

    void setOnFire()
    {
        if (!onFire)
        {
            onFire = true;
            gameObject.tag = "OilPatch_OnFire";
            model.putOnFire();
        }
    }

    void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "OilPatch_Spreading")
        {
            setOnFire();
        }
        if (spreading && coll.gameObject.tag == "OilBall")
        {
            Destroy(gameObject);
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
                tag = "OilPatch_Spreading";
                if (onFireTimer > fireLimit + spreadLimit)
                {
                    Destroy(gameObject);
                }
            }
        }
        else if(clock > 5)
        {
            Destroy(gameObject);
        }
        
	}
}
