using UnityEngine;
using System.Collections;

public class FireBall : MonoBehaviour {

    public GameController demo;
    public FireModel model;
    public CircleCollider2D coll;
    public Vector3 lastDirection;

    public float maxSpeed = 10f;
    public float minSpeed = 3f;
    public float speedChange = 0.3f; //the change in speed per frame if fireball is on/off an oil patch
    public float speed;
    public bool onOil = false;

    // Use this for initialization
    public void init(GameController demo)
    {
        this.demo = demo;
        lastDirection = Vector3.up;

        gameObject.tag = "FireBall";
        
        GameObject uselessQuad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        var filter = gameObject.AddComponent<MeshFilter>();
        filter.mesh = uselessQuad.GetComponent<MeshFilter>().mesh;
        Destroy(uselessQuad);

        var renderer = gameObject.AddComponent<MeshRenderer>();
        renderer.enabled = true;

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
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "OilPatch" ||
            collider.gameObject.tag == "OilPatch_Spreading" ||
            collider.gameObject.tag == "OilPatch_OnFire")
        {
            onOil = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "OilBall")
        {
            //print("Fireball registered a collide with oilball");
            speed = minSpeed;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 relativePosition = Camera.main.transform.InverseTransformDirection(transform.position - Camera.main.transform.position);
        Vector3 direction = Vector3.zero;



        if (Input.GetButton("Fire Up"))
        {
            direction += Vector3.up;
        }

        bool moving = false;

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
            if (speed < maxSpeed) { speed += speedChange; }
        }
        else {
            if (speed > minSpeed) { speed -= speedChange; }
        }

        if (direction != Vector3.zero)
        {
            lastDirection = direction;
            transform.position += direction.normalized * Time.deltaTime * speed;
            model.isRunning = true;
        }
        else model.isRunning = false;

        if(Input.GetButtonDown("Fire Shoot"))
        {
            Debug.Log(lastDirection);
            demo.addProjectile(transform.position + lastDirection.normalized/2, lastDirection.normalized, Projectile.FIRE);
        }

        onOil = false;
    
    }
}
