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
        Uniform,
        Gaussian
    }

    public class InjectorManager : MonoBehaviour
    {
        public bool isActive = true;
        public KeyCode toggleKey = KeyCode.None;

        public EyeTrackerList EyeTrackerSDK;
        public ErrorMode gazeMode;
        [Header("Gaze")]
        public GazeErrorSettings gazeSettings;
        private InjectorContainer _gazeInjectors;

        [Header("Right Eye")]
        public GazeErrorSettings rightEyeSettings;
        private InjectorContainer _rightEyeInjectors;

        [Header("Left Eye")]
        public GazeErrorSettings leftEyeSettings;
        private InjectorContainer _leftEyeInjectors;

        private Dictionary<GazeErrorSettings, InjectorContainer> injectors = new Dictionary<GazeErrorSettings, InjectorContainer>();
        private string _compilerFlagString;
        private string _eyeTrackerName;
        private EyeTracker _eyeTracker;

        public EyeTracker EyeTracker
        {
            get
            {
                return _eyeTracker;
            }
        }

        public delegate void NewErrorData(GazeErrorData data);
        public event NewErrorData OnNewErrorData;

        private GazeErrorData _latestErrorData;
        public GazeErrorData LatestErrorData 
        {
            get
            {
                return _latestErrorData;
            }
            private set
            {
                _latestErrorData = value;
                if(OnNewErrorData != null)
                {
                    OnNewErrorData(_latestErrorData);
                }
            }
        }

        
        // Start is called before the first frame update
        void Start()
        {
            injectors.Add(gazeSettings, AddComponents("Gaze", gazeSettings));
            injectors.Add(rightEyeSettings, AddComponents("Right Eye", rightEyeSettings));
            injectors.Add(leftEyeSettings, AddComponents("Left Eye", leftEyeSettings));
        }

        // Update is called once per frame
        void Update()
        {
            //TODO: EXPAND TO UNITY EVENT
            if(Input.GetKeyDown(toggleKey))
            {
                ToggleErrors();
            }

            if(!isActive) return;
        }

        public void ToggleErrors()
        {
            isActive = !isActive;
        }

        private void OnEyeTrackerData (GazeErrorData data)
        {
            if(isActive)
            {
                LatestErrorData = AddError(data);
            }
        }

        private GazeErrorData AddError(GazeErrorData data)
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
            return data;
        }


        private void AddNoError(GazeErrorData data)
        {

        }

        private void AddDependentError(GazeErrorData data)
        {
            //Left Eye
            
            // Right Eye

            // Cyclopean Eye
        }

        private void AddIndependentError(GazeErrorData data)
        {
            //Left Eye
            
            // Right Eye

            // Cyclopean Eye
        }

        private void AddError()
        {
            
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

        private void UpdateErrorSettings(GazeErrorSettings settings)
        {
            InjectorContainer container = injectors[settings];
            UpdateErrorSettings(container, settings);
        }

        private void UpdateErrorSettings(InjectorContainer container, GazeErrorSettings settings)
        {
            container.accuracy.AccuracyAmplitude = settings.gazeAccuracyError;
            container.accuracy.AccuracyDirection = settings.gazeAccuracyErrorDirection;
            container.gaussian.error = settings.precisionError;
            container.uniform.error = settings.precisionError;
            container.dataLoss.dataLossProbability = settings.dataLossProbability;
        }

        private InjectorContainer AddComponents(string name, GazeErrorSettings settings)
        {
            InjectorContainer container = new InjectorContainer();

            GameObject go = new GameObject(name);//, typeof(AccuracyInjector), typeof(GaussianPrecisionInjector), typeof(UniformPrecisionInjector), typeof(DataLossInjector));
            container.accuracy = go.AddComponent<AccuracyInjector>();
            container.gaussian = go.AddComponent<GaussianPrecisionInjector>();
            container.uniform = go.AddComponent<UniformPrecisionInjector>();
            container.dataLoss = go.AddComponent<DataLossInjector>();

            go.transform.parent = this.transform;
            container.obj = go;
            UpdateErrorSettings(container, settings);
            return container;
        }

        private class InjectorContainer
        {
            public GameObject obj;
            public AccuracyInjector accuracy;
            public GaussianPrecisionInjector gaussian;
            public UniformPrecisionInjector uniform;
            public DataLossInjector dataLoss; 
        }
    }
}