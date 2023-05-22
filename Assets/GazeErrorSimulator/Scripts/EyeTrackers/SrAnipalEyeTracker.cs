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

        /// <summary>
        /// Initialize the SRanipal Eye Tracker.
        /// This method must be called before calling GetGazeData.
        /// </summary>
        /// <returns>True if it is a Vive Pro Eye and the eye tracker is working</returns>
        public override bool Initialize()
        {
            if (!SRanipal_Eye_API.IsViveProEye()) return false;
            origin = GetOriginTransform();

            return (SRanipal_Eye_Framework.Status == SRanipal_Eye_Framework.FrameworkStatus.WORKING);
        }
        
        /// <summary>
        /// Get the gaze data from the SRanipal Eye Tracker,
        /// including the left eye, right eye, and cyclopean gaze data.
        /// </summary>
        /// <returns>The gaze data from the SRanipal Eye Tracker</returns>
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

        /// <summary>
        /// Get the transform of the origin, in this case the main camera.
        /// </summary>
        /// <returns>The main camera transform</returns>
        public override Transform GetOriginTransform() 
        { 
            return Camera.main.transform;
        }
#else
        /// <summary>
        /// Default implementation of the Initialize method for the SRanipal Eye Tracker.
        /// Always returns false when the VIVE_SDK compile flag is not set.
        /// </summary>
        /// <returns>False</returns>
        public override bool Initialize()
        {
            Debug.LogError("Could not initialize SRanipal Eye Tracker.");
            return false;
        }
#endif
    }
}