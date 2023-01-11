using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if QUESTPRO_SDK

#endif

namespace GazeErrorInjector
{
    public class QuestProEyeTracker : MonoBehaviour, IEyeTracker
    {

        private GazeErrorData _latestdata = new GazeErrorData();
        public GazeErrorData LatestData
        {
            get
            {
                return _latestdata;
            }
        }

#if QUESTPRO_SDK
        private OVREyeGaze[] eyes;

        public bool Initialize()
        {
            eyes = FindObjectsOfType<OVREyeGaze>().OrderBy(eye => eye.Eye).ToArray();
            if (eyes.Length < 2)
                return false;

            return true;
        }

        public GazeErrorData GetGazeData()
        {
            GazeErrorData gazeErrorData = new GazeErrorData();

            gazeErrorData.LeftEye.Timestamp = Time.unscaledTime;
            gazeErrorData.LeftEye.GazeOrigin = eyes[0].transform.position;
            gazeErrorData.LeftEye.GazeDirection = eyes[0].transform.forward;
            gazeErrorData.LeftEye.isDataValid = eyes[0].EyeTrackingEnabled;

            gazeErrorData.RightEye.Timestamp = Time.unscaledTime;
            gazeErrorData.RightEye.GazeOrigin = eyes[1].transform.position;
            gazeErrorData.RightEye.GazeDirection = eyes[1].transform.forward;
            gazeErrorData.RightEye.isDataValid = eyes[1].EyeTrackingEnabled;

            Vector3 centerEyePos = Vector3.zero;
            foreach (var eye in eyes)
                centerEyePos += eye.transform.position;
            centerEyePos /= eyes.Length;
            Quaternion centerEyeRot = Quaternion.Lerp(eyes[0].transform.rotation, eyes[1].transform.rotation, 0.5f);

            gazeErrorData.Gaze.Timestamp = Time.unscaledTime;
            gazeErrorData.Gaze.GazeOrigin = centerEyePos;
            gazeErrorData.Gaze.GazeDirection = centerEyeRot * Vector3.forward;
            gazeErrorData.Gaze.isDataValid = eyes[0].EyeTrackingEnabled && eyes[1].EyeTrackingEnabled;

            _latestdata = gazeErrorData;

            return gazeErrorData;
        }

        public void GetOrigin()
        {
            throw new System.NotImplementedException();
        }

        public Transform GetOriginTransform()
        {
            return Camera.main.transform;
        }

        public void Destroy() { }

#else
        //TODO A LOT OF CODE REPETITION WITHIN THIS PART.
        public bool Initialize()
        {
            Debug.LogError("Could not initialize HoloLens 2 Eye Tracker.");
            return false;
        }

        public GazeErrorData GetGazeData() { return null; }

        public Transform GetOriginTransform() { return null; }

        public void Destroy() { }
#endif
    }
}