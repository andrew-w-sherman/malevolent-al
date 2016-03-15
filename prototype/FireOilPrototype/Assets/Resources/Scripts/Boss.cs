using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour {
	
	public float clock;
	public BossModel model;
	public int state;
	public static int DOWN = 0;        // during phase 2, the boss' helmet has been knocked off and it can be attacked
	public static int ATTACK_SHOOT = 1;   // during phase 2, the boss' normal attacking phase.
	public static int ATTACK_CHARGE = 2;
	public static int ATTACK_CHARGE_END = 3;
	public static int PAUSE_BEGINNING = 4; // 3 second pause at the beginning of the level
	public static int PAUSE_BEFORE_CHARGE = 5; // 1.5 second pause before charge
	public static Vector3 center = new Vector3 (0, 0, 0);
	CircleCollider2D collider;
	GameController controller;

	public float shootTimer = 5; //time in between shots in stage 2
	public float rotateSpeed = 0.8f; // variable to affect how quickly boss rotates
	public int health;
	public int maxHealth = 80;
	public float timeInCurrentState;
	public float timeToStayDownFor = 7f;
	public Character target; //wether or not the boss is targeting oil (if false, the boss will target fire)
	public bool alreadySwitched;
	public bool alreadyShot;
	public Vector3 chargeDirection;
	public float chargeSpeed = 7f;
	public float chargeEndSpeed = 2f;

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

		collider = gameObject.AddComponent<CircleCollider2D>();
		collider.radius = 0.3f;
		collider.isTrigger = false;

		var bossModelObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
		BossModel bossModel = bossModelObject.AddComponent<BossModel> ();
		bossModel.init (this);

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
		
		if (state == ATTACK_SHOOT || state == PAUSE_BEFORE_CHARGE) {
			if (coll.gameObject.tag != "OilBall" &&
			    coll.gameObject.tag != "FireBall" &&
			    coll.gameObject.tag != "projectile-enemy") {

					Vector3 otherPosition = coll.gameObject.transform.position;
					Vector3 helmetMotionDirection = (transform.position - otherPosition).normalized;
			
					if (coll.gameObject.tag == "OilBall_Speeding") {
						//animate helmet getting knocked off in helmetMotionDirection
						print ("my helmet has been knocked off and i am temporarily down for the count!");
						state = DOWN;
						timeInCurrentState = 0f;
					} else {
						//animate helmet wiggling in accordance with helmetMotionDirection
					}
				}
			}

		if (state == DOWN) {
			if (coll.gameObject.tag != "OilBall" &&
			    coll.gameObject.tag != "FireBall" &&
			    coll.gameObject.tag != "projectile-enemy") {
					health--;
					print ("health is " + health);
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
			}
		}
	}

	// Update is called once per frame
	void Update () {
		clock += Time.deltaTime;

		if (state == DOWN) {
			timeInCurrentState += Time.deltaTime;
			if (timeInCurrentState > timeToStayDownFor) {
				state = ATTACK_SHOOT;
				timeInCurrentState = 0f;
				alreadySwitched = false;
				print ("now my helmet is back on and I am raring to go!");
			}
		} else if (state == ATTACK_SHOOT) {
			
			timeInCurrentState += Time.deltaTime;
			//shoot at current target
			if ((int)timeInCurrentState % 2 == 0) {
				if ((int)(timeInCurrentState * 10) % 2 == 0) {
					if (!alreadyShot || health < maxHealth / 3) {
						Vector3 shootDir = (target.transform.position - transform.position).normalized;
						Vector3 projectileStart = shootDir * collider.radius * 1.6f;
						controller.addProjectile (transform.position + projectileStart, shootDir, Projectile.ENEMY);
						alreadyShot = true;
						if (health < maxHealth / 3) {
							switchTargets ();
							Vector3 shootDir2 = (target.transform.position - transform.position).normalized;
							Vector3 projectileStart2 = shootDir2 * collider.radius * 1.6f;

							//controller.addProjectile (transform.position + projectileStart2, shootDir2a, Projectile.ENEMY);
						}
					}
				} else {
					alreadyShot = false;
				}
				alreadySwitched = false;
			} else if (!alreadySwitched) {
				switchTargets ();
				alreadySwitched = true;
			}

			//switch targets
			/*
			if (((int)timeInCurrentState + 1) % 10 == 0) {
				if (!alreadySwitched) {
					switchTargets ();
					alreadySwitched = true;
				}
			} else {
				alreadySwitched = false;
			}
			*/

			if (timeInCurrentState > 20 || (timeInCurrentState > 10 && health < maxHealth / 3)) {
				//prep for charge attack
				if (Random.Range (0, 2) > 1) {
					switchTargets ();
				}
				chargeDirection = (target.transform.position - transform.position).normalized;
				state = PAUSE_BEFORE_CHARGE;
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