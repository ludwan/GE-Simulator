using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if HOLOLENS_SDK

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

            public bool Initialize()
            {
                throw new System.NotImplementedException();
            }

            public GazeErrorData GetGazeData()
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