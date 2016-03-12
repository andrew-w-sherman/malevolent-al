using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public bool DEBUG_LVL;
    readonly string[] LEVELS = { "test" };
    int levelIndex;

    public GameObject boardGO;
    public Board board;
    
    public bool menuShowing;

    public FireBall fire;
    public OilBall oil;
    public List<Enemy> enemies;
    public List<Wall> walls;
    public List<Projectile> projectiles;
    public List<Pit> pits;
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

        addFire(0, 3);
        addOil(0, 1);
        cam = Camera.main;
        minCamSize = cam.orthographicSize;

        if (DEBUG_LVL)
        {
            enemies = new List<Enemy>();
            walls = new List<Wall>();
            pits = new List<Pit>();
            

            //addTurret(0, -2);
            //addObstacle(0, -2, 0);


            Pit pit = null;
            for (int i = -7; i < 9; i++)
            {
                pit = addPit(-2, i);

                //if (i == 1 || i == 2 || i == 3)
                //{
                //    pit.turnOff();
                //}

            }

            addSpikes(0, 6);

            addEnemy(-4, 1, "fire");
            addEnemy(-4, 3, "oil");
            //Pit pit = addPit(-2, 1);
            //pit.turnOff();
            //pit.turnOn();
            addCrumbleWall(1, -3);
            addCrumbleWall(2, -3);
            addCrumbleWall(3, -3);
            addCrumbleWall(4, -3);
            
        }
        else
        {
            // we assume we're working from 
            levelIndex = 0;
            boardGO = new GameObject();
            board = boardGO.AddComponent<Board>();
            board.init(LEVELS[levelIndex], this);
        }

        
    }

    public void pitSwitch()
    {
        if(clock % 10 > 5)
        {
            foreach(Pit pit in pits)
            {
                pit.turnOff();
            }
        }
        else
        {
            foreach (Pit pit in pits)
            {
                pit.turnOn();
            }
        }
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
        e1.name = "enemy-" + (enemies.Count + 1);

        e1.init(this, type);

        foreach(Enemy e2 in enemies)
        {
            Physics2D.IgnoreCollision(e1.model.GetComponent<Collider2D>(), e2.model.GetComponent<Collider2D>());
        }

        enemies.Add(e1);
    }

    private void addWall(float x, float y)
    {
        GameObject wallObject = new GameObject();           
        Wall w = wallObject.AddComponent<Wall>();           

        w.transform.position = new Vector3(x, y, 0);      								
        w.name = "Wall" + (walls.Count + 1);

        w.init(this);

        walls.Add(w);
    }


    private void addCrumbleWall(float x, float y)
    {
        GameObject wallObject = new GameObject();           
        CrumbleWall w = wallObject.AddComponent<CrumbleWall>();            

        w.transform.position = new Vector3(x, y, 0);     								
        //w.name = "CrumbleWall" + (walls.Count + 1);

        w.init(this);

        //walls.Add(w);
    }


    private Pit addPit(float x, float y)
    {
        GameObject pitObject = new GameObject();            // Create a new empty game object that will hold a gem.
        Pit pit = pitObject.AddComponent<Pit>();            // Add the Gem.cs script to the object.

        pit.transform.position = new Vector3(x, y, 0);      // Position the gem at x,y.								
       // pit.name = "Pit" + (walls.Count + 1);

        pit.init(this);

        pits.Add(pit);

        return pit;
        
    }

	private void addSpikes (float x, float y)
	{
		GameObject spikesObject = new GameObject ();
		Spikes spikes = spikesObject.AddComponent<Spikes> ();
		spikes.transform.position = new Vector3 (x, y, 0);
		spikes.init (this);
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


    void addTurret(float x, float y, int dir, bool isRotating)
    {
        GameObject obj = new GameObject();            // Create a new empty game object that will hold a gem.

        Turret turret = obj.AddComponent<Turret>();
        turret.transform.position = new Vector3(x, y, 0);      // Position the gem at x,y.								
        turret.name = "turret";
        turret.init(dir, isRotating, this);
    }

    void addObstacle(float x, float y, int type)
    {
        GameObject obj = new GameObject();
        Obstacle obs = obj.AddComponent<Obstacle>();
        obs.transform.position = new Vector3(x, y, 0);
        obs.init(this, type);
    }

    void addBurnWall(float x, float y)
    {
        GameObject obj = new GameObject();
        BurnWall obs = obj.AddComponent<BurnWall>();
        obs.transform.position = new Vector3(x, y, 0);
        obs.init(this);
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

        escapeCheck();

        //pitSwitch();
        //addEnemyPeriodically();
    }


    public void escapeCheck()
    {
        if (Input.GetButtonDown("Escape"))
        {
            menuShowing = !menuShowing;
            if (menuShowing)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
            }
        }
    }


    void OnGUI()
    {

        float escapeButtonWidth = 300;
        float escapeButtonHeight = 50;
        GUIStyle buttonStyle = new GUIStyle(GUI.skin.label);

        buttonStyle = GUI.skin.button;
        buttonStyle.normal.textColor = Color.white;
        buttonStyle.fontStyle = FontStyle.Bold;
        //buttonStyle.padding.top = -4;
        buttonStyle.padding.left = -10;
        buttonStyle.padding.right = -10;

        GUI.Label(new Rect(150, 10, 100, 30), "Fire Health: " + fire.health);
        GUI.Label(new Rect(270, 10, 100, 30), "Oil Health: " + oil.health);
        if (menuShowing)
        {
            float screenHeight = Screen.height;
            float screenWidth = Screen.width;

            GUI.Button(new Rect(screenWidth / 2 - escapeButtonWidth / 2, screenHeight / 2 - escapeButtonHeight / 2 - 50, escapeButtonWidth, escapeButtonHeight), "Resume", buttonStyle);
            GUI.Button(new Rect(screenWidth / 2 - escapeButtonWidth / 2, screenHeight / 2 - escapeButtonHeight / 2, escapeButtonWidth, escapeButtonHeight), "Restart", buttonStyle);
            GUI.Button(new Rect(screenWidth / 2 - escapeButtonWidth / 2, screenHeight / 2 - escapeButtonHeight / 2 + 50, escapeButtonWidth, escapeButtonHeight), "Main Menu", buttonStyle);
        }
    }

    //TODO: destroying projectiles probably
    public void nextLevel() {
        board.annihilate();
        levelIndex++;
        if (levelIndex >= LEVELS.Length) winScreen();
        else {
            Destroy(boardGO);
            boardGO = new GameObject();
            board = boardGO.AddComponent<Board>();
            board.init(LEVELS[levelIndex], this);
        }
    }

    public void levelSelect(int lvlNum) {
        board.annihilate();
        levelIndex = lvlNum;
        if (levelIndex >= LEVELS.Length || levelIndex < 0) print("what is even happening");
        else
        {
            Destroy(boardGO);
            board = boardGO.AddComponent<Board>();
            board.init(LEVELS[levelIndex], this);
        }
    }

    public void winScreen()
    {
        print("you win!!! (placeholder)");
    }
}
