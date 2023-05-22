using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if HOLOLENS_SDK
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
#endif

namespace GazeErrorSimulator
{
    public class HoloLensEyeTracker : EyeTracker
    {
#if HOLOLENS_SDK
        private IMixedRealityEyeGazeProvider _eyeGazeProvider;

        /// <summary>
        /// Initialize the HoloLens 2 Eye Tracker.
        /// </summary>
        /// <returns>True if eye tracking is enabled in MRTK</returns>
        public override bool Initialize()
        {
            // Get the eye gaze provider from MRTK
            _eyeGazeProvider = CoreServices.InputSystem.EyeGazeProvider;
            // Return true if the eye tracking is enabled in MRTK
            return _eyeGazeProvider.IsEyeTrackingEnabled;
        }
        
        /// <summary>
        /// Get the gaze data from the HoloLens 2 Eye Tracker.
        /// As the HoloLens 2 does not provide seperate data for the left and right eye,
        /// the left and right eye data will be the same as the cyclopean gaze data.
        /// </summary>
        /// <returns>The gaze data from the HoloLens 2 Eye Tracker</returns>
        public override GazeErrorData GetGazeData()
        {
            // Ensure that the eye tracking is both enabled and valid
            if (_eyeGazeProvider.IsEyeTrackingEnabledAndValid == false)
                return null;

            GazeErrorData newData = new GazeErrorData();

            // Set the raw data for the cyclopean gaze
            newData.Gaze.Timestamp = Time.unscaledTime;
            newData.Gaze.Origin = _eyeGazeProvider.GazeOrigin;
            newData.Gaze.Direction = _eyeGazeProvider.GazeDirection;
            newData.Gaze.isDataValid = _eyeGazeProvider.IsEyeTrackingEnabledAndValid;

            // HoloLens 2 does not provide seperate data for left and right eye...
            // So we set the left and right eye data to be the same as the cyclopean gaze
            newData.LeftEye = newData.Gaze;
            newData.RightEye = newData.Gaze;

            return newData;
        }

        /// <summary>
        /// Get the transform of the origin of the HoloLens 2.
        /// </summary>
        /// <returns>The main camera transform</returns>
        public override Transform GetOriginTransform()
        {
            // Use the main camera as the origin
            return Camera.main.transform;
        }
#else
        /// <summary>
        /// Default implementation of the Initialize method for the HoloLens 2 Eye Tracker.
        /// Always returns false when the HOLOLENS_SDK compile flag is not set.
        /// </summary>
        /// <returns>False</returns>
        public override bool Initialize()
        {
            Debug.LogError("Could not initialize HoloLens2 Eye Tracker.");
            return false;
        }
#endif
    }
}