using UnityEngine;
using System.Collections;

public class CharacterModel : MonoBehaviour {

    Material charMat;
    Material attkMat;
    GameObject charObj;
    GameObject attkObj;
    bool type;

    float attkTimer = 0f;
    bool attkTimerRunning = false;

    public void init(Character owner, bool type) {
        this.type = type;

        charObj = GameObject.CreatePrimitive(PrimitiveType.Quad);
        charObj.transform.parent = this.transform;
        charObj.transform.localPosition = new Vector3(0,0,0);
        charMat = charObj.GetComponent<Renderer>().material;
        charMat.shader = Shader.Find("Sprites/Default");
        if (type) charMat.mainTexture = Resources.Load<Texture2D>("fdsklfjkdsl");
        else charMat.mainTexture = Resources.Load<Texture2D>("dfjklsdfjkld");

        attkObj = GameObject.CreatePrimitive(PrimitiveType.Quad);
        attkObj.transform.parent = this.transform;
        attkObj.transform.localPosition = new Vector3(0,0,2);
        attkMat = attkObj.GetComponent<Renderer>().material;
        attkMat.shader = Shader.Find("Sprites/Default");
        if (type) attkMat.mainTexture = Resources.Load<Texture2D>("");
        else attkMat.mainTexture = Resources.Load<Texture2D>("");
    }

    void Update() {
        if (attkTimerRunning) {
            attkTimer += Time.deltaTime;
            if (attkTimer >= Param.ATTK_TIME) {
                attkObj.transform.localPosition = new Vector3(0,0,2);
                attkTimerRunning = false;
            }
        }
    }

    public void fireSprite(Vector3 relPos) {
        attkObj.transform.localPosition = relPos;
        attkTimerRunning = true;
        attkTimer = 0f;
    }

}
