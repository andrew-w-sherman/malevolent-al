using UnityEngine;
using System.Collections;

public class OilModel : MonoBehaviour
{

    // Use this for initialization

    public GameController controller;
    bool isCharacter;
    OilBall b;
    OilPatch p;
    private Material mat;
    SpriteRenderer sr;
    Sprite[] charSp;
    Sprite[] idle;
    Sprite[] run;
    public bool isRunning = false;
    public bool flicker;
    public float clock;

    public void init(bool isCharacter, OilBall b, OilPatch p, GameController controller)
    {
        charSp = Resources.LoadAll<Sprite>("Sprite Sheets/char-front");
        this.isCharacter = isCharacter;
        this.controller = controller;
        this.b = b;
        this.p = p;

        if (isCharacter)
        {
            b.gameObject.AddComponent<SpriteRenderer>();
            sr = b.GetComponent<SpriteRenderer>();
            transform.parent = b.transform;
            transform.localPosition = new Vector3(0, 0, 0);
            name = "Oil Ball Model";
            sr.sortingOrder = 3;
            sr.sprite = charSp[4];
        }
        else
        {
            p.gameObject.AddComponent<SpriteRenderer>();
            sr = p.GetComponent<SpriteRenderer>();
            sr.sortingOrder = 2;
            transform.parent = p.transform;
            transform.localPosition = new Vector3(0, 0, 0);
            name = "Oil Patch Model";
            sr.sprite = charSp[10];
        }

        idle = new Sprite[] { charSp[4], charSp[5] };
        run = new Sprite[] { charSp[5], charSp[6], charSp[5], charSp[7] };
        flicker = false;
    }

    public void putOnFire()
    {
        if (!isCharacter)
        {
            sr.sortingOrder = 1;
            sr.sprite = charSp[11];
        }
    }

    public void setSpeeding(bool speeding)
    {
        if (isCharacter)
        {
            if (speeding) { sr.sprite = charSp[11]; }
            else { sr.sprite = charSp[4]; }
        }
    }

    public void Start()
    {
        clock = 0f;
    }

    void LateUpdate()
    {

        clock += Time.deltaTime;

        if (!isRunning && isCharacter)
        {
            int index = (int)(Time.timeSinceLevelLoad * 2f);
            index = index % idle.Length;
            sr.sprite = idle[index];
        }
        else if (isCharacter)
        {
            int index = (int)(Time.timeSinceLevelLoad * 2f);
            index = index % run.Length;
            sr.sprite = run[index];
        }
        if (isCharacter)
        {
            if (flicker)
            {
                if ((int)(clock * 10) % 2 == 0)
                {
                    sr.sprite = null;
                }
            }
        }
    }
}
