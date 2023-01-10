using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GazeErrorInjector
{
    public class GazeData
    {
        public float Timestamp;
        public Vector3 GazeOrigin;
        public Vector3 GazeDirection;
        public bool isRayValid;
    }
}