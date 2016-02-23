using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MotionDemo : MonoBehaviour {

    public Player p1;
    public List<Enemy> enemies;
    public List<Wall> walls;

	void Start () {
        enemies = new List<Enemy>();
        walls = new List<Wall>();
        addPlayer1(0, 3);
        addEnemy(10, 3);
        addWall(5, 2);
        addWall(5, 3);
        addWall(5, 4);
    }

    private void addPlayer1(float x, float y)
    {
        GameObject playerObject = new GameObject();            // Create a new empty game object that will hold a gem.
        p1 = playerObject.AddComponent<Player>();            // Add the Gem.cs script to the object.

        p1.transform.position = new Vector3(x, y, 0);      // Position the gem at x,y.								
        p1.name = "p1";

        p1.init(this);
    }

    private void addEnemy(float x, float y)
    {
        GameObject enemyObject = new GameObject();            // Create a new empty game object that will hold a gem.
        Enemy e1 = enemyObject.AddComponent<Enemy>();            // Add the Gem.cs script to the object.

        e1.transform.position = new Vector3(x, y, 0);      // Position the gem at x,y.								
        e1.name = "e1";

        e1.init(this);
    }

    private void addWall(float x, float y)
    {
        GameObject wallObject = new GameObject();            // Create a new empty game object that will hold a gem.
        Wall w = wallObject.AddComponent<Wall>();            // Add the Gem.cs script to the object.

        w.transform.position = new Vector3(x, y, 0);      // Position the gem at x,y.								
        w.name = "w" + (walls.Count + 1);

        w.init(this);

        walls.Add(w);
    }

    // Update is called once per frame
    void Update () {
        //Debug.Log("p1" + p1.transform.position);
        //Debug.Log("p1-model" + p1.model.transform.position);
    }
}
