using UnityEngine;
using System.Collections;

public class GameControl : MonoBehaviour {

    GameObject characterFolder;
    Character fire;
    Character oil;
    GameObject boardGO;
    Board board;

    void Start () {
        // TODO: set the origin point
        this.transform = new Vector3(br.Length/2f,br[0].Length/2f,0);
        // make board
        boardGO = new GameObject();
        boardGO.transform.parent = this.transform;
        board = boardGO.AddComponent<Board>();
    }

    void Update () {
        // TODO: 8dir movement is hard
        int dir1x = 0;
        int dir1y = 0;
        int dir2x = 0;
        int dir2y = 0;
        if (Input.KeyDown('w')) dir1x++;
        if (Input.KeyDown('s')) dir1x--;
        if (Input.KeyDown('d')) dir1y++;
        if (Input.KeyDown('a')) dir1y--;
        if (Input.KeyDown('i')) dir2x++;
        if (Input.KeyDown('k')) dir2x--;
        if (Input.KeyDown('l')) dir2y++;
        if (Input.KeyDown('j')) dir2y--;

        if (Input.KeyDown('c')) keepShooting();
        if (Input.KeyDown('n')) keepShooting2();

        if (Input.KeyPressed('x') && Param.CONTROL_MODE == Param.SWITCHING) /* switch */;

        // selected char moves according to dir 1
        if (Param.CONTROL_MODE == Param.DUAL) {
            // char2 moves according to dir2
        }
    }
}
