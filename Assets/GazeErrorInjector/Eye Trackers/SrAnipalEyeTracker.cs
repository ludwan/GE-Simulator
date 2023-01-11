using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if VIVE_SDK

using ViveSR.anipal.Eye;

#endif

namespace GazeErrorInjector
{
    public class SrAnipalEyeTracker : EyeTracker
    {

        #if VIVE_SDK
            private static ViveSR.anipal.Eye.EyeData eyeData = new ViveSR.anipal.Eye.EyeData();
            private static bool eye_callback_registered = false;

            public override bool Initialize()
            {
                if (!SRanipal_Eye_API.IsViveProEye()) return false;

                return (SRanipal_Eye_Framework.Status == SRanipal_Eye_Framework.FrameworkStatus.WORKING);                
            }
            
            public override GazeErrorData GetGazeData()
            {
                if (SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.WORKING &&
                            SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.NOT_SUPPORT) return null;

                
                GazeErrorData newData = new GazeErrorData();                
                //Gaze
                newData.Gaze.Timestamp = Time.unscaledTime;
                newData.Gaze.isDataValid = SRanipal_Eye.GetGazeRay(GazeIndex.COMBINE, out newData.Gaze.Origin, out newData.Gaze.Direction);

                //Left Eye
                newData.LeftEye.Timestamp = Time.unscaledTime;
                newData.LeftEye.isDataValid = SRanipal_Eye.GetGazeRay(GazeIndex.LEFT, out newData.LeftEye.Origin, out newData.LeftEye.Direction);

                //Right Eye
                newData.RightEye.Timestamp = Time.unscaledTime;
                newData.RightEye.isDataValid = SRanipal_Eye.GetGazeRay(GazeIndex.RIGHT, out newData.RightEye.Origin, out newData.RightEye.Direction);

                return newData;
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