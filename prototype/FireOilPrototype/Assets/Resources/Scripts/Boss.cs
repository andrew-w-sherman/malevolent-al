﻿using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour {

	public float clock;
	public BossModel model;
	public BossHelmet helmet;
	public int state;
	public static int DOWN = 0;        // during phase 2, the boss' helmet has been knocked off and it can be attacked
	public static int ATTACK_SHOOT = 1;   // during phase 2, the boss' normal attacking phase.
	public static int ATTACK_CHARGE = 2;
	public static int ATTACK_CHARGE_END = 3;
	public static int PAUSE_BEGINNING = 4; // 3 second pause at the beginning of the level
	public static int PAUSE_BEFORE_CHARGE = 5; // 1.5 second pause before charge
	public static Vector3 center = new Vector3 (0, 0, 0);
	CircleCollider2D coll;
	GameController controller;

	public float shootTimer = 5; //time in between shots in stage 2
	public float rotateSpeed = 0.8f; // variable to affect how quickly boss rotates
	public int health;
	public int maxHealth = 100;
	public float timeInCurrentState;
	public float timeToStayDownFor = 9f;
	public Character target; //wether or not the boss is targeting oil (if false, the boss will target fire)
	public bool alreadySwitched;
	public bool alreadyShot;
	public Vector3 chargeDirection;
	public float chargeSpeed = 7f;
	public float chargeEndSpeed = 4f;

	public float direction; //direction boss is pointing at (in radians) 

	// Use this for initialization
	void Start () {
		clock = 0f;
		timeInCurrentState = 0f;
		direction = Mathf.PI; //boss starts pointing straight down
		state = PAUSE_BEGINNING;
		health = maxHealth;
		alreadySwitched = false;
		alreadyShot = false;

		var body = gameObject.AddComponent<Rigidbody2D>();
		body.gravityScale = 0;
		body.mass = 1000;
		body.isKinematic = false;

		coll = gameObject.AddComponent<CircleCollider2D>();
		coll.radius = 0.80f;
		coll.isTrigger = false;


		GameObject bossModelObject = new GameObject();
		bossModelObject.AddComponent<SpriteRenderer> ();
		model = bossModelObject.AddComponent<BossModel> ();
		model.init (this);


		var bossHelmetObject = new GameObject();
		bossHelmetObject.AddComponent<SpriteRenderer> ();
		helmet = bossHelmetObject.AddComponent<BossHelmet> ();
		helmet.init (this);

	}

	public void init(GameController controller){
		this.controller = controller;
		target = controller.oil;
	}

	public void switchTargets(){
		if (target == controller.oil) {
			target = controller.fire;
		} else {
			target = controller.oil;
		}
	}

	void OnCollisionEnter2D (Collision2D coll){
		
		Vector3 otherPosition = coll.gameObject.transform.position;
		Vector3 helmetMotionDirection = (transform.position - otherPosition).normalized;

		if (state == ATTACK_SHOOT || state == PAUSE_BEFORE_CHARGE) {
			if (coll.gameObject.tag != "OilBall" &&
				coll.gameObject.tag != "FireBall" &&
				coll.gameObject.tag != "projectile-enemy") {

				if (coll.gameObject.tag == "OilBall_Speeding") {
					helmet.setOff (helmetMotionDirection);
					state = DOWN;
					model.changeFace (BossModel.DAZED);
					timeInCurrentState = 0f;
				} else {
					helmet.setWiggle (helmetMotionDirection);
				}
			}
		}

		if (state == DOWN) {
			if (coll.gameObject.tag != "OilBall" &&
				coll.gameObject.tag != "FireBall" &&
				coll.gameObject.tag != "projectile-enemy") {
				health--;
				//print ("health is " + health);
			}
		}

		if (state == ATTACK_CHARGE) {
			if (coll.gameObject.tag == "OilBall" || coll.gameObject.tag == "OilBall_Speeding") {
				coll.gameObject.GetComponent<OilBall> ().damage (4);
			}
			if (coll.gameObject.tag == "FireBall") {
				coll.gameObject.GetComponent<FireBall> ().damage (4);
			}
			if (coll.gameObject.tag == "wall") {
				state = ATTACK_CHARGE_END;
				model.changeFace (BossModel.NORMAL);
			}
		}
		if (state == ATTACK_CHARGE_END && coll.gameObject.tag != "OilBall" &&
		    coll.gameObject.tag != "FireBall" &&
		    coll.gameObject.tag != "projectile-enemy") {
			helmet.setWiggle (helmetMotionDirection);
		}
	}

	// Update is called once per frame
	void Update () {
		clock += Time.deltaTime;
		if (health <= 0) {
			controller.createExplosion (transform.position.x, transform.position.y);
			Destroy (this.gameObject);
		}

		if (state == DOWN) {
			timeInCurrentState += Time.deltaTime;
			if (timeInCurrentState > timeToStayDownFor) {
				state = ATTACK_SHOOT;
				timeInCurrentState = 0f;
				alreadySwitched = false;
				//print ("now my helmet is back on and I am raring to go!");
				model.changeFace (BossModel.NORMAL);
			}
		} else if (state == ATTACK_SHOOT) {

			timeInCurrentState += Time.deltaTime;
			//shoot at current target
			if ((int)timeInCurrentState % 2 == 0) {
				model.changeFace (BossModel.ANGRY);
				if ((int)(timeInCurrentState * 10) % 2 == 0) {
					if (!alreadyShot || health < maxHealth / 3) {
						Vector3 shootDir = (target.transform.position - transform.position).normalized;
						Vector3 projectileStart = shootDir * coll.radius * 1.6f;
						controller.addProjectile (transform.position + projectileStart, shootDir, Projectile.ENEMY, coll);
						alreadyShot = true;
						if (health < maxHealth / 3) {
							switchTargets ();
							Vector3 shootDir2 = (target.transform.position - transform.position).normalized;

							Vector3 projectileStart2 = shootDir2 * coll.radius * 1.6f;

							controller.addProjectile (transform.position + projectileStart2, shootDir2, Projectile.ENEMY, coll);
						}
					}
				} else {
					alreadyShot = false;
				}
				alreadySwitched = false;
			} else if (!alreadySwitched) {
				switchTargets ();
				alreadySwitched = true;
			} else {
				model.changeFace (BossModel.NORMAL);
			}

			if (timeInCurrentState > 5 || (timeInCurrentState > 10 && health < maxHealth / 3)) {
				//prep for charge attack
				if (Random.Range (0, 2) > 1) {
					switchTargets ();
				}
				chargeDirection = (target.transform.position - transform.position).normalized;
				state = PAUSE_BEFORE_CHARGE;
				model.changeFace (BossModel.ANGRY);
				timeInCurrentState = 0f;
			}

		} else if (state == ATTACK_CHARGE) {

			transform.Translate (chargeDirection * Time.deltaTime * chargeSpeed);

		} else if (state == ATTACK_CHARGE_END) {

			transform.position = Vector3.MoveTowards (transform.position, center, Time.deltaTime * chargeEndSpeed);
			if (transform.position == center) {
				state = ATTACK_SHOOT;
				timeInCurrentState = 0f;
			}
		} else if (state == PAUSE_BEGINNING) {
			timeInCurrentState += Time.deltaTime;
			if (timeInCurrentState >= 3f) {
				state = ATTACK_SHOOT;
				timeInCurrentState = 0f;
			}
		} else if (state == PAUSE_BEFORE_CHARGE) {
			timeInCurrentState += Time.deltaTime;
			if (timeInCurrentState >= 1.5f) {
				state = ATTACK_CHARGE;
				timeInCurrentState = 0f;
			}
		}
	}
}