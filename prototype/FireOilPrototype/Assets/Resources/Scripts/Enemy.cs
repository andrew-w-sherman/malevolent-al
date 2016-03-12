using UnityEngine;
using System.Collections;

public class Enemy : Character {

    public GameController demo;
    public EnemyModel model;
    public string type;
    public int health;

    // Use this for initialization
    public void init(GameController demo, string type)
    {
        this.demo = demo;
        this.type = type;

        health = 1;

        var modelObject = new GameObject();
        model = modelObject.AddComponent<EnemyModel>();
        model.init(this, demo, type);
    }

    // Update is called once per frame
    void Update()
    {
        if (health == 0)
        {
            Destroy(gameObject);
        }
    }
}
