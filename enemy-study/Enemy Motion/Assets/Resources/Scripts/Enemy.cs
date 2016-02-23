using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    public EnemyModel model;

    // Use this for initialization
    public void init(MotionDemo demo)
    {
        var modelObject = new GameObject();
        model = modelObject.AddComponent<EnemyModel>();
        model.init(this, demo);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
