using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if VIVE_SDK
using System.Runtime.InteropServices;
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

            if (SRanipal_Eye_Framework.Instance.EnableEyeDataCallback == true && eye_callback_registered == false)
            {
                SRanipal_Eye.WrapperRegisterEyeDataCallback(Marshal.GetFunctionPointerForDelegate((SRanipal_Eye.CallbackBasic)EyeCallback));
                eye_callback_registered = true;
            }
            else if (SRanipal_Eye_Framework.Instance.EnableEyeDataCallback == false && eye_callback_registered == true)
            {
                SRanipal_Eye.WrapperUnRegisterEyeDataCallback(Marshal.GetFunctionPointerForDelegate((SRanipal_Eye.CallbackBasic)EyeCallback));
                eye_callback_registered = false;
            }

            return (SRanipal_Eye_Framework.Status == SRanipal_Eye_Framework.FrameworkStatus.WORKING);
        }

        public override GazeErrorData GetGazeData()
        {
            if (SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.WORKING &&
                        SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.NOT_SUPPORT) return null;

            Transform origin = GetOriginTransform();

            GazeErrorData newData = new GazeErrorData();
            //Gaze
            newData.Gaze.Timestamp = Time.unscaledTime;
            newData.Gaze.isDataValid = SRanipal_Eye.GetGazeRay(GazeIndex.COMBINE, out newData.Gaze.Origin, out newData.Gaze.Direction);
            newData.Gaze.Direction = origin.TransformDirection(newData.Gaze.Direction);
            newData.Gaze.Origin = origin.TransformPoint(newData.Gaze.Origin);

            //Left Eye
            newData.LeftEye.Timestamp = Time.unscaledTime;
            newData.LeftEye.isDataValid = SRanipal_Eye.GetGazeRay(GazeIndex.LEFT, out newData.LeftEye.Origin, out newData.LeftEye.Direction);
            newData.LeftEye.Direction = origin.TransformDirection(newData.LeftEye.Direction);
            newData.LeftEye.Origin = origin.TransformPoint(newData.LeftEye.Origin);

            //Right Eye
            newData.RightEye.Timestamp = Time.unscaledTime;
            newData.RightEye.isDataValid = SRanipal_Eye.GetGazeRay(GazeIndex.RIGHT, out newData.RightEye.Origin, out newData.RightEye.Direction);
            newData.RightEye.Direction = origin.TransformDirection(newData.RightEye.Direction);
            newData.RightEye.Origin = origin.TransformPoint(newData.RightEye.Origin);

            return newData;
        }

        public override Transform GetOriginTransform()
        {
            return Camera.main.transform;
        }

        public void Destroy()
        {
            SRanipal_Eye.WrapperUnRegisterEyeDataCallback(Marshal.GetFunctionPointerForDelegate((SRanipal_Eye.CallbackBasic)EyeCallback));
            eye_callback_registered = false;
        }

        private static void EyeCallback(ref ViveSR.anipal.Eye.EyeData eye_data)
        {
            eyeData = eye_data;
        }
#endif
    }
}