using UnityEngine;
using System.Collections;

public class WinScreen : MonoBehaviour {
    
    public WinScreenModel model;
    public Renderer rend;

	// Use this for initialization
	public void init () {

        var modelObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
        model = modelObject.AddComponent<WinScreenModel>();
        model.init(this);

        rend = model.GetComponent<Renderer>();
        
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
