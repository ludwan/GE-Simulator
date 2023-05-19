using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GazeErrorSimulator
{
    public enum DataType
    {
        Error,
        Original
    }

    public enum Eye
    {
        Gaze,
        Right,
        Left
    }
    public class GazeErrorData
    {
        public ErrorMode Mode;

        public EyeErrorData Gaze;

        public EyeErrorData RightEye;

        public EyeErrorData LeftEye;

        public GazeErrorData() 
        {
            Gaze = new EyeErrorData();
            RightEye = new EyeErrorData();
            LeftEye = new EyeErrorData();
        }

        public GazeErrorData (ErrorMode mode, EyeData gaze, EyeData leftEye, EyeData rightEye) 
        {
            Mode = mode;
            Gaze = new EyeErrorData(gaze);
            LeftEye = new EyeErrorData(leftEye);
            RightEye = new EyeErrorData(rightEye);
        }

        public Ray GetGazeRay(Eye eye, DataType type)
        {
            Ray ray = new Ray();
            
            // Get the ray based on the selected data error type
            switch (type)
            {
                case DataType.Error:
                    ray = GetErrorRay(eye);
                    break;
                case DataType.Original:
                    ray = GetOriginalRay(eye);
                    break;
            }

            return ray;
        }

        private Ray GetErrorRay(Eye eye)
        {
            Ray ray = new Ray();

            // Get the error ray based on the selected eye
            switch (eye)
            {
                case Eye.Gaze:
                    ray.origin = Gaze.Origin;
                    ray.direction = Gaze.ErrorDirection;
                    break;
                case Eye.Left:
                    ray.origin = LeftEye.Origin;
                    ray.direction = LeftEye.ErrorDirection;
                    break;
                case Eye.Right:
                    ray.origin = RightEye.Origin;
                    ray.direction = RightEye.ErrorDirection;
                    break;
            }

            return ray;
        }

        private Ray GetOriginalRay(Eye eye)
        {
            Ray ray = new Ray();

            // Get the original ray based on the selected eye
            switch (eye)
            {
                case Eye.Gaze:
                    ray.origin = Gaze.Origin;
                    ray.direction = Gaze.Direction;
                    break;
                case Eye.Left:
                    ray.origin = LeftEye.Origin;
                    ray.direction = LeftEye.Direction;
                    break;
                case Eye.Right:
                    ray.origin = RightEye.Origin;
                    ray.direction = RightEye.Direction;
                    break;
            }

            return ray;
        }
    }
}