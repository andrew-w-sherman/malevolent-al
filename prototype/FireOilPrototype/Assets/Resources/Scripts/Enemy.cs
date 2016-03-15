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

        health = 1;

        var modelObject = new GameObject();
        model = modelObject.AddComponent<EnemyModel>();
        model.init(this, demo, type);
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
