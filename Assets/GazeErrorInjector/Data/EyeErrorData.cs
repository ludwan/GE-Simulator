using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GazeErrorInjector
{
    public class EyeErrorData : EyeData
    {
        public Vector3 GazeErrorDirection;
        public float AccuracyErrorDirection;
        public float AccuracyErrorAmplitude;
        public float PrecisionError;
        public float DataLossProbability;

        public EyeErrorData () { }

        public EyeErrorData (EyeData data) 
        {
            this.Timestamp = data.Timestamp;
            GazeOrigin = data.GazeOrigin;
            GazeDirection = data.GazeDirection;
            isDataValid = data.isDataValid;

            GazeErrorDirection = GazeDirection;
        }
    }
}

