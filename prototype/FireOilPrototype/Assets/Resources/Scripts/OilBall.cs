using UnityEngine;
using System.Collections;

public class OilBall : Character
{

    public GameController controller;
    public CircleCollider2D coll;
    public OilModel model;
    public float speed;
    public float maxSpeed = 10f;
    public float minSpeed = 1.5f;
    Vector3 lastDirection;

    float movementCounter = 0f;
    float movementCheck = 2f;
    public OilPatch[] oilList;
    public int numPatches = 10;
    float patchDistance;

    float explodeTimer = 7f;//how long we wait between explosions
    float explosionTime = 1.2f; //how long an explosion lasts
    float timeLastExploded; //just to make sure we can explode right at the start of a level

    float speedingTime = 3f; //how long we speed for
    float timeBeenSpeeding = 0f;
    public float speedingThreshold = 8f; //how fast fireball needs to be going to activate speeding attack
    Vector3 speedDirection;
    Rigidbody2D body;

    public bool shootButtonDown;

	AudioSource audioS;
	AudioClip shootSound;
	AudioClip explodeSound;

    public void init(GameController demo)
    {

        timeLastExploded = -explodeTimer;
        this.controller = demo;
        startPosition = transform.position;
        lastDirection = Vector3.up;
        oilList = new OilPatch[numPatches];
        shootButtonDown = false;

        speed = 3;


        gameObject.tag = "OilBall";

        /*
        GameObject uselessQuad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        var filter = gameObject.AddComponent<MeshFilter>();
        filter.mesh = uselessQuad.GetComponent<MeshFilter>().mesh;
        Destroy(uselessQuad);

        var renderer = gameObject.AddComponent<MeshRenderer>();
        renderer.enabled = true;
        */

        body = gameObject.AddComponent<Rigidbody2D>();
        body.gravityScale = 0;
        body.isKinematic = false;


        coll = gameObject.AddComponent<CircleCollider2D>();
        coll.radius = (float).33;
        coll.isTrigger = false;


        var modelObject = new GameObject();
        model = modelObject.AddComponent<OilModel>();
        model.init(true, this, null, controller);

        oilList = new OilPatch[numPatches];

        createPatch(0);

		audioS = this.gameObject.AddComponent<AudioSource> ();
		shootSound = Resources.Load<AudioClip>("Sound/oil_shoot");
		explodeSound = Resources.Load<AudioClip> ("Sound/oil_explode");
		audioS.spatialBlend = 0.0f;
    }

