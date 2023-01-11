using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GazeErrorInjector
{
    public class EyeData
    {
        public float Timestamp;
        public Vector3 Origin;
        public Vector3 Direction;
        public bool isDataValid;

        public EyeData() { }

        public EyeData(float timestamp, Vector3 origin, Vector3 direction, bool isValid) 
        {
            Timestamp = timestamp;
            Origin = origin;
            Direction = direction;
            isDataValid = isValid;
        }
    }
}