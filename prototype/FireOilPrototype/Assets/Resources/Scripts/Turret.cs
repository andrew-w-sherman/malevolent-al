using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Turret : Character
{
    
    public int curDir;
    bool isRotating;
    public BoxCollider2D coll;
    GameController demo;
    public Dictionary<int, Vector3> vectorDic;
    float timer;
    TurretModel model;
    bool fire;

    // Use this for initialization
    public void init(int orientation, bool isRotating, GameController gc)
    {
        this.isRotating = isRotating;
        curDir = orientation;
        buildVectorMap();
        demo = gc;
        timer = 0;
        base.speed = .75f;    //firing rate in seconds


        coll = gameObject.AddComponent<BoxCollider2D>();
        coll.isTrigger = false;

        var modelObject = new GameObject();
        model = modelObject.AddComponent<TurretModel>();
        model.init(this, demo, isRotating);
        
    }


    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "projectile-fire" || 
            coll.gameObject.tag == "projectile-oil" ||
            coll.gameObject.tag == "OilBall_Speeding")
        {
            health -= 10;
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
        if (coll.gameObject.tag == "Explosion")
        {
            health -= 10;
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }

        // shoot every 4 seconds or so
        if (!isRotating)
        {
            if(timer > base.speed)
            {
                timer = Time.deltaTime;
                Vector3 vDir = Vector3.up;
                vectorDic.TryGetValue(curDir, out vDir);
                //print(curDir + " " + vDir);
                demo.addProjectile(transform.position, vDir.normalized, Projectile.ENEMY, coll);
            }
        }
        else
        {
            //it will have hit one of the 8 directions
            if (fire)
                
            {                
                
                Vector3 vDir = Vector3.up;
                vectorDic.TryGetValue(curDir, out vDir);
                demo.addProjectile(transform.position, vDir, Projectile.ENEMY, coll);
                fire = false;
            }
        }
        timer += Time.deltaTime;
     
    }

    void buildVectorMap()
    {
        vectorDic = new Dictionary<int, Vector3>();
        vectorDic.Add(0, Vector3.up);
        vectorDic.Add(1, new Vector3(1, 1, 0));
        vectorDic.Add(2, Vector3.right);
        vectorDic.Add(3, new Vector3(1, -1, 0));
        vectorDic.Add(4, Vector3.down);
        vectorDic.Add(5, new Vector3(-1, -1, 0));
        vectorDic.Add(6, Vector3.left);
        vectorDic.Add(7, new Vector3(-1, 1, 0));
    }

    public void shootProj(bool rotateForward)
    {
        //Updates curdir so that on next update a projectile will be fired in right direction
        fire = true;
        if (rotateForward)
        {            
            curDir = mod((curDir - 1),  8);
        }
        else
        {

            curDir = mod((curDir + 1), 8);
        }
    }
   public int mod(int x, int m)
    {
        return (x % m + m) % m;
    }
}