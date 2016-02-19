using UnityEngine;
using System.Collections;

public static class Const : MonoBehaviour {

    // PARAMETERS
    public const int CAMERA_MODE = LEASH;
    public const int CONTROL_MODE = DUAL;
    public const int WIDTH = 20;
    public const int HEIGHT = 20;
    public const float CAMERA_LEASH_SCALE = 1.0f;
    public const float CAMERA_DYNAMIC_FACTOR = 1.0f;
    public const float CAMERA_SPLIT_SCALE = 1.0f;

    // ENUMS
    //  CAMERA_MODE
    public const int LEASH = 0;
    public const int DYNAMIC = 1;
    public const int SPLIT = 2;
    public const int SWITCH = 3;
    // CONTROL_MODE
    public const int DUAL = 0;
    public const int SWITCHING = 1;
}
