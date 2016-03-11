using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;

public class Board : MonoBehaviour {

    char[,] characters;
    Tile[,] tiles;
    GameObject tileFolder;

    Switch[] switches = new Switch[10];
    List<Tile>[] switchingTiles = new List<Tile>[10];

	// Use this for initialization
	void Start () {
	
	}

    public void init(string filename, GameController gc, FireBall fb, OilBall ob)
    {
        for (int i = 0; i < 10; i++) switchingTiles[i] = new List<Tile>();
        List<char[]> arrlist = new List<char[]>();
        try {
            string line;
            StreamReader theReader = new StreamReader("Levels/" + filename, Encoding.Default);
            using (theReader)
            {
                do {
                    line = theReader.ReadLine();

                    if (line != null)
                    {
                        char[] entries = line.ToCharArray();
                        if (entries.Length > 0)
                            arrlist.Add(entries);
                    }
                }
                while (line != null);
                theReader.Close();
            }
        }
        catch (Exception e) {
            Console.WriteLine("Level failed to load!");
        }
        characters = To2D(arrlist.ToArray());

        if (characters.GetLength(1) % 2 == 1) print("something bad happened (board)");

        tileFolder.transform.position = new Vector3(-characters.GetLength(1) / 4f, -characters.GetLength(0) / 2f, 0f);

        // going along y
        for (int i = characters.GetLength(0) - 1; i >= 0; i--)
        {
            // two at a time along x
            for (int j = 0; j < characters.GetLength(1); j+=2)
            {
                char c1 = characters[i, j];
                char c2 = characters[i, j + 1];
                GameObject obj = new GameObject();
                Tile tile = null;
                switch (c1)
                {
                    case 'X':
                        if (c2 == 'X')
                        {
                            tile = obj.AddComponent<Wall>();
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
                            Tile sw = obj.AddComponent<SwitchingWall>();
                            switchingTiles[num].Add(sw);
                            tile = sw;
                        }
                        else print("whoops");
                        break;
                    case 'O':
                        if (c2 != 'O') print("syntax err");
                        tile = obj.AddComponent<Tile>();
                        break;
                    case 'B':
                        if (c2 != 'B') print("syntax err");
                        tile = obj.AddComponent<BurningWall>();
                        break;
                    case 'C':
                        if (c2 != 'C') print("syntax err");
                        tile = obj.AddComponent<CrumblingWall>();
                        break;
                    case '^':
                        if (c2 != '^') print("syntax err");
                        tile = obj.AddComponent<Spikes>();
                        break;
                    case 'P':
                        if (c2 == 'P') tile = obj.AddComponent<Pit>();
                        else if (Char.IsDigit(c2))
                        {
                            int num = (int)Char.GetNumericValue(c2);
                            Tile sw = obj.AddComponent<SwitchingPit>();
                            switchingTiles[num].Add(sw);
                            tile = sw;
                        }
                        else print("something is wrong");
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
                        tile = obj.AddComponent<Tile>();
                        GameObject enemyGO = new GameObject();
                        Enemy en = enemyGO.AddComponent<Enemy>();
                        if (c2 == 'f') en.init(gc, "fire");
                        else if (c2 == 'o') en.init(gc, "oil");
                        en.transform.position = this.transform.position + new Vector3(j, i, 0);
                        break;
                    case 's':
                        tile = obj.AddComponent<Tile>();
                        if (c2 == 'f') fb.transform.position = this.transform.position + new Vector3(j, i, 0);
                        else if (c2 == 'o') ob.transform.position = this.transform.position + new Vector3(j, i, 0);
                        else print("bad syntax");
                        break;
                    case 't':
                        tile = obj.AddComponent<Tile>();
                        GameObject turretGO = new GameObject();
                        Turret tr = turretGO.AddComponent<Turret>();
                        if (c2 == 'f') tr.init(gc, "fire");
                        else if (c2 == 'o') tr.init(gc, "oil");
                        en.transform.position = this.transform.position + new Vector3(j, i, 0);
                        break;
                    default:
                        print("uhhhhh");
                        break;
                }
                tile.transform.parent = tileFolder.transform;
                tile.transform.localPosition = new Vector3(j, i, 0);
                tiles[j, i] = tile;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
	
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
