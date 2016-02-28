using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MotionDemo : MonoBehaviour {

    public Player p1;
    public Player p2;
    public List<Enemy> enemies;
    public List<Wall> walls;

	void Start () {
        enemies = new List<Enemy>();
        walls = new List<Wall>();
        addPlayer(0, 3, 1);
        //addPlayer(0, 1, 2);
        addEnemy(10, 3);
        addWall(5, 2);
        addWall(5, 3);
        addWall(5, 4);
    }

    private void addPlayer(float x, float y, int num)
    {
        GameObject playerObject = new GameObject();            // Create a new empty game object that will hold a gem.
        Player p = null;

        p = playerObject.AddComponent<Player>();         
        if (num == 1)
        {
            p1 = p;
        }
        if(num == 2)
        {
            p2 = p;
        }
        p.transform.position = new Vector3(x, y, 0);      // Position the gem at x,y.								
        p.name = "p" + num;

        p.init(num, this);
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
        //if (Camera.current != null)
        //{
        //    Vector3 direction = new Vector3(0, 0, 0);

        //    if (Input.GetKey("w"))
        //    {
        //        direction += new Vector3(0, 1, 0);
        //    }
        //    if (Input.GetKey("a"))
        //    {
        //        direction += new Vector3(-1, 0, 0);
        //    }
        //    if (Input.GetKey("s"))
        //    {
        //        direction += new Vector3(0, -1, 0);
        //    }
        //    if (Input.GetKey("d"))
        //    {
        //        direction += new Vector3(1, 0, 0);
        //    }

        //    Camera.current.transform.Translate(direction.normalized * Time.deltaTime);
        //}

    }
}
