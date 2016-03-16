using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

    public const int FIRE = 0;
    public const int OIL = 1;
    public const int ENEMY = 2;

    public int speedUp = 5;

    public GameController demo;
    public int type;
    public Vector3 velocity;
    public CircleCollider2D coll;
    

    int spriteswitch = 1;
    string spritename;

    Material mat;

    // Use this for initialization
    public void init(Vector3 startPos, Vector3 velocity, int type, GameController demo, Collider2D shooter)
    {
        this.demo = demo;
        this.type = type;
        transform.position = startPos;
        this.velocity = velocity;
        if (type == 2) this.tag = "projectile-enemy";
		if (type == 0) {
			this.tag = "projectile-fire";
		}
		if (type == 1){
			this.tag = "projectile-oil";
		}
		
        setSpriteOrientation();

        GameObject uselessQuad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        var filter = gameObject.AddComponent<MeshFilter>();
        filter.mesh = uselessQuad.GetComponent<MeshFilter>().mesh;
        Destroy(uselessQuad);

        var renderer = gameObject.AddComponent<MeshRenderer>();
        renderer.enabled = true;
        renderer.sortingOrder = 5;

        var body = gameObject.AddComponent<Rigidbody2D>();
        body.gravityScale = 0;
        body.isKinematic = false;

        coll = gameObject.AddComponent<CircleCollider2D>();
        coll.radius = (float).1;
        coll.isTrigger = false;

        if (type == FIRE)
        {
            Physics2D.IgnoreCollision(coll, demo.fire.coll);
            Physics2D.IgnoreCollision(coll, demo.oil.coll);
		} else if (type == OIL)
        {
            Physics2D.IgnoreCollision(coll, demo.oil.coll);
        }

        Physics2D.IgnoreCollision(coll, shooter);

        mat = GetComponent<Renderer>().material;
        mat.shader = Shader.Find("Sprites/Default");
        
        switch (type)
        {
            case FIRE:
                spritename = "proj_fire"; break;
            case OIL:
                spritename = "proj_oil"; break;
            case ENEMY:
                spritename = "proj_enemy"; break;
        }
        mat.mainTexture = Resources.Load<Texture2D>("Textures/" + spritename + 1);
    }

    private void setSpriteOrientation()
    {
        if(velocity.normalized == Vector3.right)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        if (velocity.normalized == (Vector3.right + Vector3.up).normalized)
        {
            transform.eulerAngles = new Vector3(0, 0, 45);
        }
        if (velocity.normalized == Vector3.up)
        {
            transform.eulerAngles = new Vector3(0, 0, 90);
        }
        if (velocity.normalized == (Vector3.up + Vector3.left).normalized)
        {
            transform.eulerAngles = new Vector3(0, 0, 135);
        }
        if (velocity.normalized == Vector3.left)
        {
            transform.eulerAngles = new Vector3(0, 0, 180);
        }
        if (velocity.normalized == (Vector3.left + Vector3.down).normalized)
        {
            transform.eulerAngles = new Vector3(0, 0, 225);
        }
        if (velocity.normalized == Vector3.down)
        {
            transform.eulerAngles = new Vector3(0, 0, 270);
        }
        if (velocity.normalized == (Vector3.down + Vector3.right).normalized)
        {
            transform.eulerAngles = new Vector3(0, 0, 315);
        }
    }
	
	// Update is called once per frame
	void Update () {
        // move!
        transform.position += velocity * Time.deltaTime * speedUp;
        mat.mainTexture = Resources.Load<Texture2D>("Textures/" + spritename + spriteswitch);
        if (spriteswitch == 2) spriteswitch = 1;
        else spriteswitch = 2;
	}

    void OnCollisionEnter2D(Collision2D coll)
    {
        string tagOther = coll.gameObject.tag;
		/*
		if (tagOther == "OilBall" || tagOther == "OilBall_Speeding") {
			gameObject.GetComponent<OilBall> ().damage (1);
		}
		if (tagOther == "FireBall") {
			gameObject.GetComponent<FireBall> ().damage (1);
		}
		*/

		if (tagOther != "projectile-fire" && tagOther != "projectile-oil" && tagOther != "projectile-enemy")
        {
            Destroy(this);
            Destroy(gameObject);
        }
    }
}
