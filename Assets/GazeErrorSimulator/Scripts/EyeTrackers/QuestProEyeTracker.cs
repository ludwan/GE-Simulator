using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if QUESTPRO_SDK
using static OVREyeGaze;
using static OVRPlugin;
#endif

namespace GazeErrorSimulator
{
    public class QuestProEyeTracker : EyeTracker
    {
        // The below code is only the QUESTPRO_SDK compile flag is set, 
        // which can be done through the ErrorSimulator component in the scene.
#if QUESTPRO_SDK 
        private OVRPlugin.EyeGazesState _currentEyeGazesState;

        /// <summary>
        /// Initialize the Quest Pro Eye Tracker.
        /// This method must be called before calling GetGazeData.
        /// </summary>
        /// <returns>True if eye tracking is enabled and the eye 
        /// tracker was started succesfully.</returns>
        public override bool Initialize()
        {
            bool isEyeTrackingEnabled = OVRPlugin.eyeTrackingEnabled;
            if (!isEyeTrackingEnabled)
            {
                Debug.LogError($"[{nameof(QuestProEyeTracker)}] Eye tracking is not enabled on this device.");
                return false;
            }
            bool eyeTrackingStartedSuccesfully = OVRPlugin.StartEyeTracking();
            if (!eyeTrackingStartedSuccesfully)
            {
                Debug.LogWarning($"[{nameof(QuestProEyeTracker)}] Failed to start eye tracking.");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Get the gaze data from the Quest Pro Eye Tracker,
        /// including the left eye, right eye, and cyclopean gaze data.
        /// </summary>
        /// <returns>The gaze data from the Quest Pro Eye Tracker</returns>
        public override GazeErrorData GetGazeData()
        {
            bool gotEyeGazeState = OVRPlugin.GetEyeGazesState(OVRPlugin.Step.Render, -1, ref _currentEyeGazesState);
            if (gotEyeGazeState == false)
            {
                Debug.Log($"[{nameof(QuestProEyeTracker)}] Failed to get eye gaze state.");
                return null;
            }

            GazeErrorData newData = new GazeErrorData();

            // Left Eye
            EyeGazeState leftGaze = _currentEyeGazesState.EyeGazes[(int)EyeId.Left];
            OVRPose leftGazePose = leftGaze.Pose.ToOVRPose();
            newData.LeftEye.Timestamp = System.Convert.ToSingle(_currentEyeGazesState.Time);
            newData.LeftEye.Origin = leftGazePose.position;
            newData.LeftEye.Direction = leftGazePose.orientation * Vector3.forward;
            newData.LeftEye.isDataValid = leftGaze.IsValid;

            // Right Eye
            EyeGazeState rightGaze = _currentEyeGazesState.EyeGazes[(int)EyeId.Right];
            OVRPose rightGazePose = rightGaze.Pose.ToOVRPose();
            newData.RightEye.Timestamp = System.Convert.ToSingle(_currentEyeGazesState.Time);
            newData.RightEye.Origin = rightGazePose.position;
            newData.RightEye.Direction = rightGazePose.orientation * Vector3.forward;
            newData.RightEye.isDataValid = rightGaze.IsValid;

            // Gaze (Cyclopean)
            newData.Gaze.Timestamp = System.Convert.ToSingle(_currentEyeGazesState.Time);
            newData.Gaze.Origin = (leftGazePose.position + rightGazePose.position) / 2.0f;
            newData.Gaze.Direction = Quaternion.Lerp(leftGazePose.orientation, rightGazePose.orientation, 0.5f) * Vector3.forward;
            newData.Gaze.isDataValid = leftGaze.IsValid && rightGaze.IsValid;

            return newData;
        }

        public override Transform GetOriginTransform()
        {
            return Camera.main.transform;
        }
#else
        /// <summary>
        /// Default implementation of the Initialize method for the Quest Pro Eye Tracker.
        /// Always returns false when the QUESTPRO_SDK compile flag is not set.
        /// </summary>
        /// <returns>False</returns>
        public override bool Initialize()
        {
            Debug.LogError("Could not initialize Meta Quest Pro Eye Tracker.");
            return false;
        }
#endif
    }
}