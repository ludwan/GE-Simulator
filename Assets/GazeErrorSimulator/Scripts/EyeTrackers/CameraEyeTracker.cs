using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GazeErrorSimulator
{
    public class CameraEyeTracker : EyeTracker
    {
        private Transform origin;

        /// <summary>
        /// Initialize the camera eye tracker.
        /// </summary>
        /// <returns>True if the camera was found, false otherwise</returns>
        public override bool Initialize()
        {
            origin = GetOriginTransform();

            return origin != null ? true : false;
        }

        /// <summary>
        /// Get the (simulated) gaze data from the camera.
        /// </summary>
        /// <returns>The gaze data from the camera</returns>
        public override GazeErrorData GetGazeData()
        {
            if (origin == null)  return null;

            GazeErrorData newData = new GazeErrorData();                
            //Gaze
            newData.Gaze.Timestamp = Time.unscaledTime;
            newData.Gaze.isDataValid = true;
            newData.Gaze.Direction = origin.rotation * Vector3.forward;
            newData.Gaze.Origin = origin.position;

            //Left Eye
            newData.LeftEye.Timestamp = newData.Gaze.Timestamp;
            newData.LeftEye.isDataValid = newData.Gaze.isDataValid;
            newData.LeftEye.Direction = newData.Gaze.Direction;
            newData.LeftEye.Origin = newData.Gaze.Origin;

            //Right Eye
            newData.RightEye.Timestamp = newData.Gaze.Timestamp;
            newData.RightEye.isDataValid = newData.Gaze.isDataValid;
            newData.RightEye.Direction = newData.Gaze.Direction;
            newData.RightEye.Origin = newData.Gaze.Origin;

            return newData;
        }

        /// <summary>
        /// Get the transform of the origin, in this case the main camera.
        /// </summary>
        /// <returns>The main camera transform</returns>
        public override Transform GetOriginTransform() 
        {
            return Camera.main.transform;
        }
    }
}