using UnityEngine;
using System.Collections;

public class TitleScreen : MonoBehaviour {

    public Renderer rend;
    public TitleScreenModel model;

	// Use this for initialization
	public void init () {

        var modelObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
        model = modelObject.AddComponent<TitleScreenModel>();
        model.init(this);

        rend = model.GetComponent<Renderer>();
        
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
