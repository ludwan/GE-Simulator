using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if HOLOLENS_SDK
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
#endif

namespace GazeErrorInjector
{
    public class HoloLensEyeTracker : MonoBehaviour, IEyeTracker
    {

        private GazeErrorData _latestdata = new GazeErrorData();
        public GazeErrorData LatestData
        {
            get
            {
                return _latestdata;
            }
        }

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

                _latestdata = gazeErrorData;

                return gazeErrorData;
            }

            public override void GetOrigin()
            {
                throw new System.NotImplementedException();
            }

            public override Transform GetOriginTransform() 
            { 
                return Camera.main.transform;
            }

            public void Destroy() { }

#else
        //TODO A LOT OF CODE REPETITION WITHIN THIS PART.

        public bool Initialize()
        {
            Debug.LogError("Could not initialize HoloLens 2 Eye Tracker.");
            return false;
        }

#endif
    }
}