using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if TOBII_SDK

#endif

namespace GazeErrorInjector
{
    public class TobiiProEyeTracker : EyeTracker
    {

        #if TOBII_SDK
            public bool Initialize()
            {
                throw new System.NotImplementedException();
            }

            public GazeErrorData GetGazeData()
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
            public override bool Initialize()
            {
                Debug.LogError("Could not initialize TobiiXR Eye Tracker.");
                return false;
            }

        #endif
    }
}