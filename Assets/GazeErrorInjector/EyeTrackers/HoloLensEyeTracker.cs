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

        public override bool Initialize()
        {
            // Get the eye gaze provider from MRTK
            _eyeGazeProvider = CoreServices.InputSystem.EyeGazeProvider;
            // Return true if the eye tracking is enabled in MRTK
            return _eyeGazeProvider.IsEyeTrackingEnabled;
        }

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

        public override Transform GetOriginTransform()
        {
            // Use the main camera as the origin
            return Camera.main.transform;
        }
#endif
    }
}