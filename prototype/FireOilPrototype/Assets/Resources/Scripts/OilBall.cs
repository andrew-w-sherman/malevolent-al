using UnityEngine;
using System.Collections;

public class OilBall : MonoBehaviour {

    public GameController demo;
    public CircleCollider2D coll;
    public OilModel model;
    public Vector3 lastDirection;
    float clock;
    float movementCounter;
    float movementCheck;
    public OilPatch[] oilList;
    public int numPatches = 10;
    float explodeTimer;
    float timeLastExploded;
    public bool canLayPatches;

    public void init(GameController demo)
    {
        this.demo = demo;
        lastDirection = Vector3.up;
        oilList = new OilPatch[numPatches];

        gameObject.tag = "oil ball";

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

        //coll = GetComponent<CircleCollider2D>();

        var modelObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
        model = modelObject.AddComponent<OilModel>();
        model.init(true, this, null);
        movementCheck = 2f;
        movementCounter = 0f;
        oilList = new OilPatch[numPatches];

        createPatch(0);

        explodeTimer = 4f;
        timeLastExploded = -1f;
    }

    void Start()
    {
        clock = 0f;
    }


    void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "spreading")
        {
            if (clock - timeLastExploded > explodeTimer)
            {
                print("boom");
                timeLastExploded = clock;
                var modelObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
                ExplosionModel model = modelObject.AddComponent<ExplosionModel>();
                model.init(this);
            }
        }
    }


    void createPatch(int i)
    {
        GameObject oilPatchObject = new GameObject();
        oilPatchObject.tag = "patch";
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
        oilList[i].init(this, i);
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
