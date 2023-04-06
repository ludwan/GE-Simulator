using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

namespace GazeErrorSimulator
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

    public class ErrorSimulator : MonoBehaviour
    {
        public bool isActive = true;
        public KeyCode toggleKey = KeyCode.None;
        public EyeTrackerList EyeTrackerSDK;
        public int samepleRate = 120;
        public ErrorMode gazeMode;

        public GazeErrorSettings gazeSettings;
        public GazeErrorSettings leftEyeSettings;
        public GazeErrorSettings rightEyeSettings;

        private Dictionary<GazeErrorSettings, SimulatorContainer> simulators = new Dictionary<GazeErrorSettings, SimulatorContainer>();
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
            get { return _latestErrorData; }
            private set
            {
                _latestErrorData = value;
                OnNewErrorData?.Invoke(_latestErrorData);
            }
        }


        // Start is called before the first frame update
        void OnEnable()
        {
            Debug.Log("Enabling");
            InitEyeTracker();
            Time.fixedDeltaTime = (float)1 / samepleRate;
            if (_eyeTracker != null)
            {
                // SubscribeToGaze();
                simulators.Add(gazeSettings, AddComponents("Gaze", gazeSettings));
                simulators.Add(rightEyeSettings, AddComponents("Right Eye", rightEyeSettings));
                simulators.Add(leftEyeSettings, AddComponents("Left Eye", leftEyeSettings));
            }
            else
            {
                isActive = false;
            }
        }

        void FixedUpdate()
        {
            if (!isActive) return;
            GazeErrorData data = _eyeTracker.GetGazeData();
            LatestErrorData = AddError(data);
        }

        // Update is called once per frame
        void Update()
        {
            //TODO: EXPAND TO UNITY EVENT
            if (Input.GetKeyDown(toggleKey))
            {
                ToggleErrors();
            }

            if (!isActive) return;
            UpdateErrorSettings(gazeSettings);
            UpdateErrorSettings(rightEyeSettings);
            UpdateErrorSettings(leftEyeSettings);
        }

        public void ToggleErrors()
        {
            isActive = !isActive;
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

            data.Gaze.ErrorDirection = data.Gaze.Direction;
            data.Gaze.isErrorDataValid = data.Gaze.isDataValid;

            data.LeftEye.ErrorDirection = data.LeftEye.Direction;
            data.LeftEye.isErrorDataValid = data.LeftEye.isDataValid;

            data.RightEye.ErrorDirection = data.RightEye.Direction;
            data.RightEye.isErrorDataValid = data.RightEye.isDataValid;
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

            if (data.LeftEye.isErrorDataValid && data.RightEye.isErrorDataValid)
            {
                data.Gaze.ErrorDirection = (data.LeftEye.ErrorDirection + data.RightEye.ErrorDirection) / 2f;
                data.Gaze.isErrorDataValid = false;
            }
            else if (data.LeftEye.isErrorDataValid && !data.RightEye.isErrorDataValid)
            {
                data.Gaze.ErrorDirection = data.LeftEye.ErrorDirection;
                data.Gaze.isErrorDataValid = false;
            }
            else if (!data.LeftEye.isErrorDataValid && data.RightEye.isErrorDataValid)
            {
                data.Gaze.ErrorDirection = data.RightEye.ErrorDirection;
                data.Gaze.isErrorDataValid = false;
            }
            else if (!data.LeftEye.isErrorDataValid && !data.RightEye.isErrorDataValid)
            {
                data.Gaze.ErrorDirection = Vector3.zero;
                data.Gaze.isErrorDataValid = true;
            }
            return data;
        }

        private GazeErrorData AddIndependentError(GazeErrorData data)
        {
            if (data == null)
                return data;
                
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

            if(!data.isDataValid)
            {
                data.ErrorDirection = data.Direction;
                data.isErrorDataValid = data.isDataValid;
            }
            else
            {
                Vector3 errorDirection = AddError(data.Direction, settings);
                data.ErrorDirection = errorDirection;

                if(errorDirection == Vector3.zero)
                {
                    
                    data.isErrorDataValid = false;
                }
                else
                {
                    data.isErrorDataValid = true;
                }
            }        
            return data;
        }

        private Vector3 AddError(Vector3 dir, GazeErrorSettings settings)
        {
            SimulatorContainer container = simulators[settings];

            dir = container.dataLoss.Inject(dir);

            if (dir == Vector3.zero)
            {
                return dir;
            }
            
            dir = container.accuracy.Inject(dir);
            switch (settings.precisionErrorMode)
            {
                case PrecisionErrorMode.Uniform:
                    dir = container.uniform.Inject(dir);
                    break;
                case PrecisionErrorMode.Gaussian:
                    dir = container.uniform.Inject(dir);
                    break;
            }
            return dir;
        }

        private void InitEyeTracker()
        {
            if (_eyeTracker != null) return;

            Debug.Log("Initializing Eye Tracker: " + EyeTrackerSDK);

            UpdateEyeTracker();

            _eyeTracker = GetEyeTracker();
            if (_eyeTracker != null)
            {
                _eyeTracker.Initialize();
            }
        }

        private void UpdateEyeTracker()
        {
            _eyeTrackerName = SimulatorConstants.GetEyeTrackerName(EyeTrackerSDK);
            _compilerFlagString = SimulatorConstants.GetEyeTrackerCompilerFlag(EyeTrackerSDK);
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
                return this.gameObject.AddComponent(eyeTrackerType) as EyeTracker;
            }
            catch (Exception)
            {
                Debug.LogError("There was an error instantiating the Eye Tracker: " + eyeTrackerName);
            }
            return null;
        }

        private void UpdateErrorSettings(GazeErrorSettings settings)
        {
            SimulatorContainer container = simulators[settings];
            UpdateErrorSettings(container, settings);
        }

        private void UpdateErrorSettings(SimulatorContainer container, GazeErrorSettings settings)
        {
            container.accuracy.AccuracyAmplitude = settings.gazeAccuracyError;
            container.accuracy.AccuracyDirection = settings.gazeAccuracyErrorDirection;
            container.gaussian.error = settings.precisionError;
            container.uniform.error = settings.precisionError;
            container.dataLoss.dataLossProbability = settings.dataLossProbability;
        }

        private SimulatorContainer AddComponents(string name, GazeErrorSettings settings)
        {
            SimulatorContainer container = new SimulatorContainer();

            GameObject go = new GameObject(name);
            container.accuracy = go.AddComponent<AccuracySimulator>();
            container.gaussian = go.AddComponent<GaussianPrecisionSimulator>();
            container.uniform = go.AddComponent<UniformPrecisionSimulator>();
            container.dataLoss = go.AddComponent<DataLossSimulator>();

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


        private class SimulatorContainer
        {
            public GameObject obj;
            public AccuracySimulator accuracy;
            public GaussianPrecisionSimulator gaussian;
            public UniformPrecisionSimulator uniform;
            public DataLossSimulator dataLoss;
        }
    }
}