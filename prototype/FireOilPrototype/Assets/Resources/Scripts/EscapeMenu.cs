using UnityEngine;
using System.Collections;

public class EscapeMenu : MonoBehaviour {

    public GameController controller;
    public bool isActive;

	// Use this for initialization
	public void init (GameController controller) {
        this.controller = controller;
        isActive = false;
	}
	
    public void SetActive(bool isActive)
    {
        this.isActive = isActive;
    }

	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        if (isActive)
        {
            Vector3 camCenter = controller.cam.transform.position;
            GUI.Label(new Rect(camCenter.x, camCenter.y, 100, 30), "Menu");
        }
    }
}
