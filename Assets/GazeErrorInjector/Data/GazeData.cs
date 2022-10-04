using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GazeErrorInjector
{
    public class GazeErrorData : MonoBehaviour
    {
        public float Timestamp;
        public Vector3 GazeOrigin;
        public Vector3 GazeDirection;
        public Vector3 GazeErrorDirection;

        public float AccuracyErrorDirection;
        public float AccuracyErrorAmplitude;
        
        //public float 

        public bool isRayValid;
        // public float Distance;  
        // public bool isDistanceValid;
        // public bool isRayValid;




    }
}

