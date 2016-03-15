using UnityEngine;
using System.Collections;

public class PitModel : MonoBehaviour {

    public GameController demo;
    public Pit owner;
    public Material mat;
    public Renderer rend;
    private Sprite[] tileSp;
    public SpriteRenderer sr;

    public void init(Pit owner, GameController demo)
    {

        this.owner = owner;
        this.demo = demo;

        tileSp = Resources.LoadAll<Sprite>("Sprite Sheets/env-tile");
        transform.parent = owner.transform;                 // Set the model's parent to the gem.
        transform.localPosition = new Vector3(0, 0, 0);     // Center the model on the parent. 
        name = "pit-model";

        owner.sr = owner.gameObject.AddComponent<SpriteRenderer>();
        sr = owner.GetComponent<SpriteRenderer>();
        sr.sortingOrder = 1;
        sr.sprite = tileSp[5];
    }

    // Update is called once per frame
    void Update()
    {

    }
}
