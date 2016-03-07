using UnityEngine;
using System.Collections;

public class FireModel : MonoBehaviour {

    private GameController demo;
    private FireBall owner;
    private Material mat;
    private Sprite[] charSp;
    private Sprite[] idle;
    private Sprite[] run;
    public bool isRunning;
    private SpriteRenderer sr;

    // Use this for initialization
    public void init(FireBall owner, GameController board)
    {
        charSp = Resources.LoadAll<Sprite>("Sprite Sheets/char-front");

        transform.parent = owner.transform;
        transform.localPosition = new Vector3(0, 0, 0);
        name = "fire model";

        DestroyImmediate(owner.GetComponent<MeshFilter>());
        DestroyImmediate(owner.GetComponent<MeshRenderer>());
        owner.gameObject.AddComponent<SpriteRenderer>();
        sr = owner.GetComponent<SpriteRenderer>();
        sr.sprite = charSp[0];
        sr.sortingOrder = 2;
        idle = new Sprite[] { charSp[0], charSp[1] };
        run = new Sprite[] { charSp[1], charSp[2], charSp[1], charSp[3] };
    }

    void LateUpdate()
    {
        if (!isRunning)
        {
            int index = (int)(Time.timeSinceLevelLoad * 2f);
            index = index % idle.Length;
            sr.sprite = idle[index];
        }
        else
        {
            int index = (int)(Time.timeSinceLevelLoad * 2f);
            index = index % run.Length;
            sr.sprite = run[index];
        }
    }
}
