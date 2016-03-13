using UnityEngine;
using System.Collections;

public class BurnWall : Tile {

    GameController controller;
    BoxCollider2D myCollider;
    Rigidbody2D body;
    BurnWallModel model;
    public bool burning;

    //when it hits a collider call burn()

	// Use this for initialization
	public override void init (GameController gc) {
        controller = gc;
        myCollider = gameObject.AddComponent<BoxCollider2D>();
        body = gameObject.AddComponent<Rigidbody2D>();
        body.isKinematic = true;

        gameObject.tag = "BurnWall";        

        var modelObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
        model = modelObject.AddComponent<BurnWallModel>();
        model.init(this, controller);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter2D(Collision2D coll)
    {
        //print("oncolburnwall "+coll.gameObject.tag);

        if(coll.gameObject.tag == "Explosion" || coll.gameObject.tag == "FireBall")
        {
            startBurn();
            myCollider.size = new Vector2(myCollider.size.x * 1.2f, myCollider.size.y *1.2f);
        }
        if (coll.gameObject.tag == "BurnWall")
        {            
            if (coll.gameObject.GetComponent<BurnWall>().burning)
            {
                startBurn();
                myCollider.size = new Vector2(myCollider.size.x * 1.2f, myCollider.size.y * 1.2f);
            }
        }
    }

    public void replaceWithTile()
    {
        Vector3 pos = transform.position;
        controller.addTile(pos.x, pos.y);
    }
    
   

    void OnTriggerEnter2D(Collider2D col)
    {
        
        if (col.gameObject.tag == "BurnWall")
        {            
            col.gameObject.GetComponent<BurnWall>().startBurn();
        }
        if (col.gameObject.tag == "Explosion")
        {
            startBurn();
        }
    }

    public void notBurning()
    {
        Destroy(this.gameObject);
    }

    public void startBurn()
    {
        replaceWithTile();
        model.burn();
        burning = true;
        myCollider.isTrigger = true;
    }
}
