using UnityEngine;
using System.Collections;

public class OilBall : MonoBehaviour {

    Controller c;
    BoxCollider2D coll;
    OilModel model;
    float clock;

    float movementCounter;
    float movementCheck;
    public OilPatch[] oilList;
    public int numPatches;
    float patchDistance;

    float explodeTimer;
    float explosionTime;
    float timeLastExploded;


    float speedingTime;
    float timeBeenSpeeding;
    float speedingThreshold;
    bool speeding;
    Vector3 speedDirection;

    public void init(Controller c)
    {
        this.c = c;
        coll = GetComponent<BoxCollider2D>();

        numPatches = 10;
        movementCheck = 2f;
        movementCounter = 0f;
        explodeTimer = 4f; //how long we wait between explosions
        explosionTime = 1.2f; //how long an explosion lasts
        speedingThreshold = 8f; //how fast fireball needs to be going to activate speeding attack
        timeLastExploded = -1f;
        patchDistance = coll.bounds.size.x - 0.1f;
        speedingTime = 3f; //how long we speed for
        timeBeenSpeeding = 0f;
        speeding = false;

        var modelObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
        model = modelObject.AddComponent<OilModel>();
        model.init(true, this, null);

        oilList = new OilPatch[numPatches];

        createPatch(0);


    }

    void Start()
    {
        clock = 0f;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "FireBall")
        {
            print(other.gameObject.GetComponent<FireBall>().delta);

            if (other.gameObject.GetComponent<FireBall>().speed > speedingThreshold)
            {
                speeding = true;
                timeBeenSpeeding = 0f;
                speedDirection = other.gameObject.GetComponent<FireBall>().delta;
                model.setSpeeding(true); //tell model to change color
            }
            
        }
    }

    void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "OilPatch_Spreading")
        {
            if (clock - timeLastExploded > explodeTimer)
            {
                timeLastExploded = clock;
                /*
                var modelObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
                modelObject.tag = "Explosion";
                ExplosionModel model = modelObject.AddComponent<ExplosionModel>();
                model.init(this, explosionTime);
                */
                GameObject explModel = new GameObject();
                explModel.tag = "Explosion";
                BoxCollider2D coll2 = explModel.AddComponent<BoxCollider2D>();
                explModel.SetActive(true);
                coll2.isTrigger = true;
                Explosion explosion = explModel.AddComponent<Explosion>();
                explosion.transform.position = transform.position;
                explosion.init(explosionTime);
            }
        }
    }


    //creates a new oil patch in index i of oilList[], the array of oil patches.
    void createPatch(int i)
    {
        GameObject oilPatchObject = new GameObject();
        oilPatchObject.tag = "OilPatch";
        oilList[i] = oilPatchObject.AddComponent<OilPatch>();
        CircleCollider2D coll2 = oilPatchObject.AddComponent<CircleCollider2D>();
        Rigidbody2D rig = oilPatchObject.AddComponent<Rigidbody2D>();
        oilPatchObject.SetActive(true);
        rig.mass = 10;
        rig.gravityScale = 0f;
        rig.isKinematic = true;
        coll2.isTrigger = true;
        //coll2.size = new Vector2(1, 1);
        oilList[i].transform.position = transform.position;
        oilList[i].init(this);
    }

  
    void Update () {
        clock = clock + Time.deltaTime;


        if (speeding)
        {
            timeBeenSpeeding = timeBeenSpeeding + Time.deltaTime;

            Vector3 speedMov = speedDirection * Time.deltaTime * 6;
            Vector3 yMov = new Vector3(0, 0, 0);
            Vector3 xMov = new Vector3(0, 0, 0);
            if (Input.GetButton("Vertical"))
            {
                yMov = Vector3.up * Input.GetAxis("Vertical") * Time.deltaTime * 1.5f;
            }
            if (Input.GetButton("Horizontal"))
            {
                xMov = Vector3.right * Input.GetAxis("Horizontal") * Time.deltaTime * 1.5f;
            }
            transform.Translate(speedMov + xMov + yMov);

            if (timeBeenSpeeding > speedingTime)
            {
                speeding = false;
                model.setSpeeding(false);
            }
        }
        else { 

	    if (Input.GetButton("Vertical"))
        {
            Vector3 yMov = Vector3.up * Input.GetAxis("Vertical") * Time.deltaTime * 1.5f;
            transform.Translate(yMov, Space.World);
            movementCounter += Mathf.Abs(yMov.y);
        }
        if (Input.GetButton("Horizontal"))
        {
            Vector3 xMov = Vector3.right * Input.GetAxis("Horizontal") * Time.deltaTime * 1.5f;
            transform.Translate(xMov, Space.World);
            movementCounter += Mathf.Abs(xMov.x);
        }

            //check if it's time to lay down a new patch
            if (movementCounter > patchDistance)
            {
                if (clock - timeLastExploded > 1.2f) //a check so we don't lay down oil patches right after the ball explodes
                {                                    // (we didn't explicitly discuss this, but it seemed natural to me. feel free to change)
                    movementCounter = 0f;
                    bool patchMade = false;
                    for (int i = 0; i < numPatches; i++)
                    {
                        if (!patchMade && oilList[i] == null)
                        {
                            createPatch(i);
                            patchMade = true;
                        }
                    }
                    if (!patchMade)
                    {
                        float oldestClock = -1f;
                        int oldestIndex = -1;
                        for (int i = 0; i < numPatches; i++)
                        {
                            if (oilList[i].clock > oldestClock)
                            {
                                oldestClock = oilList[i].clock;
                                oldestIndex = i;
                            }
                        }
                        if (oldestIndex != -1)
                        {
                            Destroy(oilList[oldestIndex].gameObject);
                            createPatch(oldestIndex);
                        }
                    }
                }
            }
        }
    }
}
