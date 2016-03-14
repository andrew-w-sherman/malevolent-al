using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour {
	
	public float clock;
	public BossModel model;
	public int state;
	public static int UNDERGROUND = 0; // during phase 1, the boss is underground
	public static int EMERGING = 1;    // the boss emerges at the start of phase 2, and after its helmet gets knocked off and it sinks down
	public static int DOWN = 2;        // during phase 2, the boss' helmet has been knocked off and it can be attacked
	public static int SINKING = 3;     // during phase 2, after its been down for the appropriate time, the boss sinks into the ground before emerging again.
	public static int ATTACKING = 4;   // during phase 2, the boss' normal attacking phase.
	CircleCollider2D collider;
	GameController controller;

	//Stage 2 variables
	public float shootTimer = 5; //time in between shots in stage 2
	public float rotateSpeed = 0.8f; // variable to affect how quickly boss rotates
	public int health;
	public int maxHealth = 100;
	public float timeInCurrentState;
	public float timeToStayDownFor = 7f;
	public Character target; //wether or not the boss is targeting oil (if false, the boss will target fire)
	public bool alreadySwitched;
	public bool alreadyShot;

	public float direction; //direction boss is pointing at (in radians) 

	// Use this for initialization
	void Start () {
		clock = 0f;
		timeInCurrentState = 0f;
		direction = Mathf.PI; //boss starts pointing straight down
		state = ATTACKING;
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

	void OnCollisionEnter2D (Collision2D coll){
		
			if (coll.gameObject.tag != "OilBall" && 
				coll.gameObject.tag != "FireBall" &&
				coll.gameObject.tag != "projectile-enemy") {  

			if (state == ATTACKING) {
					Vector3 otherPosition = coll.gameObject.transform.position;
					Vector3 helmetMotionDirection = (transform.position - otherPosition).normalized;

					if (coll.gameObject.tag == "OilBall_Speeding") {
						//animate helmet getting knocked off in helmetMotionDirection
						print("my helmet has been knocked off and i am temporarily down for the count!");
						state = DOWN;
						timeInCurrentState = 0f;
					} else {
						//animate helmet wiggling in accordance with helmetMotionDirection
					}
				}

			if (state == DOWN) {
				health--;
				print ("health is " + health);
			}
		}
	}

	// Update is called once per frame
	void Update () {
		clock += Time.deltaTime;

		if (state == DOWN) {
			timeInCurrentState += Time.deltaTime;
			if (timeInCurrentState > timeToStayDownFor) {
				state = ATTACKING;
				timeInCurrentState = 0f;
				alreadySwitched = false;
				print ("now my helmet is back on and I am raring to go!");
			}
		} else if (state == ATTACKING) {
			timeInCurrentState += Time.deltaTime;

			//shoot at current target
			if ((int)timeInCurrentState % 2 == 0) {
				if ((int)(timeInCurrentState * 10) % 2 == 0) {
					if (!alreadyShot || health < maxHealth / 2) {
						Vector3 shootDir = (target.transform.position - transform.position).normalized;
						Vector3 projectileStart = shootDir * collider.radius * 1.6f;
						controller.addProjectile (transform.position + projectileStart, shootDir, Projectile.ENEMY);
						alreadyShot = true;
					}
				} else {
					alreadyShot = false;
				}
			}

			//switch targets
			if (((int)timeInCurrentState + 1 )% 10 == 0) {
				if (!alreadySwitched) {
					if (target == controller.oil) {
						target = controller.fire;
					} else {
						target = controller.oil;
					}
					alreadySwitched = true;
				}
			} else {
				alreadySwitched = false;
			}
		}
	}
}