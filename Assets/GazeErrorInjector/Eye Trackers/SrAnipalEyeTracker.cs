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
            private Transform origin;

            public override bool Initialize()
            {
                if (!SRanipal_Eye_API.IsViveProEye()) return false;

                origin = GetOriginTransform();

                return (SRanipal_Eye_Framework.Status == SRanipal_Eye_Framework.FrameworkStatus.WORKING);


            }

            // void Update()
            // {
            //     if (SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.WORKING) return;

            //     if (SRanipal_Eye_Framework.Instance.EnableEyeDataCallback == true && eye_callback_registered == false)
            //     {
            //         SRanipal_Eye.WrapperRegisterEyeDataCallback(Marshal.GetFunctionPointerForDelegate((SRanipal_Eye.CallbackBasic)EyeCallback));
            //         eye_callback_registered = true;
            //     }
            //     else if (SRanipal_Eye_Framework.Instance.EnableEyeDataCallback == false && eye_callback_registered == true)
            //     {
            //         SRanipal_Eye.WrapperUnRegisterEyeDataCallback(Marshal.GetFunctionPointerForDelegate((SRanipal_Eye.CallbackBasic)EyeCallback));
            //         eye_callback_registered = false;
            //     }
            //     if (eye_callback_registered)
            //     {
            //         LatestData = GetGazeData();
            //     }
            // }
            
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
        //TODO A LOT OF CODE REPETITION WITHIN THIS PART.
            public override bool Initialize()
            {
                Debug.LogError("Could not initialize SRanipal Eye Tracker.");
                return false;
            }
        #endif
        
    }
}

// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// #if VIVE_SDK
// using System.Runtime.InteropServices;
// using ViveSR.anipal.Eye;

// #endif

// namespace GazeErrorInjector
// {
//     public class SrAnipalEyeTracker : EyeTracker
//     {

//         #if VIVE_SDK

//             private delegate void NewData();
//             private static event NewData OnNewViveData;
//             private static ViveSR.anipal.Eye.EyeData _eyeData = new ViveSR.anipal.Eye.EyeData();

//             private static bool eye_callback_registered = false;

//             public override bool Initialize()
//             {
//                 if (!SRanipal_Eye_API.IsViveProEye()) return false;

//                 if (SRanipal_Eye_Framework.Status == SRanipal_Eye_Framework.FrameworkStatus.WORKING)
//                 {
//                     OnNewViveData += HandleCallbackData;
//                 }

//                 return (SRanipal_Eye_Framework.Status == SRanipal_Eye_Framework.FrameworkStatus.WORKING);                
//             }

//             void Update()
//             {
//                 if (SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.WORKING) return;

//                 if (SRanipal_Eye_Framework.Instance.EnableEyeDataCallback == true && eye_callback_registered == false)
//                 {
//                     SRanipal_Eye.WrapperRegisterEyeDataCallback(Marshal.GetFunctionPointerForDelegate((SRanipal_Eye.CallbackBasic)EyeCallback));
//                     eye_callback_registered = true;
//                 }
//                 else if (SRanipal_Eye_Framework.Instance.EnableEyeDataCallback == false && eye_callback_registered == true)
//                 {
//                     SRanipal_Eye.WrapperUnRegisterEyeDataCallback(Marshal.GetFunctionPointerForDelegate((SRanipal_Eye.CallbackBasic)EyeCallback));
//                     eye_callback_registered = false;
//                 }
//             }

//             public void HandleCallbackData()
//             {
//                 Debug.Log("TEST");
//                 LatestData = GetGazeData();
//             }
            
//             public override GazeErrorData GetGazeData()
//             {
//                 Debug.Log(SRanipal_Eye_Framework.Status);
//                 if (SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.WORKING &&
//                             SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.NOT_SUPPORT) return null;

//                 GazeErrorData newData = new GazeErrorData();                
//                 //Gaze
//                 newData.Gaze.Timestamp = _eyeData.timestamp;
//                 newData.Gaze.isDataValid = _eyeData.verbose_data.combined.eye_data.GetValidity(SingleEyeDataValidity.SINGLE_EYE_DATA_GAZE_DIRECTION_VALIDITY);
//                 if(newData.LeftEye.isDataValid)
//                 {
//                     newData.Gaze.Direction = _eyeData.verbose_data.combined.eye_data.gaze_direction_normalized;
//                     newData.Gaze.Direction.x *= -1;
//                     newData.Gaze.Origin = _eyeData.verbose_data.combined.eye_data.gaze_origin_mm * 0.001f;
//                 }
//                 //Left Eye
//                 newData.LeftEye.Timestamp = _eyeData.timestamp;
//                 newData.LeftEye.isDataValid = _eyeData.verbose_data.left.GetValidity(SingleEyeDataValidity.SINGLE_EYE_DATA_GAZE_DIRECTION_VALIDITY);
//                 if(newData.LeftEye.isDataValid)
//                 {
//                     newData.LeftEye.Direction = _eyeData.verbose_data.left.gaze_direction_normalized;
//                     newData.LeftEye.Direction.x *= -1;
//                     newData.LeftEye.Origin = _eyeData.verbose_data.left.gaze_origin_mm * 0.001f;
//                     newData.LeftEye.Origin.x *= -1;
//                 }
                
//                 //Right Eye
//                 newData.RightEye.Timestamp = _eyeData.timestamp;
//                 newData.LeftEye.isDataValid = _eyeData.verbose_data.right.GetValidity(SingleEyeDataValidity.SINGLE_EYE_DATA_GAZE_DIRECTION_VALIDITY);
//                 if(newData.RightEye.isDataValid)
//                 {
//                     newData.RightEye.Direction = _eyeData.verbose_data.right.gaze_direction_normalized;
//                     newData.RightEye.Direction.x *= -1;
//                     newData.RightEye.Origin = _eyeData.verbose_data.right.gaze_origin_mm * 0.001f;
//                     newData.RightEye.Origin.x *= -1;
//                 }
//                 Debug.Log("callback2");
//                 //
//                 return newData;
//             }

//             public override Transform GetOriginTransform() 
//             { 
//                 return Camera.main.transform;
//             }

//             public override void Destroy() 
//             { 
//                 SRanipal_Eye.WrapperUnRegisterEyeDataCallback(Marshal.GetFunctionPointerForDelegate((SRanipal_Eye.CallbackBasic)EyeCallback));
//                 eye_callback_registered = false;
//                 OnNewViveData -= HandleCallbackData;
//             }

//             /// <summary>
//             /// Required class for IL2CPP scripting backend support
//             /// </summary>
//             internal class MonoPInvokeCallbackAttribute : System.Attribute
//             {
//                 public MonoPInvokeCallbackAttribute() { }
//             }

//             //Note: Unity is not thread-safe and cannot call any UnityEngine api from within callback thread.
//             [MonoPInvokeCallback]
//             private static void EyeCallback(ref ViveSR.anipal.Eye.EyeData eye_data)
//             {
//                 Debug.Log("callback");
//                 _eyeData = eye_data;
//                 if(OnNewViveData != null)
//                 {
//                     print("Hej");
//                     OnNewViveData();
//                 }
//             }

//         #else

//             public override bool Initialize()
//             {
//                 Debug.LogError("Could not initialize SRanipal Eye Tracker.");
//                 return false;
//             }
//         #endif
        
//     }
// }