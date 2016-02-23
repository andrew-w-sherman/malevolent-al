using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{

    public PlayerModel model;

    // Use this for initialization
    public void init(MotionDemo demo)
    {
        gameObject.tag = "player";

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

        var modelObject = new GameObject();
        model = modelObject.AddComponent<PlayerModel>();
        model.init(this, demo);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("w"))
        {
            transform.position += new Vector3(0, 1, 0) * Time.deltaTime;
        }
        if (Input.GetKey("a"))
        {
            transform.position += new Vector3(-1, 0, 0) * Time.deltaTime;
        }
        if (Input.GetKey("s"))
        {
            transform.position += new Vector3(0, -1, 0) * Time.deltaTime;
        }
        if (Input.GetKey("d"))
        {
            transform.position += new Vector3(1, 0, 0) * Time.deltaTime;
        }
    }
}
