using UnityEngine;
using System.Collections;

public class Pit : MonoBehaviour {

    public GameController demo;
    public PitModel model;
    public int on;

    // Use this for initialization
    public void init(GameController demo)
    {

        this.demo = demo;
        gameObject.tag = "Pit";
        on = 1;

        var modelObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
        model = modelObject.AddComponent<PitModel>();
        model.init(this, demo);

        //var body = gameObject.AddComponent<Rigidbody2D>();
        //body.gravityScale = 0;
        //body.velocity = Vector3.zero;
        //body.isKinematic = false;

        var boxCollider = gameObject.AddComponent<BoxCollider2D>();
        boxCollider.isTrigger = true;
        boxCollider.enabled = true;
        boxCollider.size = new Vector2(0.5f, 0.5f);

    }

    public void turnOff()
    {
        if (on == 1)
        {
            on = 0;
            model.rend.enabled = false;
        }
    }

    public void turnOn()
    {
        if (on == 0)
        {
            on = 1;
            model.rend.enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
