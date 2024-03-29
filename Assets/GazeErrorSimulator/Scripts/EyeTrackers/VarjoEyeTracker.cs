using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;

#if VARJO_SDK
using Varjo.XR;
#endif

namespace GazeErrorSimulator
{
    public class VarjoEyeTracker : EyeTracker
    {
#if VARJO_SDK
        /// <summary>
        /// Initialize the Varjo Eye Tracker.
        /// This method must be called before calling GetGazeData.
        /// Also sets the gaze output frequency to the maximum supported value.
        /// </summary>
        /// <returns>True if gaze is allowed and calibrated</returns>
        public override bool Initialize()
        {
            VarjoEyeTracking.SetGazeOutputFrequency(VarjoEyeTracking.GazeOutputFrequency.MaximumSupported);
            return VarjoEyeTracking.IsGazeAllowed() && VarjoEyeTracking.IsGazeCalibrated();
        }

        public override GazeErrorData GetGazeData()
        {
            if (VarjoEyeTracking.IsGazeAllowed() == false || VarjoEyeTracking.IsGazeCalibrated() == false)
                return null;

            // Get gaze data if gaze is allowed and calibrated
            VarjoEyeTracking.GazeData gazeData = VarjoEyeTracking.GetGaze();

            GazeErrorData newData = new GazeErrorData();

            // Left eye
            newData.LeftEye.Timestamp = Time.unscaledTime;
            newData.LeftEye.isDataValid = (gazeData.leftStatus != VarjoEyeTracking.GazeEyeStatus.Invalid);
            newData.LeftEye.Origin = GetOriginTransform().TransformPoint(gazeData.left.origin);
            newData.LeftEye.Direction = GetOriginTransform().TransformDirection(gazeData.left.forward);

            // Right eye
            newData.RightEye.Timestamp = Time.unscaledTime;
            newData.RightEye.isDataValid = (gazeData.rightStatus != VarjoEyeTracking.GazeEyeStatus.Invalid);
            newData.RightEye.Origin = GetOriginTransform().TransformPoint(gazeData.right.origin);
            newData.RightEye.Direction = GetOriginTransform().TransformDirection(gazeData.right.forward);

            // Gaze
            newData.Gaze.Timestamp = Time.unscaledTime;
            newData.Gaze.isDataValid = gazeData.status != VarjoEyeTracking.GazeStatus.Invalid && newData.LeftEye.isDataValid && newData.RightEye.isDataValid;
            newData.Gaze.Origin = GetOriginTransform().TransformPoint(gazeData.gaze.origin);
            newData.Gaze.Direction = GetOriginTransform().TransformDirection(gazeData.gaze.forward);

            return newData;
        }

        public override Transform GetOriginTransform()
        {
            return Camera.main.transform;
        }

#else
        /// <summary>
        /// Default implementation of the Initialize method for the Varjo Eye Tracker.
        /// Always returns false when the VARJO_SDK compile flag is not set.
        /// </summary>
        /// <returns>False</returns>
        public override bool Initialize()
        {
            Debug.LogError("Could not initialize Varjo Eye Tracker.");
            return false;
        }
#endif
    }
}