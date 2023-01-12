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

        public override bool Initialize()
        {
            var eyes = FindObjectsOfType<OVREyeGaze>();

            if (eyes.Length != 2)
                return false;

            foreach (var eye in eyes)
            {
                if (eye.Eye == OVREyeGaze.EyeId.Left)
                    leftEye = eye;
                else if (eye.Eye == OVREyeGaze.EyeId.Right)
                    rightEye = eye;
            }

            return true;
        }

        void Update()
        {
            LatestData = GetGazeData();
        }

        public override GazeErrorData GetGazeData()
        {
            GazeErrorData gazeErrorData = new GazeErrorData();

            // Left Eye
            gazeErrorData.LeftEye.Timestamp = Time.unscaledTime;
            gazeErrorData.LeftEye.Origin = leftEye.transform.position;
            gazeErrorData.LeftEye.Direction = leftEye.transform.forward;
            gazeErrorData.LeftEye.isDataValid = leftEye.EyeTrackingEnabled;

            // Right Eye
            gazeErrorData.RightEye.Timestamp = Time.unscaledTime;
            gazeErrorData.RightEye.Origin = rightEye.transform.position;
            gazeErrorData.RightEye.Direction = rightEye.transform.forward;
            gazeErrorData.RightEye.isDataValid = rightEye.EyeTrackingEnabled;

            // Gaze
            gazeErrorData.Gaze.Timestamp = Time.unscaledTime;
            gazeErrorData.Gaze.Origin = (leftEye.transform.position + rightEye.transform.position) / 2.0f;
            gazeErrorData.Gaze.Direction = Quaternion.Lerp(leftEye.transform.rotation, rightEye.transform.rotation, 0.5f) * Vector3.forward;
            gazeErrorData.Gaze.isDataValid = leftEye.EyeTrackingEnabled && rightEye.EyeTrackingEnabled;

            return gazeErrorData;
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
            Debug.LogError("Could not initialize HoloLens 2 Eye Tracker.");
            return false;
        }

#endif
    }
}