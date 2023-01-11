using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GazeErrorInjector
{
    public class EyeErrorData : EyeData
    {
        public Vector3 ErrorDirection;
        public bool ErrorDataLoss;
        public float AccuracyErrorDirection;
        public float AccuracyError;
        public PrecisionErrorMode PrecisionMode;
        public float PrecisionError;
        public float DataLossProbability;

        public EyeErrorData () { }

        public EyeErrorData (EyeData data) 
        {
            this.Timestamp = data.Timestamp;
            Origin = data.Origin;
            Direction = data.Direction;
            isDataValid = data.isDataValid;

            ErrorDirection = Direction;
        }
    }
}

