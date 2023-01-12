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

            public override GazeErrorData GetGazeData()
            {
                if (_eyeGazeProvider.IsEyeTrackingEnabledAndValid == false)
                    return null;

                GazeErrorData gazeErrorData = new GazeErrorData();

                gazeErrorData.Gaze.Timestamp = Time.unscaledTime;
                gazeErrorData.Gaze.Origin = _eyeGazeProvider.GazeOrigin;
                gazeErrorData.Gaze.Direction = _eyeGazeProvider.GazeDirection;
                gazeErrorData.Gaze.isDataValid = _eyeGazeProvider.IsEyeTrackingEnabledAndValid;

                LatestData = gazeErrorData;

                return gazeErrorData;
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