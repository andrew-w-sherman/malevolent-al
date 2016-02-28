using UnityEngine;
using System.Collections;

public class OilBall : MonoBehaviour {

    Controller c;
    BoxCollider2D coll;
    float clock;
    float movementCounter;
    float movementCheck;
    public OilPatch[] oilList;
    public int numPatches = 10;
    float explodeTimer;
    float timeLastExploded;
    public bool canLayPatches;

    public void init(Controller c)
    {
        this.c = c;
        oilList = new OilPatch[numPatches];
        coll = GetComponent<BoxCollider2D>();
        var modelObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
        OilModel model = modelObject.AddComponent<OilModel>();
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
