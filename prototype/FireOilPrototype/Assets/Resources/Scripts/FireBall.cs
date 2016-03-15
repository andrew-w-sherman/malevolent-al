using UnityEngine;
using System.Collections;

public class FireBall : Character
{

    public GameController controller;
    public FireModel model;
    public CircleCollider2D coll;
    public Vector3 lastDirection;

    public const float maxSpeed = 10f;
    public const float minSpeed = 3f;
    public const float speedUp = 0.3f; //the change in speed per frame if fireball is on/off an oil patch
    public const float slowDown = 0.1f;
    public bool onOil = false;
    public int charge = 0; //counter to keep track of how many times oil has shot fire

	AudioSource audioS;
	AudioClip shootSound;

    // Use this for initialization
    public void init(GameController demo)
    {
        this.controller = demo;
        startPosition = transform.position;
        lastDirection = Vector3.up;

        gameObject.tag = "FireBall";

        /*
        GameObject uselessQuad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        var filter = gameObject.AddComponent<MeshFilter>();
        filter.mesh = uselessQuad.GetComponent<MeshFilter>().mesh;
        Destroy(uselessQuad);

        var renderer = gameObject.AddComponent<MeshRenderer>();
        renderer.enabled = true;
        */

        var body = gameObject.AddComponent<Rigidbody2D>();
        body.gravityScale = 0;
        body.isKinematic = false;

        coll = gameObject.AddComponent<CircleCollider2D>();
        coll.radius = (float).33;
        coll.isTrigger = false;

        var modelObject = new GameObject();
        model = modelObject.AddComponent<FireModel>();
        model.init(this, demo);

        speed = minSpeed;

		audioS = this.gameObject.AddComponent<AudioSource> ();
		shootSound = Resources.Load<AudioClip>("Sound/fire_shoot");
		audioS.spatialBlend = 0.0f;
		//audioS.clip = shoot;

        /*
		falling = 0;
		lastDamage = -5;
		lastRegen = 0;
		health = maxHealth;
        */
    }

    void OnTriggerStay2D(Collider2D collider)
    {

        if (!falling)
        {
            pitHit(collider);
        }

        if (collider.gameObject.tag == "OilPatch" ||
            collider.gameObject.tag == "OilPatch_Spreading" ||
            collider.gameObject.tag == "OilPatch_OnFire")
        {
            onOil = true;
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        updateLastTile(other);
        pitHit(other);
        switchHit(other);

        if (other.gameObject.tag == "OilBall")
        {
            if (speed >= other.gameObject.GetComponent<OilBall>().speedingThreshold)
            {
                //print("Fireball registered a collide with oilball");
                speed = minSpeed;
            }
        } 
    }

    void OnCollisionEnter2D(Collision2D coll) //this should handle charging up for the radius attack
    {
        if (coll.gameObject.tag == "projectile-oil")
        {
            charge++;
            print("charge is " + charge);
        }
		if (coll.gameObject.tag == "projectile-enemy") {
			damage (3);
		}
    }

    /*
	void Start()
	{
		
		health = maxHealth;
	}
    */
    // Update is called once per frame
    void FixedUpdate()
    {
        //Debug.Log("Fire: " + transform.position);
        if (clock - lastDamage < damageCooldown)
        {
            model.flicker = true;
        }
        else {
            model.flicker = false;
        }
			
        if (falling)
        {
            fallSequence();
        }
        else {

            Vector3 direction = Vector3.zero;
            bool moving = false;

            if (Input.GetButton("Fire Up"))
            {
                direction += Vector3.up;
                moving = true;
            }

            if (Input.GetButton("Fire Down"))
            {
                direction += Vector3.down;
                moving = true;
            }

            if (Input.GetButton("Fire Right"))
            {
                direction += Vector3.right;
                moving = true;
            }

            if (Input.GetButton("Fire Left"))
            {
                direction += Vector3.left;
                moving = true;
            }

            if (onOil && moving)
            {
                if (speed < maxSpeed) { speed += speedUp; }
            }
            else {
                if (speed > minSpeed) { speed -= slowDown; }
            }

            if (direction != Vector3.zero)
            {
                lastDirection = direction;
                transform.position += direction.normalized * Time.deltaTime * speed;
                model.isRunning = true;
            }
            else model.isRunning = false;

            if (Input.GetButtonDown("Fire Shoot"))
            {
				audioS.PlayOneShot (shootSound);

                if (charge == 0)
                {

                    //Debug.Log(lastDirection);
                    controller.addProjectile(transform.position + lastDirection.normalized / 2, lastDirection.normalized, Projectile.FIRE, coll);

                }
                else {                                 //if we're charged, do that radius attack!
                    if (charge > 4) { charge = 4; }
                    charge *= 4;
                    float diagonalModifier = 0f;
                    if (direction.x != 0 && direction.y != 0)
                    {
                        diagonalModifier = Mathf.PI / 4f;
                    }
                    for (int i = 0; i < charge; i++)
                    {
                        float radians = (Mathf.PI * 2f / (float)charge * i) + diagonalModifier;
                        Vector3 degreeVector = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0);
                        controller.addProjectile(transform.position + lastDirection.normalized / 2, degreeVector, Projectile.FIRE, coll);

                    }
                    charge = 0;
                }
            }
            onOil = false;
        }
    }

    public override void hitGoal()
    {
        gameObject.SetActive(false);
        controller.goal(1);
    }
}
