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
        private OVREyeGaze[] eyes;

        public override bool Initialize()
        {
            eyes = FindObjectsOfType<OVREyeGaze>().OrderBy(eye => eye.Eye).ToArray();
            if (eyes.Length < 2)
                return false;

            return true;
        }

        public override GazeErrorData GetGazeData()
        {
            GazeErrorData gazeErrorData = new GazeErrorData();

            gazeErrorData.LeftEye.Timestamp = Time.unscaledTime;
            gazeErrorData.LeftEye.Origin = eyes[0].transform.position;
            gazeErrorData.LeftEye.Direction = eyes[0].transform.forward;
            gazeErrorData.LeftEye.isDataValid = eyes[0].EyeTrackingEnabled;

            gazeErrorData.RightEye.Timestamp = Time.unscaledTime;
            gazeErrorData.RightEye.Origin = eyes[1].transform.position;
            gazeErrorData.RightEye.Direction = eyes[1].transform.forward;
            gazeErrorData.RightEye.isDataValid = eyes[1].EyeTrackingEnabled;

            Vector3 centerEyePos = Vector3.zero;
            foreach (var eye in eyes)
                centerEyePos += eye.transform.position;
            centerEyePos /= eyes.Length;
            Quaternion centerEyeRot = Quaternion.Lerp(eyes[0].transform.rotation, eyes[1].transform.rotation, 0.5f);

            gazeErrorData.Gaze.Timestamp = Time.unscaledTime;
            gazeErrorData.Gaze.Origin = centerEyePos;
            gazeErrorData.Gaze.Direction = centerEyeRot * Vector3.forward;
            gazeErrorData.Gaze.isDataValid = eyes[0].EyeTrackingEnabled && eyes[1].EyeTrackingEnabled;

            _latestdata = gazeErrorData;

            return gazeErrorData;
        }

        public void GetOrigin()
        {
            throw new System.NotImplementedException();
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