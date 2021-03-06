﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {


    public bool DEBUG_LVL = false;


    public bool DEBUG_BOSS = false;
   
	readonly string[] LEVELS = { "test2", "dan tutorial2","dan tutorial3", "dan tutorial4", "dan tutorial1", "dan level 1","mike2","joel1", "dan level", "test3", "bossLevel" };
    int levelIndex;

    public GameObject boardGO;
    public Board board;
	public MusicController musicController;
    public TitleScreen ts;
    public WinScreen ws;
    public TutorialScreen tuts;

    public bool startMenu;
    public bool levelMenu;
    public bool tutorial;
    public bool escapeMenu;
    public bool beatGame;
    public bool newGame;

    public FireBall fire;
    public OilBall oil;
    public Explosion expl;
    public List<Enemy> enemies;
    public List<Turret> turrets;
    public List<Wall> walls;
    public List<Pit> pits;
    public List<Projectile> projectiles;
    public List<Tile> tiles;
    public int projectileCount;
    public float clock;
    public int addEnemyInterval = 5;
    public int whenAddEnemy;
    public int whichAddEnemy;
    public Camera cam;
    public float minCamSize;
	public Boss boss;
	public BossHelmet helmet;

	bool aboutToWin;
	float timeUntilWin;

    public const int NaN = 10 ^ 30;
    public static Vector3 NULL = new Vector3(NaN, NaN, NaN);

    private int inGoal = 0;

    void Start () {

		startMenu = false;
        levelMenu = false;
        tutorial = false;
        escapeMenu = false;
        beatGame = false;
        newGame = false;
        addTitleScreen();
        addWinScreen();
        addTutorialScreen();

		GameObject musicControllerObject = new GameObject ();
		musicControllerObject.name = "music controller";
		musicController = musicControllerObject.AddComponent<MusicController>();
		musicController.init (this);

        projectiles = new List<Projectile>();
        projectileCount = 0;
        tiles = new List<Tile>();
        clock = 0f;
        whenAddEnemy = 0;
        whichAddEnemy = 0;

        cam = Camera.main;
        minCamSize = cam.orthographicSize;

		//DEBUG_BOSS = true;

		if (DEBUG_BOSS) {
            loadPrototype();
            boss = addBoss (0, 0);
		}

        else if (DEBUG_LVL)
        {
            loadPrototype();
            
        }
        else
        {
            startMenu = true;
        }

		aboutToWin = false;
		timeUntilWin = 0f;
    }

    public void addTitleScreen()
    {
        GameObject tsObject = new GameObject();
        ts = tsObject.AddComponent<TitleScreen>();
        ts.transform.position = new Vector3(0, 0, 0);
        ts.init();
        ts.rend.enabled = false;
    }

    public void addWinScreen()
    {
        GameObject wsObject = new GameObject();
        ws = wsObject.AddComponent<WinScreen>();
        ws.transform.position = new Vector3(0, 0, 0);
        ws.init();
        ws.rend.enabled = false;
    }

    public void addTutorialScreen()
    {
        GameObject tutsObject = new GameObject();
        tuts = tutsObject.AddComponent<TutorialScreen>();
        tuts.transform.position = new Vector3(0, 0, 0);
        tuts.init();
        tuts.rend.enabled = false;
    }

    public void loadPrototype()
    {
        fire = addFire(0, 3);
        oil = addOil(0, 1);
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

        addEnemy(-4, 1, "fire", true);
        addEnemy(-4, 3, "oil", true);
        //Pit pit = addPit(-2, 1);
        //pit.turnOff();
        //pit.turnOn();
        addCrumbleWall(1, -3);
        addCrumbleWall(2, -3);
        addCrumbleWall(3, -3);
        addCrumbleWall(4, -3);
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

    //private void addEnemyPeriodically()
    //{
    //    if (clock > whenAddEnemy)
    //    {
    //        if (whichAddEnemy == 0)
    //        {
    //            addEnemy(10, 3, "fire");
    //            whenAddEnemy += addEnemyInterval;
    //            whichAddEnemy = 1;
    //        }
    //        else if (whichAddEnemy == 1)
    //        {
    //            addEnemy(10, 3, "oil");
    //            whenAddEnemy += addEnemyInterval;
    //            whichAddEnemy = 0;
    //        }
    //    }
    //}

    public void addTile(float x, float y)
    {
        GameObject tileObject = new GameObject();            // Create a new empty game object that will hold a gem.

        Tile t = tileObject.AddComponent<Tile>();
        t.transform.position = new Vector3(x, y, 0);      // Position the gem at x,y.								

        t.init(this);
        tiles.Add(t);
    }

    public FireBall addFire(float x, float y)
    {
        GameObject playerObject = new GameObject();            // Create a new empty game object that will hold a gem.

        fire = playerObject.AddComponent<FireBall>();
        fire.transform.position = new Vector3(x, y, 0);      // Position the gem at x,y.								
        fire.name = "Fire Ball";

        fire.init(this);
        return fire;
    }

    public OilBall addOil(float x, float y)
    {
        GameObject playerObject = new GameObject();            

        oil = playerObject.AddComponent<OilBall>();
        oil.transform.position = new Vector3(x, y, 0);     							
        oil.name = "Oil Ball";

        oil.init(this);
        return oil;
    }

    private void addEnemy(float x, float y, string type, bool moves)
    {
        GameObject enemyObject = new GameObject();            
        Enemy e1 = enemyObject.AddComponent<Enemy>();           

        e1.transform.position = new Vector3(x, y, 0);      								
        e1.name = "enemy-" + (enemies.Count + 1);

        e1.init(this, type, moves);

        foreach(Enemy e2 in enemies)
        {
            Physics2D.IgnoreCollision(e1.model.GetComponent<Collider2D>(), e2.model.GetComponent<Collider2D>());
        }

        enemies.Add(e1);
    }

	public Boss addBoss(float x, float y){
		GameObject bossObject = new GameObject();            
		bossObject.tag = "Boss";
		boss = bossObject.AddComponent<Boss>();         
		boss.transform.position = new Vector3(x, y, 0);    								
		boss.name = "Boss";
		boss.init(this);
		this.boss = boss;
		/*
		GameObject bossHelmetObject = new GameObject();
		helmet = bossHelmetObject.AddComponent<BossHelmet> ();
		helmet.init (boss);
*/
		return boss;
	}

	public void createExplosion(float x, float y){
		GameObject explModel = new GameObject();
		Explosion explosion = explModel.AddComponent<Explosion>();
		explosion.transform.position = new Vector3 (x, y, 0);
		explosion.init(null, 3f);
		aboutToWin = true;
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

    public void addProjectile(Vector3 start, Vector3 velocity, int type, Collider2D shooter)
    {
        GameObject projectileObject = new GameObject();            
        Projectile p = projectileObject.AddComponent<Projectile>();            

        p.transform.position = start;      							
        p.name = "Projectile " + (projectileCount + 1);
        projectileCount++;
        projectiles.Add(p);

        p.init(start, velocity, type, this, shooter);

        //foreach (Projectile p2 in projectiles)
        //{
        //    if(p2.coll != null)
        //    Physics2D.IgnoreCollision(p.coll, p2.coll);
        //}

        //if (type == Projectile.ENEMY)
        //{
        //    foreach(Enemy e in enemies)
        //    {
        //        if (e.model.coll != null)
        //            Physics2D.IgnoreCollision(p.coll, e.model.coll);
        //    }
        //    foreach (Turret t in turrets)
        //    {
        //        if (t.coll != null)
        //            Physics2D.IgnoreCollision(p.coll, t.coll);
        //    }
        //}
        //else
        //{
        //    Physics2D.IgnoreCollision(p.coll, fire.coll);
        //    Physics2D.IgnoreCollision(p.coll, oil.coll);
        //}
       

    }


    void addTurret(float x, float y, int dir, bool isRotating)
    {
        GameObject obj = new GameObject();            // Create a new empty game object that will hold a gem.

        Turret turret = obj.AddComponent<Turret>();
        turret.transform.position = new Vector3(x, y, 0);      // Position the gem at x,y.								
        turret.name = "turret";
        turret.init(dir, isRotating, this);

        turrets.Add(turret);
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
        if (fire == null || oil == null) return;
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

		if (aboutToWin) {
			timeUntilWin += Time.deltaTime;
			if (timeUntilWin >= 3f) {
				beatGame = true;
			}
		}
    }


    public void escapeCheck()
    {
        if (Input.GetButtonDown("Escape") && startMenu == false)
        {
            escapeMenu = !escapeMenu;
            if (escapeMenu)
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
        float screenHeight = Screen.height;
        float screenWidth = Screen.width;
        float escapeButtonWidth = 300;
        float escapeButtonHeight = 50;
        int startButtonWidth = 250;
        int startButtonHeight = 30;
        int winButtonWidth = 200;
        int winButtonHeight = 30;
        GUIStyle buttonStyle = new GUIStyle(GUI.skin.label);

        buttonStyle = GUI.skin.button;
        buttonStyle.normal.textColor = Color.white;
        buttonStyle.fontStyle = FontStyle.Bold;
        //buttonStyle.padding.top = -4;
        buttonStyle.padding.left = -10;
        buttonStyle.padding.right = -10;


        if (startMenu)
        {
            setTitleScreen();

            if (!levelMenu)
            {
                if (GUI.Button(new Rect(screenWidth / 2 - startButtonWidth / 2, screenHeight / 2 + screenHeight / 8 - escapeButtonHeight / 2, startButtonWidth, startButtonHeight), "New Game", buttonStyle))
                {
                    startMenu = false;
                    tutorial = true;
                    newGame = true;
                    ts.rend.enabled = false;
                }
                if (GUI.Button(new Rect(screenWidth / 2 - startButtonWidth / 2, screenHeight / 2 + screenHeight / 8 - startButtonHeight / 2 + startButtonHeight, startButtonWidth, startButtonHeight), "Load Level", buttonStyle))
                {
                    levelMenu = true;
                }
            }
            else
            {
                int buttonHeight = 0;
                int numButtons = LEVELS.Length + 1;
                int i = 0;

                if (numButtons % 2 == 0 && numButtons > 0)
                {
                    buttonHeight = -startButtonHeight / 2;
                    while (i < numButtons / 2 - 1)
                    {
                        buttonHeight -= startButtonHeight;
                        i++;
                    }

                }
                else if (numButtons % 2 == 1 && numButtons > 0)
                {
                    buttonHeight = 0;
                    while (i < numButtons / 2)
                    {
                        buttonHeight -= startButtonHeight;
                        i++;
                    }
                }

                i = 0;
                while (i < numButtons - 1)
                {
                    if (GUI.Button(new Rect(screenWidth / 2 - startButtonWidth / 2, screenHeight / 2 - startButtonHeight / 2 + buttonHeight, startButtonWidth, startButtonHeight), "Level " + (i + 1), buttonStyle))
                    {
                        levelIndex = i;
                        loadLevel();
                    }

                    buttonHeight += startButtonHeight;
                    i++;
                }

                if (GUI.Button(new Rect(screenWidth / 2 - startButtonWidth / 2, screenHeight / 2 - startButtonHeight / 2 + buttonHeight, startButtonWidth, startButtonHeight), "Return to Main Menu", buttonStyle))
                {
                    levelMenu = false;
                }

            }
        }
        else if (tutorial)
        {
            setTutorialScreen();

            if (newGame)
            {
                if (GUI.Button(new Rect(screenWidth / 2 - startButtonWidth / 2, screenHeight / 8 - startButtonHeight / 2, startButtonWidth, startButtonHeight), "Continue", buttonStyle))
                {
                    tuts.rend.enabled = false;
                    tutorial = false;
                    newGame = false;
                    loadNewGame();
                }
            }
            else
            {
                if (GUI.Button(new Rect(screenWidth / 2 - startButtonWidth / 2, screenHeight / 8 - startButtonHeight / 2, startButtonWidth, startButtonHeight), "Continue", buttonStyle))
                {
                    tuts.rend.enabled = false;
                    tutorial = false;
                }
            }
        }
        else if (beatGame == true)
        {
            setWinScreen();

            destroyEverything();
            if (GUI.Button(new Rect(screenWidth / 2 - winButtonWidth / 2, screenHeight / 2 + screenHeight/4 + screenHeight / 8 - winButtonHeight / 2, winButtonWidth, winButtonHeight), "Return to Main Menu", buttonStyle))
            {
                startMenu = true;
                beatGame = false;
            }
        }
        else
        {
            ts.rend.enabled = false;
            ws.rend.enabled = false;
            tuts.rend.enabled = false;

            if (fire != null) GUI.Label(new Rect(150, 10, 100, 30), "Fire Health: " + fire.health);
            if (oil != null) GUI.Label(new Rect(270, 10, 100, 30), "Oil Health: " + oil.health);
			if (boss != null)
				GUI.Label (new Rect (390, 10, 100, 30), "Boss health: " + boss.health);
            if (escapeMenu)
            {
                if (GUI.Button(new Rect(screenWidth / 2 - escapeButtonWidth / 2, screenHeight / 2 - escapeButtonHeight / 2 - 75, escapeButtonWidth, escapeButtonHeight), "Resume", buttonStyle))
                {
                    escapeMenu = false;
                    Time.timeScale = 1f;
                }
                if (GUI.Button(new Rect(screenWidth / 2 - escapeButtonWidth / 2, screenHeight / 2 - escapeButtonHeight / 2 - 25, escapeButtonWidth, escapeButtonHeight), "Controls", buttonStyle))
                {
                    tutorial = true;
                }
                if (GUI.Button(new Rect(screenWidth / 2 - escapeButtonWidth / 2, screenHeight / 2 - escapeButtonHeight / 2 + 25, escapeButtonWidth, escapeButtonHeight), "Restart Level", buttonStyle))
                {
                    changeBoard();
                }
                if (GUI.Button(new Rect(screenWidth / 2 - escapeButtonWidth / 2, screenHeight / 2 - escapeButtonHeight / 2 + 75, escapeButtonWidth, escapeButtonHeight), "Main Menu", buttonStyle))
                {
                    destroyEverything();
                    escapeMenu = false;
                    startMenu = true;
                }

            }
            else
            {
                if (Time.timeScale != 1f)
                {
                    Time.timeScale = 1f;
                }
            }
        }
    }

    public void loadNewGame()
    {
        levelIndex = 0;
        loadLevel();
    }

    public void setTitleScreen()
    {
        ts.rend.enabled = true;
        cam.transform.position = new Vector3(0, 0, -10);
        cam.transform.localScale = new Vector3(1, 1, 1);
        cam.orthographicSize = 5;
    }

    public void setWinScreen()
    {
        ws.rend.enabled = true;
        cam.transform.position = new Vector3(0, 0, -10);
        cam.transform.localScale = new Vector3(1, 1, 1);
        cam.orthographicSize = 5;
    }

    public void setTutorialScreen()
    {
        tuts.rend.enabled = true;
        tuts.transform.position = cam.transform.position - new Vector3(0, 0, -10);
        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;
        tuts.model.transform.localScale = new Vector3(width, height, 1);
        //cam.transform.position = new Vector3(0, 0, -10);
        //cam.transform.localScale = new Vector3(1, 1, 1);
        //cam.orthographicSize = 5;
    }

    private void loadLevel()
    {
        startMenu = false;
        levelMenu = false;
        boardGO = new GameObject();
        board = boardGO.AddComponent<Board>();
        board.init(LEVELS[levelIndex], 100, this);
		if (levelIndex < (LEVELS.Length - 2) / 3) {
			musicController.changeMusic (MusicController.EASY_MUSIC);
		} else if (levelIndex < (LEVELS.Length - 2) * 2 / 3) {
			musicController.changeMusic (MusicController.MID_MUSIC);
		} else if (levelIndex < (LEVELS.Length - 2)) {
			musicController.changeMusic (MusicController.HARD_MUSIC);
		} else {
			//levelIndex == Levels.Length - 1 , i.e. we're at the last level (the boss level)
			musicController.changeMusic (MusicController.BOSS_MUSIC);
		}
    }

    private void destroyEverything()
    {
        board.annihilate();
        if (fire != null)
        {
            Destroy(fire.gameObject);
            Destroy(fire);
        }
        if (oil != null)
        {
            Destroy(oil.gameObject);
            Destroy(oil);
        }
        fire = null; oil = null;
        Destroy(boardGO);
        inGoal = 0;
    }

    private void changeBoard()
    {
        destroyEverything();
        loadLevel();
    }

    public void goal(int type)
    {
        if ((inGoal == 2 && type == 1) || (inGoal == 1 && type == 2))
        {
            inGoal = 0; nextLevel();
        }
        else if (inGoal == 0)
        {
            inGoal = type;
        }
    }

    //TODO: destroying projectiles probably
    public void nextLevel() {
        levelIndex++;
        if (levelIndex >= LEVELS.Length) beatGame = true;
        else {
            changeBoard();
        }
    }

    public void levelSelect(int lvlNum) {
        levelIndex = lvlNum;
        if (levelIndex >= LEVELS.Length || levelIndex < 0) print("what is even happening");
        else
        {
            changeBoard();
        }
    }

    public void winScreen()
    {
        print("you win!!! (placeholder)");
    }
}
