using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

    public const int FIRE = 0;
    public const int OIL = 1;
    public const int ENEMY = 2;

    public int type;
    public Vector3 v;

    int spriteswitch = 1;
    string spritename;

    Material mat;

    // Use this for initialization
    void init(Vector3 startPos, Vector3 velocity, int type)
    {
        this.type = type;
        this.transform.position = startPos;
        this.v = velocity;
        if (type == 2) this.tag = "projectile-enemy";
        else this.tag = "projectile-friendly";

        mat = GetComponent<Renderer>().material;
        mat.shader = Shader.Find("Sprites/Default");
        
        switch (type)
        {
            case FIRE:
                spritename = "proj_fire"; break;
            case OIL:
                spritename = "proj_oil"; break;
            case ENEMY:
                spritename = "proj_enemy"; break;
        }
        mat.mainTexture = Resources.Load<Texture2D>("Textures/" + spritename + 1);
    }
	
	// Update is called once per frame
	void Update () {
        // move!
        this.transform.position += v * Time.deltaTime;
        mat.mainTexture = Resources.Load<Texture2D>("Textures/" + spritename + spriteswitch);
        if (spriteswitch == 2) spriteswitch = 1;
        else spriteswitch = 2;
	}

    void OnTriggerEnter(Collider other)
    {
        string tagOther = other.gameObject.tag;

        if (type == ENEMY && tagOther == "enemy") ;
        else if (type != ENEMY && (tagOther == "fire" || tagOther == "oil")) ;
        else Destroy(this.gameObject);
    }
}
