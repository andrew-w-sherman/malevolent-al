using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;

public class Board : MonoBehaviour {

    GameController gc;
    FireBall fb;
    OilBall ob;
    char[][] characters;
    Tile[,] tiles;
    GameObject tileFolder;
    List<GameObject> enemies = new List<GameObject>();
    List<GameObject> turrets = new List<GameObject>();

    Switch[] switches = new Switch[10];
    List<Tile>[] switchingTiles = new List<Tile>[10];

	// Use this for initialization
	void Start () {
	
	}

    public void init(string filename, GameController gc)
    {
        tileFolder = new GameObject();
        this.gc = gc;
        fb = gc.fire;
        ob = gc.oil;
        for (int i = 0; i < 10; i++) switchingTiles[i] = new List<Tile>();
        
        TextAsset ta = Resources.Load("Levels/" + filename) as TextAsset;
        string[] lines = ta.text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
        characters = new char[lines.Length][];
        for (int i = 0; i < lines.Length; i++) characters[i] = lines[i].ToCharArray();

        if (characters[0].Length % 2 == 1) print("something bad happened (board)");

        tileFolder.transform.position = new Vector3(-(characters[0].Length - 2) / 4f, -(characters.Length - 1) / 2f, 0f);
        tiles = new Tile[characters[0].Length/2,characters.Length];

        // going along y
        for (int i = characters.Length - 1; i >= 0; i--)
        {
            // two at a time along x
            for (int j = 0; j < characters[0].Length; j+=2)
            {
                char c1 = characters[i][j];
                char c2 = characters[i][j + 1];
                Vector3 pos = tileFolder.transform.position + new Vector3(j / 2, characters.Length - i - 1, 0);
                GameObject obj = new GameObject();
                Tile tile = null;
                switch (c1)
                {
                    case 'X':
                        if (c2 == 'X')
                        {
                            tile = obj.AddComponent<Wall>();
                            tile.init(gc);
                        }
                        else if (c2 == 'F')
                        {
                            Obstacle obs = obj.AddComponent<Obstacle>();
                            obs.init(gc, Obstacle.FIRE);
                            tile = obs;
                        }
                        else if (c2 == 'O')
                        {
                            Obstacle obs = obj.AddComponent<Obstacle>();
                            obs.init(gc, Obstacle.OIL);
                            tile = obs;
                        }
                        else if (Char.IsDigit(c2))
                        {
                            int num = (int)Char.GetNumericValue(c2);
                            Tile sw = obj.AddComponent<Wall>();
                            switchingTiles[num].Add(sw);
                            tile = sw;
                            tile.init(gc);
                        }
                        else print("whoops");
                        break;
                    case 'O':
                        if (c2 != 'O') print("syntax err");
                        tile = obj.AddComponent<Tile>();
                        tile.init(gc);
                        break;
                    case ' ':
                        if (c2 != ' ') print("syntax err");
                        tile = obj.AddComponent<Tile>();
                        tile.init(gc);
                        break;
                    case 'B':
                        if (c2 != 'B') print("syntax err");
                        tile = obj.AddComponent<BurnWall>();
                        tile.init(gc);
                        break;
                    case 'C':
                        if (c2 != 'C') print("syntax err");
                        // TODO: do we have a crumbling wall???
                        tile = obj.AddComponent<CrumbleWall>();
                        tile.init(gc);
                        break;
                    case '^':
                        if (c2 != '^') print("syntax err");
                        tile = obj.AddComponent<Spikes>(); tile.init(gc);
                        break;
                    case 'G':
                        if (c2 != 'G') print("syntax err");
                        tile = obj.AddComponent<Spikes>(); tile.init(gc);
                        break;
                    case 'P':
                        if (c2 == 'P') tile = obj.AddComponent<Pit>();
                        else if (Char.IsDigit(c2))
                        {
                            int num = (int)Char.GetNumericValue(c2);
                            Tile sw = obj.AddComponent<Pit>();
                            switchingTiles[num].Add(sw);
                            tile = sw;
                        }
                        else print("something is wrong");
                        tile.init(gc);
                        break;
                    case 'S':
                        if (!Char.IsDigit(c2)) print("whoa there");
                        else
                        {
                            int num = (int)Char.GetNumericValue(c2);
                            Switch sw = obj.AddComponent<Switch>();
                            switches[num] = sw;
                            tile = sw;
                        }
                        break;
                    case 'e':
                        tile = obj.AddComponent<Tile>(); tile.init(gc);
                        GameObject enemyGO = new GameObject();
                        Enemy en = enemyGO.AddComponent<Enemy>();
                        if (c2 == 'f') en.init(gc, "fire");
                        else if (c2 == 'o') en.init(gc, "oil");
                        en.transform.position = pos;
                        enemies.Add(enemyGO);
                        break;
                    case 's':
                        tile = obj.AddComponent<Tile>(); tile.init(gc);
                        
                        if (c2 == 'f')
                        {
                            fb.transform.position = pos;
                            fb.startPosition = pos;
                        }
                        else if (c2 == 'o')
                        {
                            ob.transform.position = pos;
                            ob.startPosition = pos;
                        }
                        else print("bad syntax");
                        break;
                    case 't':
                        tile = obj.AddComponent<Tile>(); tile.init(gc);
                        GameObject turretGO = new GameObject();
                        Turret tr = turretGO.AddComponent<Turret>();
                        if (c2 == '0' || c2 == '1' || c2 == '2' ||
                            c2 == '3' || c2 == '4' || c2 == '5' ||
                            c2 == '6' || c2 == '7') tr.init((int)Char.GetNumericValue(c2), false, gc);
                        else print("bad syntax");
                        tr.transform.position = pos;
                        turrets.Add(turretGO);
                        break;
                    default:
                        print("uhhhhh");
                        break;
                }
                tile.transform.parent = tileFolder.transform;
                tile.transform.localPosition = new Vector3(j/2, characters.Length - i - 1, 0);
                tiles[j/2, i] = tile;
            }
        }
        linkSwitches();
        linkTiles();
    }

