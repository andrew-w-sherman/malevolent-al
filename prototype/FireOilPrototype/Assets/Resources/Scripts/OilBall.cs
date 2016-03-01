using UnityEngine;
using System.Collections;

public class OilBall : MonoBehaviour {

    GameController demo;
    public CircleCollider2D coll;
    public OilModel model;
    float clock;
    Vector3 lastDirection;

    float movementCounter = 0f;
    float movementCheck = 2f;
    public OilPatch[] oilList;
    public int numPatches = 10;
    float patchDistance;

    float explodeTimer = 4f;//how long we wait between explosions
    float explosionTime = 1.2f; //how long an explosion lasts
    float timeLastExploded = -1f; //


    float speedingTime = 3f; //how long we speed for
    float timeBeenSpeeding = 0f;
    float speedingThreshold = 8f; //how fast fireball needs to be going to activate speeding attack
    bool speeding = false;
    Vector3 speedDirection;

    public void init(GameController demo)
    {
        this.demo = demo;
        lastDirection = Vector3.up;
        oilList = new OilPatch[numPatches];

        gameObject.tag = "OilBall";

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

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "FireBall")
        {
            //print(other.gameObject.GetComponent<FireBall>().lastDirection);
            //print(other.gameObject.GetComponent<FireBall>().speed);

            if (other.gameObject.GetComponent<FireBall>().speed > speedingThreshold)
            {
                print("Time to speed!");
                speeding = true;
                timeBeenSpeeding = 0f;
                speedDirection = other.gameObject.GetComponent<FireBall>().lastDirection ;
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
                GameObject explModel = new GameObject();
                explModel.tag = "Explosion";
                BoxCollider2D coll2 = explModel.AddComponent<BoxCollider2D>();
                explModel.SetActive(true);
                coll2.isTrigger = true;
                Explosion explosion = explModel.AddComponent<Explosion>();
                explosion.transform.position = transform.position;
                explosion.init(this, explosionTime);
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
        //coll2.size = new Vector2(1, 1);
        oilList[i].transform.position = transform.position;
        oilList[i].init(this);
    }

    // Update is called once per frame
    void Update () {
        clock = clock + Time.deltaTime;

        Vector3 relativePosition = Camera.main.transform.InverseTransformDirection(transform.position - Camera.main.transform.position);
        Vector3 direction = Vector3.zero;
        
        if (Input.GetButton("Oil Up") && relativePosition.y < 4)
        {
            direction += Vector3.up;
        }

        if (Input.GetButton("Oil Down") && relativePosition.y > -4)
        {
            direction += Vector3.down;
        }

        if (Input.GetButton("Oil Right") && relativePosition.x < 9.5)
        {
            direction += Vector3.right;
        }

        if (Input.GetButton("Oil Left") && relativePosition.x > -9.5)
        {
            direction += Vector3.left;
        }

        lastDirection = direction;
        transform.position += direction.normalized * Time.deltaTime;
        if (speeding)
        {
            timeBeenSpeeding += Time.deltaTime;
            transform.position += speedDirection * Time.deltaTime * 6;
            if (timeBeenSpeeding > speedingTime)
            {
                speeding = false;
                model.setSpeeding(false);
            }
        }
        movementCounter += (direction.normalized * Time.deltaTime).magnitude;

        if (Input.GetButtonDown("Oil Shoot"))
        {
            demo.addProjectile(transform.position + lastDirection.normalized / 2, lastDirection.normalized, Projectile.OIL);
        }

        

        if (movementCounter > coll.bounds.size.x - 0.1f)
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
