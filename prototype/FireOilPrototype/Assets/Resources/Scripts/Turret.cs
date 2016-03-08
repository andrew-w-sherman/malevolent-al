using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Turret : MonoBehaviour
{
    
   public  int curDir;
    bool rotate;
    GameController demo;
    public Dictionary<int, Vector3> vectorDic;
    float timer;
    public float speed;
    TurretModel model;
    bool fire;

    // Use this for initialization
    public void init(int orientation, bool rotate, GameController gc)
    {
        this.rotate = rotate;
        curDir = orientation;
        buildVectorMap();
        demo = gc;
        timer = 0;
        speed = 5;    //firing rate in seconds
        var modelObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
        model = modelObject.AddComponent<TurretModel>();
        modelObject.AddComponent<Rigidbody>();
        model.init(this, demo, rotate);
        
    }

    // Update is called once per frame
    void FixedUpdate()
    { 
               
        // shoot every 4 seconds or so
        if (!rotate)
        {
            if(timer > speed)
            {
                timer = Time.deltaTime;
                Vector3 vDir = Vector3.up;
                vectorDic.TryGetValue(curDir, out vDir);
                print(vDir);
                demo.addProjectile(transform.position, vDir, Projectile.ENEMY);
            }
        }
        else
        {
            //it will have hit one of the 8 directions
            if (fire)
                
            {                
                
                Vector3 vDir = Vector3.up;
                vectorDic.TryGetValue(curDir, out vDir);
                demo.addProjectile(transform.position, vDir, Projectile.ENEMY);
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