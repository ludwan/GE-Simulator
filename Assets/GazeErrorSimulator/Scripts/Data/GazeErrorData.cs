using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GazeErrorSimulator
{
    /// <summary>
    /// Original or simulated eye tracking data.
    /// </summary>
    public enum DataType
    {
        Error,
        Original
    }

    /// <summary>
    /// The eye you wish to get data from.
    /// </summary>
    public enum Eye
    {
        Gaze,
        Right,
        Left
    }
    public class GazeErrorData
    {
        /// <summary>
        /// The current error mode.
        /// </summary>
        public ErrorMode Mode;

        /// <summary>
        /// Error data for gaze (eyes combined).
        /// </summary>
        public EyeErrorData Gaze;

        /// <summary>
        /// Error data for the right eye.
        /// </summary>
        public EyeErrorData RightEye;

        /// <summary>
        /// Error data for the left eye.
        /// </summary>
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

        /// <summary>
        /// Get gaze ray associated with errortype and eye.
        /// </summary>
        /// <param name="eye">Data from specified eye.</param>
        /// <param name="type">Original or error simulated data.</param>
        /// <returns>Gaze ray from specified data.</returns>
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

        /// <summary>
        /// Get error simulated gaze ray associated with eye.
        /// </summary>
        /// <param name="eye">Data from specified eye.</param>
        /// <returns>Error simulated gaze ray from specified eye.</returns>
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

        /// <summary>
        /// Get original gaze ray associated with eye.
        /// </summary>
        /// <param name="eye">Data from specified eye.</param>
        /// <returns>Original gaze ray from specified eye.</returns>
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