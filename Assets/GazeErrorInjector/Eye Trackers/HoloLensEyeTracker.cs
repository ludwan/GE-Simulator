#define HOLOLENS_SDK

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

            public bool Initialize()
            {
                _eyeGazeProvider = CoreServices.InputSystem.EyeGazeProvider;
                return _eyeGazeProvider.IsEyeTrackingEnabled;
            }

            public GazeErrorData GetGazeData()
            {
                if (_eyeGazeProvider.IsEyeTrackingEnabledAndValid == false)
                    return null;

                GazeErrorData gazeErrorData = new GazeErrorData();

                gazeErrorData.Gaze.Timestamp = Time.unscaledTime;
                gazeErrorData.Gaze.GazeOrigin = _eyeGazeProvider.GazeOrigin;
                gazeErrorData.Gaze.GazeDirection = _eyeGazeProvider.GazeDirection;
                gazeErrorData.Gaze.isDataValid = _eyeGazeProvider.IsEyeTrackingEnabledAndValid;

                return gazeErrorData;
            }

            public void GetOrigin()
            {
                throw new System.NotImplementedException();
            }

            public Transform GetOriginTransform() 
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

            public GazeErrorData GetGazeData() { return null; }

            public Transform GetOriginTransform() { return null; }

            public void Destroy() { }
        #endif
    }
}