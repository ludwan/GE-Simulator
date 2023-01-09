using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

namespace GazeErrorInjector
{
    public enum GazeMode
    {
        Independent,
        Dependent
    }

    public enum PrecisionErrorMode
    {
        Magnification,
        Gaussian
    }

    public class InjectorManager : MonoBehaviour
    {
        public bool isActive = true;
        public KeyCode toggleKey = KeyCode.None;

        public EyeTrackerList EyeTrackerSDK;


        [Header("Gaze")]
        public GazeMode gazeMode;

        public GazeErrorSettings cyclopeanSettings;
        // [Range(0f, 360f)] public float gazeAccuracyErrorDirection;
        // public float gazeAccuracyError;
        // public PrecisionErrorMode precisionErrorMode;
        // public float precisionError;
        // public float dataLossProbability;

        [Header("Right Eye")]
        public GazeErrorSettings rightEyeSettings;
        // [Range(0f, 360f)] public float rightAccuracyErrorDirection;
        // public float rightAccuracyError;
        // public PrecisionErrorMode rightPrecisionErrorMode;
        // [Range(0f, 10f)] public float rightPrecisionError;
        // [Range(0f, 1f)] public float rightDataLossProbability;

        [Header("Left Eye")]
        public GazeErrorSettings leftEyeSettings;
        // [Range(0f, 360f)] public float leftAccuracyErrorDirection;
        // public float leftAccuracyError;
        // public PrecisionErrorMode leftPrecisionErrorMode;
        // [Range(0f, 10f)] public float leftPrecisionError;
        // [Range(0f, 1f)] public float leftDataLossProbability;


        private string _compilerFlagString;
        private string _eyeTrackerName;
        private GazeErrorData _gazeData;
        private EyeTracker _eyeTracker;

        //
        public event Action OnNewGazeData;

        
        // Start is called before the first frame update
        void Start()
        {
            AddComponents("Cyclopean Eye");
            AddComponents("Right Eye");
            AddComponents("Left Eye");
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyDown(toggleKey))
            {
                ToggleErrorInjection();
            }

            if(!isActive) return;
        }

        public void ToggleErrorInjection()
        {
            isActive = !isActive;
        }

        private void SubscribeToGaze()
        {
        
        }

        private void InitEyeTracker()
        {
            if (_eyeTracker != null) return;

            Debug.Log("Initializing Eye Tracker: " + EyeTrackerSDK);

            UpdateEyeTracker();

            if (_compilerFlagString != null)
            {
                PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, _compilerFlagString);
            }

            _eyeTracker = GetEyeTracker();
            _eyeTracker.Initialize();
        }

        private void UpdateEyeTracker()
        {
            _eyeTrackerName = GazeErrorInjectorConstants.GetEyeTrackerName(EyeTrackerSDK);
            _compilerFlagString = GazeErrorInjectorConstants.GetEyeTrackerCompilerFlag(EyeTrackerSDK);
        }

        private EyeTracker GetEyeTracker()
        {
            return GetEyeTracker(_eyeTrackerName);
        }

        private EyeTracker GetEyeTracker(string eyeTrackerName)
        {
            Type eyeTrackerType = Type.GetType(eyeTrackerName);
            if (eyeTrackerType == null)
            {
                Debug.Log("Eye Tracker type not found");
                return null;
            } 
            try
            {
                return Activator.CreateInstance(eyeTrackerType) as EyeTracker;
            }
            catch (Exception) 
            {
                Debug.LogError("There was an error instantiating the Eye Tracker: " + eyeTrackerName);
            }
            return null;
        }

        private GameObject AddComponents(string name)
        {
            GameObject go = new GameObject(name, typeof(AccuracyInjector), typeof(GaussianPrecisionInjector), typeof(UniformPrecisionInjector), typeof(DataLossInjector));
            go.transform.parent = this.transform;
            return go;
        }
    }
}

