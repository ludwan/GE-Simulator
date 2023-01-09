using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if HOLOLENS_SDK

#endif

namespace GazeErrorInjector
{
    public class HoloLensEyeTracker : MonoBehaviour, EyeTracker
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

            public bool Initialize()
            {
                throw new System.NotImplementedException();
            }

            public void GetGazeData()
            {
                throw new System.NotImplementedException();
            }

            public void GetOrigin()
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
                Debug.LogError("Could not initialize HoloLens 2 Eye Tracker.");
                return false;
            }

            public void GetGazeData() { }

            public Transform GetOriginTransform() { return null; }

            public void Destroy() { }
        #endif
    }
}