using UnityEngine;
using System.Collections;

public class WinScreenModel : MonoBehaviour {

    public Material mat;
    public WinScreen owner;

	// Use this for initialization
	public void init (WinScreen owner) {

        this.owner = owner;
        transform.parent = owner.transform;                 // Set the model's parent to the gem.
        transform.localPosition = new Vector3(0, 0, 0);     // Center the model on the parent. 

        MeshRenderer mesh = GetComponent<MeshRenderer>();
        mesh.material = Resources.Load("WinScreen", typeof(Material)) as Material;

        mat = GetComponent<Renderer>().material;
        mat.shader = Shader.Find("Sprites/Default");
        //mat.color = new Color(1, 1, 1);
        
        Camera cam = Camera.main;
        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;
        transform.localScale = new Vector3(width, height, 1);

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
