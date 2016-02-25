using UnityEngine;
using System.Collections;

public class MainCamera : MonoBehaviour {

    GameObject one;
    GameObject two;

    public void init(GameObject one, GameObject two) {
        this.one = one;
        this.two = two;
        reposition();
    }

    void LateUpdate() { reposition(); }

    public void reposition() {
        Vector3 v1 = one.transform.position;
        Vector3 v2 = two.transform.position;
        float x1 = v1.x;
        float y1 = v1.y;
        float x2 = v2.x;
        float y2 = v2.y;
        float diff = (v1-v2).magnitude;
    }
}
