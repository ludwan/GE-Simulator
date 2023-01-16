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
        #endif
    }
}