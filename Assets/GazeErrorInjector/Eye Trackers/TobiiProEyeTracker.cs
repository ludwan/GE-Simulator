using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if TOBII_SDK

#endif

namespace GazeErrorInjector
{
    public class TobiiProEyeTracker : MonoBehaviour, EyeTracker
    {
        private GazeErrorData _latestdata = new GazeErrorData();
        public GazeErrorData LatestData 
        {
            get
            {
                return _latestdata;
            }
        }

        #if TOBII_SDK
            public bool Initialize()
            {
                throw new System.NotImplementedException();
            }

            public void GetGazeData()
            {
                throw new System.NotImplementedException();
            }

            public Transform GetOriginTransform() 
            { 
                throw new System.NotImplementedException();
            }

            public void Destroy()
            {
                throw new System.NotImplementedException();
            }
        #else
            public bool Initialize()
            {
                Debug.LogError("Could not initialize TobiiXR Eye Tracker.");
                return false;
            }

            public void GetGazeData() { }

            public Transform GetOriginTransform() { return null; }

            public void Destroy() { }
        #endif
    }
}