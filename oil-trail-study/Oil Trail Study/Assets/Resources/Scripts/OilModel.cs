using UnityEngine;
using System.Collections;

public class OilModel : MonoBehaviour {

    // Use this for initialization

    bool isCharacter;
    OilBall b;
    OilPatch p;
    private Material mat;

    public void init(bool isCharacter, OilBall b, OilPatch p)
    {
        this.isCharacter = isCharacter;
        this.b = b;
        this.p = p;

        if (isCharacter)
        {
            transform.parent = b.transform;
            transform.localPosition = new Vector3(0, 0, 0);
            name = "Oil Ball Model";
            mat = GetComponent<Renderer>().material;
            mat.renderQueue = 5001;
            mat.color = new Color(1, 1, 1);
            mat.shader = Shader.Find("Transparent/Diffuse");
            mat.mainTexture = Resources.Load<Texture2D>("Textures/ball");
        }
        else
        {
            transform.parent = p.transform;
            transform.localPosition = new Vector3(0, 0, 0);
            name = "Oil Patch Model";
            mat = GetComponent<Renderer>().material;
            mat.renderQueue = 4998;
            mat.color = new Color(1, 1, 1);
            mat.shader = Shader.Find("Transparent/Diffuse");
            mat.mainTexture = Resources.Load<Texture2D>("Textures/patch");
        }
    }

    public void putOnFire()
    {
        if (!isCharacter)
        {
            mat.renderQueue = 4999;
            mat.mainTexture = Resources.Load<Texture2D>("Textures/Fire");
        }
    }
}
