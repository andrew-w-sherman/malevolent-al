using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {

    public int dirx = 0;
    public int diry = -1;
    public CharacterModel model;

    // for type, true is fire and false is oil
    // inheritance is 4 nerds
    public void init(bool type) {
        // make the model!! (pass in type, or just the sprite)
        if(type) {
            //fire stuff
        }
        else {
            //oil stuff
        }
    }

    public void move(Vector3 moveBase) {
        dirx = (int)moveBase.x;
        diry = (int)moveBase.y;
        Vector3 move = moveBase * Param.BASE_SPEED * Time.deltaTime;
        transform.localPosition += move;
    }

    public void fire() {
        model.fireSprite(new Vector3(dirx,diry,0));
    }

}
