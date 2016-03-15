using UnityEngine;
using System.Collections;

public class TutorialScreen : MonoBehaviour {
    
    public TutorialScreenModel model;
    public Renderer rend;

	// Use this for initialization
	public void init () {

        var modelObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
        model = modelObject.AddComponent<TutorialScreenModel>();
        model.init(this);

        rend = model.GetComponent<Renderer>();
        
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
