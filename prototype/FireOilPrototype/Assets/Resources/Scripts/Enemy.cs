using UnityEngine;
using System.Collections;

public class Enemy : Character {

    public GameController demo;
    public EnemyModel model;
    public string type;
    public bool moves;

    // Use this for initialization
    public void init(GameController demo, string type, bool moves)
    {
        this.demo = demo;
        this.type = type;
        this.moves = moves;

        tag = "enemy";

        health = 1;

        var modelObject = new GameObject();
        model = modelObject.AddComponent<EnemyModel>();
        model.init(this, demo, type);
    }

    public void enemyPitHit(Collider2D other)
    {
        //Debug.Log("hit1");
        if (other.gameObject.tag == "Pit" && touchingTiles.Count == 0)
        {
            //Debug.Log("hit2");
            if (other.gameObject.GetComponent<Pit>().on == true && speeding == false)
            {
                //Debug.Log("hit3");
                fallingInto = other;
                initialDistance = Vector2.Distance(transform.position, other.transform.position);
                whenFell = clock;
                falling = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(health);
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
