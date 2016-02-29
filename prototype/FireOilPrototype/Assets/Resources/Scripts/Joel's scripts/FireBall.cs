using UnityEngine;
using System.Collections;

public class FireBall : MonoBehaviour {

    Controller c;
    public Vector3 delta;
    float clock;
    public float speed;
    float maxSpeed;
    float minSpeed;
    float speedChange;
    bool onOil;

    public void init(Controller c)
    {
        this.c = c;

        var fireModelObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
        FireModel model = fireModelObject.AddComponent<FireModel>();
        model.init(this);

        maxSpeed = 10f;
        minSpeed = 1.5f;
        speedChange = 0.3f; //change in speed when Fireball is on/off an oil patch
        speed = minSpeed;
    }

	// Use this for initialization
	void Start () {
        clock = 0f;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "OilBall")
        {
            //print("Fireball registered a collide with oilball");
            speed = minSpeed;
        }
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
    
	
	// Update is called once per frame
	void Update () {

        delta = new Vector3(0, 0, 0);

        clock = clock + Time.deltaTime;

        if (onOil)
        {
            if (speed < maxSpeed) { speed += speedChange; }
        } else {
            if (speed > minSpeed) { speed -= speedChange; }
        }

        if (Input.GetButton("Vertical2"))
        {
            transform.Translate(Vector3.up * Input.GetAxis("Vertical2") * Time.deltaTime * speed, Space.World);
            delta += new Vector3(0, 1 * Input.GetAxis("Vertical2"), 0);
        }
        if (Input.GetButton("Horizontal2"))
        {
            transform.Translate(Vector3.right * Input.GetAxis("Horizontal2") * Time.deltaTime * speed, Space.World);
            delta += new Vector3(1 * Input.GetAxis("Horizontal2"), 0, 0);
        }

        onOil = false;
    }
}
