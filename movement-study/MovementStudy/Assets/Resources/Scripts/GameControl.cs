using UnityEngine;
using System.Collections;

public class GameControl : MonoBehaviour {

    GameObject characterFolder;
    Character fire;
    Character oil;
    Character currentChar;
    GameObject boardGO;
    Board board;
    GameObject cameraGO;
    MainCamera mc;

    void Start () {
        // set the origin point
        this.transform.position = new Vector3(-br.Length/2f,-br[0].Length/2f,0);
        
        // grab camera and give it a script
        cameraGO = GameObject.Find("Main Camera");
        mc = cameraGO.AddComponent<MainCamera>();
        camera.transform.parent = this.transform;
        camera.transform.localPosition = new Vector3(br.Length/2f,br[0].Length/2f,0);
        camera.init();

        // make board
        boardGO = new GameObject();
        boardGO.transform.parent = this.transform;
        board = boardGO.AddComponent<Board>();

        // do character things
        GameObject fireGO = new GameObject();
        GameObject oilGO = new GameObject();
        fire = fireGO.AddComponent<Character>();
        oil = oilGO.AddComponent<Character>();
        fire.transform.parent = transform; oil.transform.parent = transform;
        fire.transform.localPosition = new Vector3(1,1,0);
        oil.transform.localPosition = new Vector3(2,2,0);
        fire.init(true); oil.init(false);
    }

    void Update () {
        // 8dir movement is hard
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

        if (Input.KeyDown('c')) currentChar.fire();
        if (Input.KeyDown('n') && Param.CONTROL_MODE == Param.DUAL) oil.fire();

        if (Input.KeyPressed('x') && Param.CONTROL_MODE == Param.SWITCHING) switch();

        currentChar.move(new Vector3(dir1x,dir1y,0));
        if (Param.CONTROL_MODE == Param.DUAL) {
            // char2 moves according to dir2
            oil.move(new Vector3(dir2x,dir2y,0));
        }
    }

    void switch() {
        if (currentChar == fire) currentChar = oil;
        else currentChar = fire;
        camera.center();
    }
}
