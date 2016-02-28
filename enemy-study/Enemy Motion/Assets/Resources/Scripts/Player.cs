using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{

    public PlayerModel model;
    public int num;

    // Use this for initialization
    public void init(int num, MotionDemo demo)
    {
        this.num = num;
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
        Vector3 relativePosition = Camera.main.transform.InverseTransformDirection(transform.position - Camera.main.transform.position);
        Vector3 direction = Vector3.zero;

        if (num == 1)
        {
            if (Input.GetKey("w") && relativePosition.y < 4)
            {
                direction += new Vector3(0, 1, 0);
            }
            if (Input.GetKey("a") && relativePosition.x > -9.5)
            {
                direction += new Vector3(-1, 0, 0);
            }
            if (Input.GetKey("s") && relativePosition.y > -4)
            {
                direction += new Vector3(0, -1, 0);
            }
            if (Input.GetKey("d") && relativePosition.x < 9.5)
            {
                direction += new Vector3(1, 0, 0);
            }

            transform.position += direction.normalized * Time.deltaTime;
        }
        if (num == 2)
        {
            if (Input.GetKey("[") && relativePosition.y < 4)
            {
                direction += new Vector3(0, 1, 0);
            }
            if (Input.GetKey(";") && relativePosition.x > -9.5)
            {
                direction += new Vector3(-1, 0, 0);
            }
            if (Input.GetKey("'") && relativePosition.y > -4)
            {
                direction += new Vector3(0, -1, 0);
            }
            if (Input.GetKey("return") && relativePosition.x < 9.5)
            {
                direction += new Vector3(1, 0, 0);
            }

            transform.position += direction.normalized * Time.deltaTime;
        }
    }
}
