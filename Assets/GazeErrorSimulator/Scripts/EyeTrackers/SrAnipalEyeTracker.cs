using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if VIVE_SDK
using System.Runtime.InteropServices;
using ViveSR.anipal.Eye;
#endif

namespace GazeErrorSimulator
{
    public class SrAnipalEyeTracker : EyeTracker
    {
#if VIVE_SDK
        private static ViveSR.anipal.Eye.EyeData eyeData = new ViveSR.anipal.Eye.EyeData();
        private static bool eye_callback_registered = false;
        private Transform origin;

        public override bool Initialize()
        {
            if (!SRanipal_Eye_API.IsViveProEye()) return false;
            origin = GetOriginTransform();

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
            if(newData.Gaze.Direction == Vector3.forward)
            {
                newData.Gaze.isDataValid = false;
            }
            newData.Gaze.Direction = origin.TransformDirection(newData.Gaze.Direction);
            newData.Gaze.Origin = origin.TransformPoint(newData.Gaze.Origin);

            //Left Eye
            newData.LeftEye.Timestamp = Time.unscaledTime;
            newData.LeftEye.isDataValid = SRanipal_Eye.GetGazeRay(GazeIndex.LEFT, out newData.LeftEye.Origin, out newData.LeftEye.Direction);
            if(newData.LeftEye.Direction == Vector3.forward)
            {
                newData.LeftEye.isDataValid = false;
            }
            newData.LeftEye.Direction = origin.TransformDirection(newData.LeftEye.Direction);
            newData.LeftEye.Origin = origin.TransformPoint(newData.LeftEye.Origin);

            //Right Eye
            newData.RightEye.Timestamp = Time.unscaledTime;
            newData.RightEye.isDataValid = SRanipal_Eye.GetGazeRay(GazeIndex.RIGHT, out newData.RightEye.Origin, out newData.RightEye.Direction);
            if(newData.LeftEye.Direction == Vector3.forward)
            {
                newData.LeftEye.isDataValid = false;
            }
            newData.RightEye.Direction = origin.TransformDirection(newData.RightEye.Direction);
            newData.RightEye.Origin = origin.TransformPoint(newData.RightEye.Origin);

            return newData;
        }

        public override Transform GetOriginTransform() 
        { 
            return Camera.main.transform;
        }
#else
        public override bool Initialize()
        {
            Debug.LogError("Could not initialize SRanipal Eye Tracker.");
            return false;
        }
#endif
    }
}