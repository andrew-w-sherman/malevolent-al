using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GameController : MonoBehaviour {

    public FireBall fire;
    public OilBall oil;
    public List<Enemy> enemies;
    public List<Wall> walls;
    public List<Projectile> projectiles;
    public int projectileCount;
    public float clock;
    public int addEnemyInterval = 5;
    public int whenAddEnemy;
    public int whichAddEnemy;
    public Camera cam;
    public float minCamSize;

    public const int NaN = 10 ^ 30;
    public static Vector3 NULL = new Vector3(NaN, NaN, NaN);

    void Start () {
        projectileCount = 0;
        clock = 0f;
        whenAddEnemy = 0;
        whichAddEnemy = 0;

        enemies = new List<Enemy>();
        walls = new List<Wall>();
        addFire(0, 3);
        addOil(0, 1);

        Pit pit = addPit(-2, 1);
        pit.fill();
        pit.empty();

        cam = Camera.main;
        minCamSize = cam.orthographicSize;

    }

    private void addEnemyPeriodically()
    {
        if (clock > whenAddEnemy)
        {
            if (whichAddEnemy == 0)
            {
                addEnemy(10, 3, "fire");
                whenAddEnemy += addEnemyInterval;
                whichAddEnemy = 1;
            }
            else if (whichAddEnemy == 1)
            {
                addEnemy(10, 3, "oil");
                whenAddEnemy += addEnemyInterval;
                whichAddEnemy = 0;
            }
        }
    }

    private void addFire(float x, float y)
    {
        GameObject playerObject = new GameObject();            // Create a new empty game object that will hold a gem.

        fire = playerObject.AddComponent<FireBall>();         
        fire.transform.position = new Vector3(x, y, 0);      // Position the gem at x,y.								
        fire.name = "Fire Ball";

        fire.init(this);
    }

    private void addOil(float x, float y)
    {
        GameObject playerObject = new GameObject();            

        oil = playerObject.AddComponent<OilBall>();
        oil.transform.position = new Vector3(x, y, 0);     							
        oil.name = "Oil Ball";

        oil.init(this);
    }

    private void addEnemy(float x, float y, string type)
    {
        GameObject enemyObject = new GameObject();            
        Enemy e1 = enemyObject.AddComponent<Enemy>();           

        e1.transform.position = new Vector3(x, y, 0);      								
        e1.name = "e1";

        e1.init(this, type);
    }

    private void addWall(float x, float y)
    {
        GameObject wallObject = new GameObject();            // Create a new empty game object that will hold a gem.
        Wall w = wallObject.AddComponent<Wall>();            // Add the Gem.cs script to the object.

        w.transform.position = new Vector3(x, y, 0);      // Position the gem at x,y.								
        w.name = "Wall" + (walls.Count + 1);

        w.init(this);

        walls.Add(w);
    }

    private Pit addPit(float x, float y)
    {
        GameObject pitObject = new GameObject();            // Create a new empty game object that will hold a gem.
        Pit pit = pitObject.AddComponent<Pit>();            // Add the Gem.cs script to the object.

        pit.transform.position = new Vector3(x, y, 0);      // Position the gem at x,y.								
       // pit.name = "Pit" + (walls.Count + 1);

        pit.init(this);

        return pit;
        
    }

    public void addProjectile(Vector3 start, Vector3 velocity, int type)
    {
        GameObject projectileObject = new GameObject();            
        Projectile p = projectileObject.AddComponent<Projectile>();            

        p.transform.position = start;      							
        p.name = "Projectile " + (projectileCount + 1);
        projectileCount++;

        p.init(start, velocity, type, this);

        projectiles.Add(p);
    }

    private void updateCamera()
    {
        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;
        float camX = cam.transform.position.x;
        float camY = cam.transform.position.y;
        float fireX = fire.transform.position.x;
        float fireY = fire.transform.position.y;
        float oilX = oil.transform.position.x;
        float oilY = oil.transform.position.y;
        float whenIncreaseSizeX = 5f;
        float whenIncreaseSizeY = 3f;
        float whenDecreaseSizeX = 5f;
        float whenDecreaseSizeY = 3f;
        float camDelta = 0f;
        float minCamDelta = 0.005f;

        if (cam != null)
        {
            Vector3 direction = oil.transform.position - fire.transform.position;
            Vector3 halfwayPoint = fire.transform.position + (direction / 2) + new Vector3(0, 0, -10);
            cam.transform.position = halfwayPoint;

            if(fireX > camX + width/2 - whenIncreaseSizeX || fireX < camX - width/2 + whenIncreaseSizeX ||
               fireY > camY + height/2 - whenIncreaseSizeY || fireY < camY - height/2 + whenIncreaseSizeY ||
               oilX > camX + width / 2 - whenIncreaseSizeX || oilX < camX - width / 2 + whenIncreaseSizeX ||
               oilY > camY + height / 2 - whenIncreaseSizeY || oilY < camY - height / 2 + whenIncreaseSizeY)
            {
                while (fireX > camX + width / 2 - whenIncreaseSizeX || fireX < camX - width / 2 + whenIncreaseSizeX ||
                       fireY > camY + height / 2 - whenIncreaseSizeY || fireY < camY - height / 2 + whenIncreaseSizeY ||
                       oilX > camX + width / 2 - whenIncreaseSizeX || oilX < camX - width / 2 + whenIncreaseSizeX ||
                       oilY > camY + height / 2 - whenIncreaseSizeY || oilY < camY - height / 2 + whenIncreaseSizeY)
                {
                    camDelta += minCamDelta;
                    height = 2f * (cam.orthographicSize + camDelta);
                    width = height * cam.aspect;
                }

                camDelta -= minCamDelta;
            }
            else if(cam.orthographicSize > minCamSize)
            {
                if(fireX < camX + width / 2 - whenDecreaseSizeX && fireX > camX - width / 2 + whenDecreaseSizeX &&
                   fireY < camY + height / 2 - whenDecreaseSizeY && fireY > camY - height / 2 + whenDecreaseSizeY &&
                   oilX < camX + width / 2 - whenDecreaseSizeX && oilX > camX - width / 2 + whenDecreaseSizeX &&
                   oilY < camY + height / 2 - whenDecreaseSizeY && oilY > camY - height / 2 + whenDecreaseSizeY)
                {
                    while (fireX < camX + width / 2 - whenDecreaseSizeX && fireX > camX - width / 2 + whenDecreaseSizeX &&
                           fireY < camY + height / 2 - whenDecreaseSizeY && fireY > camY - height / 2 + whenDecreaseSizeY &&
                           oilX < camX + width / 2 - whenDecreaseSizeX && oilX > camX - width / 2 + whenDecreaseSizeX &&
                           oilY < camY + height / 2 - whenDecreaseSizeY && oilY > camY - height / 2 + whenDecreaseSizeY
                           && cam.orthographicSize + camDelta >= minCamSize)
                    {
                        camDelta -= minCamDelta;
                        height = 2f * (cam.orthographicSize + camDelta);
                        width = height * cam.aspect;
                    }

                    camDelta += minCamDelta;
                }
            }
        }

        cam.orthographicSize += camDelta;
    }

    // Update is called once per frame
    void Update () {

        clock += Time.deltaTime;
        updateCamera();
        //addEnemyPeriodically();
    }
}
