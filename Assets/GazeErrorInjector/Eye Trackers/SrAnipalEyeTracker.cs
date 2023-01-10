using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if VIVE_SDK

#endif

namespace GazeErrorInjector
{
    public class SrAnipalEyeTracker : EyeTracker
    {

        #if VIVE_SDK
            public override bool Initialize()
            {
                //TODO: ADD GAMOBJECT.
                if (!SRanipal_Eye_API.IsViveProEye()) return false;

                return (SRanipal_Eye_Framework.Status == SRanipal_Eye_Framework.FrameworkStatus.WORKING);                
            }
            
            public override GazeErrorData GetGazeData()
            {
                if (SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.WORKING &&
                            SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.NOT_SUPPORT) return;
                
                _localEyeGazeData.Timestamp = Time.unscaledTime;
                _localEyeGazeData.isRayValid = SRanipal_Eye.GetGazeRay(GazeIndex.COMBINE, out _localEyeGazeData.Origin, out _localEyeGazeData.Direction);

                return null;
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
                Debug.LogError("Could not initialize SRanipal Eye Tracker.");
                return false;
            }
        #endif
        
    }
}