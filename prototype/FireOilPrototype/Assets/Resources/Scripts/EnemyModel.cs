﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyModel : MonoBehaviour
{

    private GameController controller;
    private Enemy owner;
    public string type;
    private Material mat;
    private Vector3 lastSeen;
    private Vector3 lastDirection;
    private float lostTimer;
    private int lost;
    private int speed = 2;
    private float noSightTimer = 0f;
    private bool noSight = false;

    public CircleCollider2D coll;

    public float height;
    public float width;

    SpriteRenderer sr;
    Sprite[] charSp;
    float clock;
    private Sprite[] idle;

    public float onOilSpeedChange;

    public void init(Enemy owner, GameController demo, string type)
    {
        charSp = Resources.LoadAll<Sprite>("Sprite Sheets/char-front");

        this.controller = demo;
        this.owner = owner;
        this.type = type;
        lastSeen = GameController.NULL;
        lastDirection = GameController.NULL;
        lost = 0;
        lostTimer = 0f;
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");

        var body = gameObject.AddComponent<Rigidbody2D>();
        body.gravityScale = 0;
        body.isKinematic = false;

        coll = gameObject.AddComponent<CircleCollider2D>();
        coll.radius = (float).33;
        coll.isTrigger = false;

        gameObject.tag = "enemy";
        transform.parent = owner.transform;
        transform.localPosition = new Vector3(0, 0, 0);
        name = "enemy-model";

        gameObject.AddComponent<SpriteRenderer>();
        sr = GetComponent<SpriteRenderer>();
        sr.sortingOrder = 2;

        if (type.Equals("fire"))
        {
            sr.sprite = charSp[8];
            idle = new Sprite[] { charSp[8], charSp[12] };
        }
        if (type.Equals("oil"))
        {
            sr.sprite = charSp[9];
            idle = new Sprite[] { charSp[9], charSp[13] };
        }
        clock = 0f;

        width = sr.bounds.size.x;
        height = sr.bounds.size.y;

        onOilSpeedChange = 1;
        
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
		if (coll.gameObject.tag == "projectile-fire" && type.Equals("oil"))
        {
            owner.health -= 10;
        }
		if (coll.gameObject.tag == "projectile-oil" && type.Equals("fire"))
		{
			owner.health -= 10;
		}
        if (coll.gameObject.tag == "OilBall_Speeding")
        {
            owner.health -= 10;
        }
        if (coll.gameObject.tag == "OilBall" || coll.gameObject.tag == "FireBall")
        {
            //Destroy(gameObject);

            if (coll.gameObject.tag == "OilBall")
            {
                coll.gameObject.GetComponent<OilBall>().damage(5);
            }
            else {
                coll.gameObject.GetComponent<FireBall>().damage(5);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
		if (coll.gameObject.tag == "Explosion" && type.Equals("fire"))
        {
            owner.health -= 10;
        }


        
    }

    void OnTriggerStay2D(Collider2D coll)
    {
        //if (coll.gameObject.tag == "plain tile")
        //{
        //    if (!owner.touchingTiles.Contains(coll.gameObject.GetComponent<Tile>()))
        //    {
        //        owner.touchingTiles.Add(coll.gameObject.GetComponent<Tile>());
        //    }
        //}

        if (!owner.falling)
        {
            owner.pitHit(coll);
        }

        if (coll.gameObject.tag == "OilPatch")
        {
            onOilSpeedChange = 0.5f;
        }
        if (coll.gameObject.tag == "OilPatch_OnFire" ||
            coll.gameObject.tag == "OilPatch_Spreading")
        {
            //owner.health--;
        }
    }

    public void cleanTouchingTiles()
    {
        foreach( Tile tile in owner.touchingTiles.ToArray() )
        {
            if (!coll.bounds.Intersects(tile.coll.bounds))
            {
                owner.touchingTiles.Remove(tile);
            }
        }
    }


    void Update()
    {
        if (owner.moves)
        {
            //cleanTouchingTiles();
            if (owner.falling)
            {
                owner.fallSequence();
            }
            else {
                Vector2 start = new Vector2(transform.position.x, transform.position.y);
                Vector3 direction3D = GameController.NULL;
                float calculatedSpeed = speed * Time.deltaTime;

                if (type.Equals("fire"))
                {
                    direction3D = controller.fire.model.transform.position - transform.position;
                }

                if (type.Equals("oil"))
                {
                    direction3D = controller.oil.model.transform.position - transform.position;
                }

                Vector2 direction = new Vector2(direction3D.x, direction3D.y).normalized;

                RaycastHit2D TopLeftHit = Physics2D.Raycast(start + new Vector2(-width / 4, height / 4), direction, direction3D.magnitude);
                RaycastHit2D TopRightHit = Physics2D.Raycast(start + new Vector2(width / 4, height / 4), direction, direction3D.magnitude);
                RaycastHit2D BotLeftHit = Physics2D.Raycast(start + new Vector2(-width / 4, -height / 4), direction, direction3D.magnitude);
                RaycastHit2D BotRightHit = Physics2D.Raycast(start + new Vector2(width / 4, -height / 4), direction, direction3D.magnitude);

                List<RaycastHit2D> hitList = new List<RaycastHit2D>();
                hitList.Add(TopLeftHit);
                hitList.Add(TopRightHit);
                hitList.Add(BotLeftHit);
                hitList.Add(BotRightHit);

                foreach (RaycastHit2D hit in hitList.ToArray())
                {
                    if (hit.collider == null)
                    {
                        hitList.Remove(hit);
                    }
                }

                if (hitList.Count > 0)
                {

                    int obstacleHit = 0;

                    //Debug.Log("new set");

                    foreach (RaycastHit2D hit in hitList.ToArray())
                    {
                        //Debug.Log(hit.collider);
                        if (hit.collider.gameObject.tag == "wall" || hit.collider.gameObject.tag == "Pit" ||
                            hit.collider.gameObject.tag == "Explosion")
                        {
                            obstacleHit = 1;
                        }
                    }

                    if (obstacleHit == 0)
                    {
                        lost = 0;
                        noSight = false;
                        lostTimer = 0f;
                        noSightTimer = 0f;
                        if (type.Equals("fire"))
                        {
                            lastSeen = controller.fire.model.transform.position;
                        }

                        if (type.Equals("oil"))
                        {
                            lastSeen = controller.oil.model.transform.position;
                        }

                        lastDirection = direction3D;
                        transform.Translate(lastDirection.normalized * calculatedSpeed);
                    }
                    else
                    {
                        if (lastDirection != GameController.NULL)
                        {
                            noSightTimer += Time.deltaTime;
                            if(noSightTimer > 5)
                            {
                                lost = 1;
                                lostTimer = 3;
                            }
                            if (lost != 1)
                            {
                                if (transform.position != lastSeen)
                                {
                                    transform.position = Vector2.MoveTowards(start, lastSeen, calculatedSpeed);
                                }
                                else
                                {
                                    lost = 1;
                                }
                            }
                            if (lost == 1 && lostTimer < 3)
                            {
                                lostTimer += Time.deltaTime;
                                transform.Translate(lastDirection.normalized * calculatedSpeed);
                            }
                        }
                    }
                }
                onOilSpeedChange = 1;
            }
        }
    }

    void LateUpdate()
    {
        clock += Time.deltaTime;
        int index = (int)(Time.timeSinceLevelLoad * 2f);
        index = index % idle.Length;
        sr.sprite = idle[index];
    }
}
