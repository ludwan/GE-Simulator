using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if QUESTPRO_SDK

#endif

namespace GazeErrorSimulator
{
    public class QuestProEyeTracker : EyeTracker
    {
#if QUESTPRO_SDK
        private OVREyeGaze leftEye;
        private OVREyeGaze rightEye;

        private OVRCameraRig ovrCameraRig;

        public override bool Initialize()
        {
            ovrCameraRig = FindObjectOfType<OVRCameraRig>();
            if (ovrCameraRig == null)
                return false;

            SetupOVREyeGaze(OVREyeGaze.EyeId.Left, out leftEye);
            SetupOVREyeGaze(OVREyeGaze.EyeId.Right, out rightEye);

            return true;
        }

        public override GazeErrorData GetGazeData()
        {
            GazeErrorData newData = new GazeErrorData();

            // Left Eye
            newData.LeftEye.Timestamp = Time.unscaledTime;
            newData.LeftEye.Origin = leftEye.transform.position;
            newData.LeftEye.Direction = leftEye.transform.forward;
            newData.LeftEye.isDataValid = leftEye.EyeTrackingEnabled;

            // Right Eye
            newData.RightEye.Timestamp = Time.unscaledTime;
            newData.RightEye.Origin = rightEye.transform.position;
            newData.RightEye.Direction = rightEye.transform.forward;
            newData.RightEye.isDataValid = rightEye.EyeTrackingEnabled;

            // Gaze
            newData.Gaze.Timestamp = Time.unscaledTime;
            newData.Gaze.Origin = (leftEye.transform.position + rightEye.transform.position) / 2.0f;
            newData.Gaze.Direction = Quaternion.Lerp(leftEye.transform.rotation, rightEye.transform.rotation, 0.5f) * Vector3.forward;
            newData.Gaze.isDataValid = leftEye.EyeTrackingEnabled && rightEye.EyeTrackingEnabled;

            return newData;
        }

        public override Transform GetOriginTransform()
        {
            return Camera.main.transform;
        }

        private void SetupOVREyeGaze(OVREyeGaze.EyeId eye, out OVREyeGaze eyeGaze)
        {
            // Convert enum to string value
            GameObject eyeGO = new GameObject(System.Enum.GetName(typeof(OVREyeGaze.EyeId), eye) + "Eye");

            eyeGO.transform.parent = ovrCameraRig.trackingSpace;
            // Reset the transform
            eyeGO.transform.localPosition = Vector3.zero;
            eyeGO.transform.localRotation = Quaternion.identity;
            eyeGO.transform.localScale = Vector3.one;

            // Add the OVREyeGaze component
            eyeGaze = eyeGO.AddComponent<OVREyeGaze>();
            // Set the specified eye
            eyeGaze.Eye = eye;
            // Set the tracking mode to TrackingSpace to ensure working tracking
            eyeGaze.TrackingMode = OVREyeGaze.EyeTrackingMode.TrackingSpace;
        }
#endif
    }
}