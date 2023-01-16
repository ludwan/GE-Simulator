using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if HOLOLENS_SDK
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
#endif

namespace GazeErrorInjector
{
    public class HoloLensEyeTracker : EyeTracker
    {
#if HOLOLENS_SDK
        private IMixedRealityEyeGazeProvider _eyeGazeProvider;

            public override bool Initialize()
            {
                _eyeGazeProvider = CoreServices.InputSystem.EyeGazeProvider;
                return _eyeGazeProvider.IsEyeTrackingEnabled;
            }

            // void Update()
            // {
            //     LatestData = GetGazeData();
            // }

            public override GazeErrorData GetGazeData()
            {
                if (_eyeGazeProvider.IsEyeTrackingEnabledAndValid == false)
                    return null;

                GazeErrorData newData = new GazeErrorData();

                // Gaze
                newData.Gaze.Timestamp = Time.unscaledTime;
                newData.Gaze.Origin = _eyeGazeProvider.GazeOrigin;
                newData.Gaze.Direction = _eyeGazeProvider.GazeDirection;
                newData.Gaze.isDataValid = _eyeGazeProvider.IsEyeTrackingEnabledAndValid;

                // HoloLens 2 does not provide seperate data for left and right eye...
                newData.LeftEye = newData.Gaze;
                newData.RightEye = newData.Gaze;

                return newData;
            }

            public override Transform GetOriginTransform() 
            { 
                return Camera.main.transform;
            }

            public override void Destroy() { }
#else
        //TODO A LOT OF CODE REPETITION WITHIN THIS PART.

        public override bool Initialize()
        {
            Debug.LogError("Could not initialize HoloLens 2 Eye Tracker.");
            return false;
        }
#endif
    }
}