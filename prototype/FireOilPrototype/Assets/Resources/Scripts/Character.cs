using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {

    public Vector3 startPosition;
    public float clock = 0f;

    public bool speeding = false;
    
    public bool falling;
    public Collider2D fallingInto;
    public float initialDistance;
    public float whenFell = 0;
    float currentScale = 1f; // your scale factor

	//health stuff
	public float health;
	public float maxHealth = 10;
	public float healthRegenCooldown = 7; //how long health takes to regenerate after taking damage
	public float lastDamage;
	public float healthRegenRate = 1;     //hp gained per second while regenerating
	public float lastRegen;
	public float damageCooldown = 1.5f; //how long we wait in between taking damage (so things like spikes don't kill in a couple frames)

    void Start () {
        falling = false;
		lastDamage = -5;
		lastRegen = 0;
		health = maxHealth;
	}

    public void switchHit(Collider2D other)
    {
        if (other.gameObject.tag == "Switch")
        {
            other.gameObject.GetComponent<Switch>().switchTime();
        }
    }

    public void pitHit(Collider2D other)
    {
        //Debug.Log("hit1");
        if (other.gameObject.tag == "Pit")
        {
            //Debug.Log("hit2");
            if (other.gameObject.GetComponent<Pit>().on == true && speeding == false)
            {
                //Debug.Log("hit3");
                fallingInto = other;
                initialDistance = Vector2.Distance(transform.position, other.transform.position);
                whenFell = clock;
                falling = true;
            }
        }
        else if (other.gameObject.tag == "Goal")
        {
            if (!speeding)
            {
                this.hitGoal();
            }
        }
    }

    public void fallSequence()
    {
        if (clock < whenFell + 1)
        {
            if (currentScale - Time.deltaTime >= 0)
            {
                currentScale = currentScale - Time.deltaTime;
            }
            transform.localScale = new Vector3(currentScale, currentScale, currentScale);
            transform.position = Vector2.MoveTowards(transform.position, fallingInto.transform.position, initialDistance * Time.deltaTime);
        }
        else
        {
            if (tag != "enemy")
            {
                speeding = false;
                falling = false;
                currentScale = 1f;
                transform.localScale = new Vector3(currentScale, currentScale, currentScale);
                transform.position = startPosition;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }


	public void damage(int amount)
	{
		if (clock - lastDamage > damageCooldown) {
			lastDamage = clock;
			if (health - amount <= 0) {
				transform.position = startPosition;
				print ("You died :(");
				health = 10;
			} else {
				health -= amount;
			}
		}
	}

    // Update is called once per frame
    void Update () {
		
        clock = clock + Time.deltaTime;

		if (health < maxHealth &&
		    clock - lastDamage > healthRegenCooldown &&
		    clock - lastRegen > healthRegenRate) 
		{
			health++;
			lastRegen = clock;
		}
    }

    public virtual void hitGoal() { }
}
