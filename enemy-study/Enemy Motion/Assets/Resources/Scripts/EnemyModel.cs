using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyModel : MonoBehaviour {

    private MotionDemo demo;
    private Enemy owner;
    private Material mat;
    private Vector3 lastSeen;
    private Vector3 lastDirection;
    private static int NaN = 10 ^ 30;
    private static Vector3 NULL = new Vector3(NaN, NaN, NaN);
    private float lostTimer;
    private int lost;

    public float height;
    public float width;

    // Use this for initialization
    public void init(Enemy owner, MotionDemo demo)
    {
        this.demo = demo;
        this.owner = owner;
        lastSeen = NULL;
        lastDirection = NULL;
        lost = 0;
        lostTimer = 0f;

        GameObject uselessQuad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        var filter = gameObject.AddComponent<MeshFilter>();
        filter.mesh = uselessQuad.GetComponent<MeshFilter>().mesh;
        Destroy(uselessQuad);

        var renderer = gameObject.AddComponent<MeshRenderer>();
        renderer.enabled = true;

        var body = gameObject.AddComponent<Rigidbody2D>();
        body.gravityScale = 0;
        body.isKinematic = false;

        var circleCollider = gameObject.AddComponent<CircleCollider2D>();
        circleCollider.radius = (float).33;
        circleCollider.isTrigger = false;

        gameObject.tag = "enemy";
        transform.parent = owner.transform;
        transform.localPosition = new Vector3(0, 0, 0);
        name = "enemy-model";

        mat = gameObject.GetComponent<MeshRenderer>().material;
        mat.shader = Shader.Find("Transparent/Diffuse");
        mat.mainTexture = Resources.Load<Texture2D>("Textures/gem3");
        mat.color = new Color(1, 1, 1);

        width = GetComponent<Renderer>().bounds.size.x;
        height = GetComponent<Renderer>().bounds.size.y;
    }

    // Update is called once per frame
    void Update () {

        Vector2 start = new Vector2(transform.position.x, transform.position.y);

        Vector3 direction3D = demo.p1.model.transform.position - transform.position;
        Vector2 direction = new Vector2(direction3D.x, direction3D.y).normalized;

        RaycastHit2D TopLeftHit = Physics2D.Raycast(start + new Vector2(-width / 4, height / 4), direction, direction3D.magnitude);
        RaycastHit2D TopRightHit = Physics2D.Raycast(start + new Vector2(width / 4, height / 4), direction, direction3D.magnitude);
        RaycastHit2D BotLeftHit = Physics2D.Raycast(start + new Vector2(-width / 4, -height / 4), direction, direction3D.magnitude);
        RaycastHit2D BotRightHit = Physics2D.Raycast(start + new Vector2(width / 4, -height / 4), direction, direction3D.magnitude);

        List<RaycastHit2D> hitList = new List<RaycastHit2D>();
        hitList.Add(TopLeftHit);
        hitList.Add(TopRightHit);
        hitList.Add(BotLeftHit);
        hitList.Add(BotRightHit);

        foreach (RaycastHit2D hit in hitList.ToArray())
        {
            if (hit.collider == null)
            {
                hitList.Remove(hit);
            }
        }

        if(hitList.Count > 0) {
            
            int wallHit = 0;

            //Debug.Log("new set");

            foreach(RaycastHit2D hit in hitList.ToArray())
            {
                //Debug.Log(hit.collider);
                if(hit.collider.gameObject.tag == "wall")
                {
                    wallHit = 1;
                }
            }

            if (wallHit == 0)
            {
                lost = 0;
                lostTimer = 0f;
                lastSeen = demo.p1.model.transform.position;
                lastDirection = direction3D;
                transform.Translate(lastDirection.normalized * Time.deltaTime);
            }
            else
            {
                if (lastDirection != NULL)
                {
                    if (lost != 1)
                    {
                        if (transform.position != lastSeen)
                        {
                            transform.position = Vector2.MoveTowards(start, lastSeen, Time.deltaTime);
                        }
                        else
                        {
                            lost = 1;
                        }
                    }
                    if (lost == 1 && lostTimer < 3)
                    {
                        lostTimer += Time.deltaTime;
                        transform.Translate(lastDirection.normalized * Time.deltaTime);
                    }
                }
            }
        }

    }
}
