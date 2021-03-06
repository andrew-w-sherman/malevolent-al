﻿using UnityEngine;
using System.Collections;

public static class Param {

    // PARAMETERS
    public const int CAMERA_MODE = DYNAMIC;
    public const int CONTROL_MODE = DUAL;
    public const float CAMERA_LEASH_SCALE = 1.0f;
    public const float CAMERA_DYNAMIC_FACTOR = 1.0f;
    public const float CAMERA_SPLIT_SCALE = 1.0f;
    public const float BASE_SPEED = 1.0f; // tiles/sec
    public const float ATTK_TIME = 1.5f;

    // ENUMS
    //  CAMERA_MODE
    public const int DYNAMIC = 0;
    public const int STATIC = 1;
    // CONTROL_MODE
    public const int DUAL = 0;
    public const int SWITCHING = 1;

    // BOARD
    public static int [,] BOARD = {{0,0,0,0,1,1,0,1,0,1,0},
                                  {0,0,1,0,0,1,0,0,0,0,0},
                                  {0,0,0,0,1,1,0,0,0,0,0},
                                  {0,0,0,0,0,0,0,0,0,1,0},
                                  {0,0,0,0,0,0,0,0,0,1,0},
                                  {0,0,0,0,0,0,0,0,0,1,0},
                                  {0,0,0,0,0,0,0,0,0,1,0},
                                  {0,0,0,0,0,0,0,0,0,1,0},
                                  {0,0,0,0,0,0,0,0,0,1,0},
                                  {0,0,0,0,0,0,0,0,0,1,0}};

}
