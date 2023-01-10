using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GazeErrorInjector
{
    public class EyeData
    {
        public float Timestamp;
        public Vector3 GazeOrigin;
        public Vector3 GazeDirection;
        public bool isDataValid;

        public EyeData() { }

        public EyeData(float timestamp, Vector3 origin, Vector3 direction, bool isValid) 
        {
            Timestamp = timestamp;
            GazeOrigin = origin;
            GazeDirection = direction;
            isDataValid = isValid;
        }
    }
}