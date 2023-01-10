using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

namespace GazeErrorInjector
{
    public enum ErrorMode
    {
        None,
        Independent,
        Dependent,
        
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
        public ErrorMode gazeMode;

        public GazeErrorSettings cyclopeanSettings;

        [Header("Right Eye")]
        public GazeErrorSettings rightEyeSettings;

        [Header("Left Eye")]
        public GazeErrorSettings leftEyeSettings;

        private string _compilerFlagString;
        private string _eyeTrackerName;
        private GazeErrorData _gazeData;
        private EyeTracker _eyeTracker;

        public EyeTracker EyeTracker
        {
            get
            {
                return _eyeTracker;
            }
        }

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

        private void OnEyeTrackerData (GazeErrorData data)
        {
            if(isActive)
            {

            }
        }

        private void InjectError(GazeErrorData data)
        {
            switch (gazeMode)
            {
                case ErrorMode.None:
                    break;
                case ErrorMode.Dependent:
                    break;
                case ErrorMode.Independent:
                    break;
                default:
                    break;
            }
        }


        private void InjectDependentError(GazeErrorData data)
        {
            //Left Eye
            
            // Right Eye

            // Cyclopean Eye
        }

        private void InjectIndependentError(GazeErrorData data)
        {
            //Left Eye
            
            // Right Eye

            // Cyclopean Eye
        }

        private void SubscribeToGaze()
        {
            _eyeTracker.OnNewGazeData += OnEyeTrackerData;
        }

        private void UnsubscribeToGaze()
        {
            _eyeTracker.OnNewGazeData -= OnEyeTrackerData;
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