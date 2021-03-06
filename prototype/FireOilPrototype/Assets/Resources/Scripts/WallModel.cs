﻿using UnityEngine;
using System.Collections;

public class WallModel : MonoBehaviour {

    public GameController demo;
    public Wall owner;
    private Material mat;
    public SpriteRenderer sr;

    public void init(Wall owner, GameController demo)
    {
        this.owner = owner;
        this.demo = demo;

        transform.parent = owner.transform;                 // Set the model's parent to the gem.
        transform.localPosition = new Vector3(0, 0, 0);     // Center the model on the parent. 
        name = "wall-model";
        
        owner.sr = gameObject.AddComponent<SpriteRenderer>();
        sr = owner.sr;
        sr.sortingOrder = 2;
        sr.sprite = Resources.LoadAll<Sprite>("Sprite Sheets/env-tile")[1];
        /*
        mat = GetComponent<Renderer>().material;
        mat.shader = Shader.Find("Transparent/Diffuse");
        mat.mainTexture = Resources.Load<Texture2D>("Textures/tileWall");
        mat.color = new Color(1, 1, 1);*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
