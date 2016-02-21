using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

    public int type;
    public bool passable;

    public void init(int type) {
        this.type = type;
        if (type == 0) passable = true;
        else passable = false;
        // TODO: make the model! (no script, just quad)
    }
}
