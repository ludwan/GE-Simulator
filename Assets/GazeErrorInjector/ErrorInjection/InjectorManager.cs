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

        public GazeErrorSettings gazeSettings;
        public GazeErrorSettings leftEyeSettings;
        public GazeErrorSettings rightEyeSettings;

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
        void OnEnable()
        {
            Debug.Log("Enabling");
            InitEyeTracker();
            if(_eyeTracker != null)
            {
                SubscribeToGaze();
                injectors.Add(gazeSettings, AddComponents("Gaze", gazeSettings));
                injectors.Add(rightEyeSettings, AddComponents("Right Eye", rightEyeSettings));
                injectors.Add(leftEyeSettings, AddComponents("Left Eye", leftEyeSettings));
            }
            else
            {
                isActive = false;
            }
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

        void OnApplicationQuit() 
        {
            if(_eyeTracker != null)
            {
                UnsubscribeToGaze();
            }
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
                    AddNoError(data);
                    break;
                case ErrorMode.Dependent:
                    AddDependentError(data);
                    break;
                case ErrorMode.Independent:
                    data = AddIndependentError(data);
                    break;
                default:
                    break;
            }
            return data;
        }


        private GazeErrorData AddNoError(GazeErrorData data)
        {
            data.Mode = ErrorMode.None;
            return data;
        }

        private GazeErrorData AddDependentError(GazeErrorData data)
        {
            data.Mode = ErrorMode.Dependent;
            //Left Eye
            data.LeftEye = AddErrorData(data.LeftEye, leftEyeSettings);
            // Right Eye
            data.RightEye = AddErrorData(data.RightEye, rightEyeSettings);
            // Cyclopean Eye

            if(!data.LeftEye.ErrorDataLoss && !data.RightEye.ErrorDataLoss)
            {
                data.Gaze.ErrorDirection = (data.LeftEye.ErrorDirection + data.RightEye.ErrorDirection) / 2f;
                data.Gaze.ErrorDataLoss = false;
            }
            else if (!data.LeftEye.ErrorDataLoss && data.LeftEye.ErrorDataLoss)
            {
                data.Gaze.ErrorDirection = data.LeftEye.ErrorDirection;
                data.Gaze.ErrorDataLoss = false;
            }
            else if (data.LeftEye.ErrorDataLoss && !data.LeftEye.ErrorDataLoss)
            {
                data.Gaze.ErrorDirection = data.RightEye.ErrorDirection;
                data.Gaze.ErrorDataLoss = false;
            }
            else if (data.LeftEye.ErrorDataLoss && data.LeftEye.ErrorDataLoss)
            {
                data.Gaze.ErrorDirection = Vector3.zero;
                data.Gaze.ErrorDataLoss = true;
            }
            return data;
        }

        private GazeErrorData AddIndependentError(GazeErrorData data)
        {
            data.Mode = ErrorMode.Independent;
            //Left Eye
            data.LeftEye = AddErrorData(data.LeftEye, leftEyeSettings);
            
            // Right Eye
            data.RightEye = AddErrorData(data.RightEye, rightEyeSettings);

            // Cyclopean Eye
            data.Gaze = AddErrorData(data.Gaze, gazeSettings);

            return data;
        }

        private EyeErrorData AddErrorData(EyeErrorData data, GazeErrorSettings settings)
        {
            data.AccuracyError = settings.gazeAccuracyError;
            data.AccuracyErrorDirection = settings.gazeAccuracyErrorDirection;
            data.PrecisionError = settings.precisionError;
            data.PrecisionMode = settings.precisionErrorMode;
            data.DataLossProbability = settings.dataLossProbability;

            Vector3 errorDirection = AddError(data.Direction, settings);
            data.ErrorDirection = errorDirection;

            if(errorDirection == Vector3.zero)
            {
                data.ErrorDataLoss = true;
            }
            else
            {
                data.ErrorDataLoss = false;
            }
            return data;
        }

        private Vector3 AddError(Vector3 dir, GazeErrorSettings settings)
        {
            InjectorContainer container = injectors[settings];

            dir = container.accuracy.Inject(dir);
            switch(settings.precisionErrorMode)
            {
                case PrecisionErrorMode.Uniform:
                    dir = container.uniform.Inject(dir);
                    break;
                case PrecisionErrorMode.Gaussian:
                    dir = container.uniform.Inject(dir);
                    break;
            }

            dir = container.dataLoss.Inject(dir);
            return dir;
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

            // if (_compilerFlagString != null)
            // {
            //     PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, _compilerFlagString);
            // }

            _eyeTracker = GetEyeTracker();
            if(_eyeTracker != null)
            {
                _eyeTracker.Initialize();
            }   
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
                print($"Name: {eyeTrackerName}");
                print(PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone));
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

            GameObject go = new GameObject(name);
            container.accuracy = go.AddComponent<AccuracyInjector>();
            container.gaussian = go.AddComponent<GaussianPrecisionInjector>();
            container.uniform = go.AddComponent<UniformPrecisionInjector>();
            container.dataLoss = go.AddComponent<DataLossInjector>();

            container.accuracy.Manager = this;
            container.gaussian.Manager = this;
            container.uniform.Manager = this;
            container.dataLoss.Manager = this;

            container.accuracy.Init();
            container.gaussian.Init();
            container.uniform.Init();
            container.dataLoss.Init();

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