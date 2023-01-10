using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if VARJO_SDK

#endif

namespace GazeErrorInjector
{
    public class VarjoEyeTracker : EyeTracker
    {

        #if VARJO_SDK
            public override bool Initialize()
            {
                throw new System.NotImplementedException();
            }

            public override GazeErrorData GetGazeData()
            {
                throw new System.NotImplementedException();
            }

            public override Transform GetOriginTransform() 
            { 
                throw new System.NotImplementedException();
            }

            public override void Destroy()
            {
                throw new System.NotImplementedException();
            }
        #else
            public override bool Initialize()
            {
                Debug.LogError("Could not initialize Varjo Eye Tracker.");
                return false;
            }
        #endif
    }
}