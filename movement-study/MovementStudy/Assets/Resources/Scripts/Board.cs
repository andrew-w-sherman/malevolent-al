using UnityEngine;
using System.Collections;

public class Board : MonoBehaviour {

    public Tile[,] board;

    // Use this for initialization
    void Start () {
        int [,] br = Param.BOARD;
        board = new Tile[br.Length, br[0].Length];
        transform.localPosition = new Vector3(0,0,0);
        for (int i = 0; i < br.Length; i++) {
            for (int j = 0; j < br[0].Length; i++) {
                if (i >= br[i].Length) {
                    // TODO: some error or default for this
                }
                board[i,j] = new Tile();
                // TODO: whatever other instantiation bullshit
                if (br[i,j] == 0) board[i,j].init(0);
                else if (j < 1 ORRR br[i,j-1] == 0) board[i,j].init(1);
                else board[i,j].init(2);
            }
        }
    }
    
    public bool isPassable(int i, int j) {
        // TODO: pipes r 4 scrubs
        if (i >= board.Length) {
            // TODO: we hecked
            print("Whoa there friendo");
        }
        if (j >= board[0].Length) {
            // TODO: we hecked
            print("Hold it there, pal");
        }
        return board[i,j].passable;
    }
}
