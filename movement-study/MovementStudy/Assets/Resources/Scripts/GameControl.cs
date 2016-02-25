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
        int[,] br = Param.BOARD;

        // set the origin point
        this.transform.position = new Vector3(-br.GetLength(0)/2f,-br.GetLength(1)/2f,0);
        
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
        
        // grab camera and give it a script
        cameraGO = GameObject.Find("Main Camera");
        mc = cameraGO.AddComponent<MainCamera>();
        mc.transform.parent = this.transform;
        mc.transform.localPosition = new Vector3(br.GetLength(0)/2f,br.GetLength(1)/2f,0);
        mc.init(fireGO,oilGO);
    }

    void Update () {
        // 8dir movement is hard
        int dir1x = 0;
        int dir1y = 0;
        int dir2x = 0;
        int dir2y = 0;
        if (Input.GetKey("w")) dir1x++;
        if (Input.GetKey("s")) dir1x--;
        if (Input.GetKey("d")) dir1y++;
        if (Input.GetKey("a")) dir1y--;
        if (Input.GetKey("i")) dir2x++;
        if (Input.GetKey("k")) dir2x--;
        if (Input.GetKey("l")) dir2y++;
        if (Input.GetKey("j")) dir2y--;

        if (Input.GetKeyDown("c")) currentChar.fire();
        if (Input.GetKeyDown("n") && Param.CONTROL_MODE == Param.DUAL) oil.fire();

        if (Input.GetKeyDown("x") && Param.CONTROL_MODE == Param.SWITCHING) switchChar();

        currentChar.move(new Vector3(dir1x,dir1y,0));
        if (Param.CONTROL_MODE == Param.DUAL) {
            // char2 moves according to dir2
            oil.move(new Vector3(dir2x,dir2y,0));
        }
    }

    void switchChar() {
        if (currentChar == fire) currentChar = oil;
        else currentChar = fire;
        mc.reposition();
    }
}
