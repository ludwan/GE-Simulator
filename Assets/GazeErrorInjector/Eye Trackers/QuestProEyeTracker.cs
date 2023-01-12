using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if QUESTPRO_SDK

#endif

namespace GazeErrorInjector
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
            // SetupDebugObjects();

            return true;
        }

        void Update()
        {
            LatestData = GetGazeData();
            // UpdateDebugObjects();
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

        public override void Destroy() { }
        
        /* private GameObject debugLeftEyeGaze;
        private GameObject debugRightEyeGaze;
        private GameObject debugEyeGaze;

        private void SetupDebugObjects()
        {
            // Instantiate a debug object to visualize the eye gaze
            debugLeftEyeGaze = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            debugLeftEyeGaze.transform.localScale = Vector3.one * 0.01f;
            debugLeftEyeGaze.GetComponent<MeshRenderer>().material.color = Color.red;

            debugRightEyeGaze = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            debugRightEyeGaze.transform.localScale = Vector3.one * 0.01f;
            debugRightEyeGaze.GetComponent<MeshRenderer>().material.color = Color.blue;

            debugEyeGaze = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            debugEyeGaze.transform.localScale = Vector3.one * 0.01f;
            debugEyeGaze.GetComponent<MeshRenderer>().material.color = Color.green;
        }

        private void UpdateDebugObjects()
        {
            // Set debug object position to visualize gaze
            debugLeftEyeGaze.transform.position = LatestData.LeftEye.Origin + LatestData.LeftEye.Direction * 1.0f;
            debugRightEyeGaze.transform.position = LatestData.RightEye.Origin + LatestData.RightEye.Direction * 1.0f;
            debugEyeGaze.transform.position = LatestData.Gaze.Origin + LatestData.Gaze.Direction * 1.0f;
        } */

#else
        //TODO A LOT OF CODE REPETITION WITHIN THIS PART.
        public override bool Initialize()
        {
            Debug.LogError("Could not initialize HoloLens 2 Eye Tracker.");
            return false;
        }

#endif
    }
}