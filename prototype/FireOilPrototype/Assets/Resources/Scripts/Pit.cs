using UnityEngine;
using System.Collections;

public class Pit : MonoBehaviour {

    public GameController demo;
    public PitModel model;
    public int filled;

    // Use this for initialization
    public void init(GameController demo)
    {

        this.demo = demo;
        gameObject.tag = "Pit";
        filled = 0;

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

    public void fill()
    {
        if (filled == 0)
        {
            filled = 1;
            model.rend.enabled = false;
        }
    }

    public void empty()
    {
        if (filled == 1)
        {
            filled = 0;
            model.rend.enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