    public void annihilate()
    {
        for (int i = 0; i < tiles.GetLength(0); i++)
        {
            for (int j = 0; j < tiles.GetLength(1); j++)
            {
                Tile tile = tiles[i, j];
                Destroy(tile.gameObject);
            }
        }
        foreach (GameObject en in enemies) Destroy(en);
        foreach (GameObject tr in turrets) Destroy(tr);
        ob.destroyTrail();
        fb.transform.position = Vector2.left;
        ob.transform.position = Vector2.right;
        Destroy(tileFolder);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    private void linkSwitches()
    {
        for (int i = 0; i < switches.Length; i++)
        {
            if (switches[i] == null) continue;
            switches[i].init(gc, switchingTiles[i]);
        }
    }

    private void linkTiles()
    {
        for (int i = 0; i < tiles.GetLength(0); i++) {
            for (int j = 0; j < tiles.GetLength(1); j++)
            {
                Tile tile = tiles[i, j];
                if (tile.linkTag == 0) continue;
                if (i + 1 < tiles.GetLength(0) && tile.linkTag == tiles[i + 1, j].linkTag) tile.neighbors[2] = tiles[i + 1, j];
                if (i - 1 >= 0 && tile.linkTag == tiles[i - 1, j].linkTag) tile.neighbors[0] = tiles[i - 1, j];
                if (j + 1 < tiles.GetLength(1) && tile.linkTag == tiles[i, j + 1].linkTag) tile.neighbors[1] = tiles[i, j + 1];
                if (j - 1 >= 0 && tile.linkTag == tiles[i, j - 1].linkTag) tile.neighbors[3] = tiles[i, j - 1];
                tile.link();
            }
        }
    }

    static T[,] To2D<T>(T[][] source)
    {
        try
        {
            int FirstDim = source.Length;
            int SecondDim = source.GroupBy(row => row.Length).Single().Key; // throws InvalidOperationException if source is not rectangular

            var result = new T[FirstDim, SecondDim];
            for (int i = 0; i < FirstDim; ++i)
                for (int j = 0; j < SecondDim; ++j)
                    result[i, j] = source[i][j];

            return result;
        }
        catch (InvalidOperationException)
        {
            throw new InvalidOperationException("The given jagged array is not rectangular.");
        }
    }
}
