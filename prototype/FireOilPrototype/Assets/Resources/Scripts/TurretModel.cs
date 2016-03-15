using UnityEngine;
using System.Collections;

public class TurretModel : MonoBehaviour {

    //SO most of this is just a mess, I had trouble with implementing the rotation, I'll fix later 

    GameController demo;
    Turret owner;
    Material mat;
    bool rotate;
    float myZ; //will be out of 360 but will only really rotate around 180 deg
    
    float startR;
    bool forwards;
    float speed; //speed of the rotation in number of frames per full rotation
    

    //Making it rotatable will allow the turret to his a 180 deg radius to the right


    public void init(Turret owner, GameController demo, bool rotate)
    {
        this.owner = owner;
        this.demo = demo;
        
        this.rotate = rotate;
        
        float angle = 0;
        
        forwards = true;
        speed = 3;//??? idk what this should be
        
        startR = transform.rotation.eulerAngles.z;
        myZ = -45 * owner.curDir; //counted in counter-clockwise fashion

       
   
        transform.parent = owner.transform;                 // Set the model's parent to the gem.
        transform.localPosition = new Vector3(0, 0, 0);     // Center the model on the parent. 
        transform.Rotate(Vector3.forward, myZ,Space.World);
        name = "wall-model";

        mat = GetComponent<Renderer>().material;
        mat.shader = Shader.Find("Transparent/Diffuse");
        mat.mainTexture = Resources.Load<Texture2D>("Textures/marble");
        mat.color = new Color(1, 1, 1);
       
    }

    void FixedUpdate () {
        if (rotate)
        {            

            float magnitude = 180 / speed * Time.deltaTime;
            
            
            bool flag=forwards;

            if (forwards && ((transform.rotation.eulerAngles.z + magnitude) >= (startR + 180)))
            {
                magnitude = transform.rotation.eulerAngles.z - (startR + 180);               
                forwards = false;
                
            }
            else if ((transform.rotation.eulerAngles.z - magnitude <= startR) && (!forwards))
            {
                magnitude = startR - transform.rotation.eulerAngles.z;                
                forwards = true;
                
            }

            if (shouldShoot(magnitude, flag))
            {
                owner.shootProj(flag);
            }


            if (flag)
            {
               // print("forward: " + magnitude);
                transform.Rotate(Vector3.forward, magnitude, Space.World);

            }
            else
            {
               // print("backwards"+magnitude);
                transform.Rotate(Vector3.back, magnitude, Space.World);
            }

           


            print("End z: " + transform.rotation.eulerAngles.z);     

        }
	}

    bool shouldShoot(float magnitude, bool f)
    {
        float z = transform.rotation.eulerAngles.z;

        
           if (!f) {
               if ((transform.rotation.eulerAngles.z - magnitude) <= 360-(owner.mod((owner.curDir + 1), 8) * 45)) ;
               {
                   return true;
               }
           }
           else
           {
               if((transform.rotation.eulerAngles.z + magnitude) >= 360-(owner.mod((owner.curDir -1),8) * 45))
               {
                   return true;
               }
           }
           /*
        if (!f)
        {
            if ((transform.rotation.eulerAngles.z) <= 360 - (owner.mod((owner.curDir + 1), 8) * 45)) ;
            {
                return true;
            }
        }
        else
        {
            if ((transform.rotation.eulerAngles.z) >= 360 - (owner.mod((owner.curDir - 1), 8) * 45))
            {
                return true;
            }
        }*/

        return false;
        
    }
}