    /* don't use probably
    public void destroyTrail()
    {
        foreach (OilPatch patch in oilList)
        {
            Destroy(patch.gameObject);
        }
    }
    */

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "FireBall")
        {
            //print(other.gameObject.GetComponent<FireBall>().lastDirection);
            //print(other.gameObject.GetComponent<FireBall>().speed);

            if (other.gameObject.GetComponent<FireBall>().speed > speedingThreshold)
            {
                //print("Time to speed!");

                if (controller.expl != null)
                {
                    Destroy(controller.expl.gameObject);
                }

                speeding = true;
                timeBeenSpeeding = 0f;
                speedDirection = other.gameObject.GetComponent<FireBall>().lastDirection;
                model.setSpeeding(true); //tell model to change color
                body.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
                gameObject.tag = "OilBall_Speeding";
				audioS.PlayOneShot (explodeSound);
            }
        }
        else if (other.gameObject.tag == "wall")
        {

            //print(other.contacts[0].normal);

            speedDirection = Vector3.Reflect(speedDirection, other.contacts[0].normal);
            //print(speedDirection);

            /*print("hit wall");
            Vector3 wallNormal;
            Ray MyRay;
            RaycastHit MyRayHit;
            Vector3 direction = (other.gameObject.transform.position - transform.position).normalized;
            MyRay = new Ray(other.gameObject.transform.position, direction);
            if (Physics.Raycast(MyRay, out MyRayHit))
            {
                print("if phys");

                if (MyRayHit.collider != null)
                {
                    Vector3 contactPoint = other.contacts[0].point;
                    Vector3 center = collider.bounds.center;

                    if( contactPoint.x > center.x)
                    {

                    }
                    bool top = contactPoint.y > center.y;

                    
                    Vector3 MyNormal = MyRayHit.normal;
                    MyNormal = MyRayHit.transform.TransformDirection(MyNormal);
                    print(MyNormal.ToString());
                    
                                        if (MyNormal == MyRayHit.transform.up) {  }
                                        if (MyNormal == -MyRayHit.transform.up) { wallNormal =  }
                                        if (MyNormal == MyRayHit.transform.forward) {  }
                                        if (MyNormal == -MyRayHit.transform.forward) { hitDirection = HitDirection.Back; }
                                        if (MyNormal == MyRayHit.transform.right) { hitDirection = HitDirection.Right; }
                                        if (MyNormal == -MyRayHit.transform.right) { hitDirection = HitDirection.Left; }
                                        
                                        

                    speedDirection = Vector3.Reflect(other.relativeVelocity, MyNormal);
                }
            }
            */
        }
		if (other.gameObject.tag == "projectile-enemy") {
			damage (1);
		}
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!speeding)
        {
            updateLastTile(other);
        }
        pitHit(other);
        switchHit(other);
    }




    void OnTriggerStay2D(Collider2D coll)
    {

        if (!falling)
        {
            pitHit(coll);
        }

        if (coll.gameObject.tag == "OilPatch_OnFire" ||
            coll.gameObject.tag == "OilPatch_Spreading")
        {
            if (clock - timeLastExploded > explodeTimer && !speeding)
            {

                timeLastExploded = clock;
                GameObject explModel = new GameObject();
                explModel.tag = "Explosion";
                BoxCollider2D coll2 = explModel.AddComponent<BoxCollider2D>();
                explModel.SetActive(true);
                coll2.isTrigger = true;
                Explosion explosion = explModel.AddComponent<Explosion>();
                explosion.transform.position = transform.position;
                explosion.init(this, explosionTime);
                controller.expl = explosion;
				audioS.PlayOneShot (explodeSound);
            }
        }
    }


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
        oilList[i].transform.position = transform.position;
        oilList[i].init(this, controller);
    }

    // Update is called once per frame
    void FixedUpdate()
    {


        if (falling)
        {
            fallSequence();
        }

        else {

            if (clock - lastDamage < damageCooldown)
            {
                model.flicker = true;
            }
            else {
                model.flicker = false;
            }

            Vector3 direction = Vector3.zero;

            if (Input.GetButton("Oil Up"))
            {
                direction += Vector3.up;
            }

            if (Input.GetButton("Oil Down"))
            {
                direction += Vector3.down;
            }

            if (Input.GetButton("Oil Right"))
            {
                direction += Vector3.right;
            }

            if (Input.GetButton("Oil Left"))
            {
                direction += Vector3.left;
            }
            if (direction != Vector3.zero)
            {
                lastDirection = direction;
                transform.position += direction.normalized * Time.deltaTime * speed;
                model.isRunning = true;
            }
            else model.isRunning = false;

            if (speeding)
            {
                timeBeenSpeeding += Time.deltaTime;
                float s_speed = 6f;
                if (timeBeenSpeeding > (speedingTime * 7f / 8f))
                {
                    s_speed -= ((timeBeenSpeeding - (speedingTime * 7f / 8f)) / (speedingTime / 8f)) * 3f;
                }
                transform.position += speedDirection * Time.deltaTime * s_speed;
                if (timeBeenSpeeding > speedingTime)
                {
                    speeding = false;
                    model.setSpeeding(false);
                    gameObject.tag = "OilBall";
                    body.collisionDetectionMode = CollisionDetectionMode2D.Discrete;
                }
            }

            if (Input.GetButtonDown("Oil Shoot"))
            {
                controller.addProjectile(transform.position + lastDirection.normalized / 2, lastDirection.normalized, Projectile.OIL);
                shootButtonDown = true;
				audioS.PlayOneShot (shootSound);
            }
            else if (Input.GetButtonUp("Oil Shoot"))
            {
                shootButtonDown = false;
            }

            if (!shootButtonDown)
            {
                movementCounter += (direction.normalized * Time.deltaTime).magnitude;
            }


            if (movementCounter > (coll.radius * 1f / 2f) && !speeding)
            {
                if (clock - timeLastExploded > 1.2f)
                {
                    movementCounter = 0f;
                    int x = 0;
                    bool patchMade = false;
                    for (int i = 0; i < numPatches; i++)
                    {
                        if (!patchMade && oilList[i] == null)
                        {
                            createPatch(i);
                            patchMade = true;
                        }
                        else { x++; }
                    }
                    if (x == numPatches)
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
                        else { print("oldestIndex set incorrectly"); }
                    }
                }
            }
        }
    }

    public override void hitGoal()
    {
        gameObject.SetActive(false);
        controller.goal(2);
    }

    void OnDestroy()
    {
        foreach (OilPatch patch in oilList)
        {
            if (patch != null) Destroy(patch.gameObject);
        }
    }
}
