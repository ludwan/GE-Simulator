using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

namespace GazeErrorSimulator
{
    /// <summary>
    /// The mode of error for simulation.
    /// </summary>
    public enum ErrorMode
    {
        ///<summary>No errors are added to gaze.</summary>
        None,
        ///<summary>Error is defined separately for left eye, right eye, and gaze.</summary>
        Independent,
        ///<summary>Gaze error is based left eye and right eye errors defined by the user.</summary>
        Dependent,
    }

    /// <summary>
    /// The type of precision error.
    /// </summary>
    public enum PrecisionErrorMode
    {
        ///<summary>Uniform distribution (UniformPrecisionSimulator).</summary>
        Uniform,
        ///<summary>Gaussian distribution (UniformPrecisionSimulator).</summary>
        Gaussian
    }

    /// <summary>
    /// Main class that handles all the error simulation. 
    /// </summary>
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
            if (EyeTrackerSDK == EyeTrackerList.None) return;
            InitEyeTracker();
            Time.fixedDeltaTime = (float)1 / samepleRate;
            if (_eyeTracker != null)
            {
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
            if (!isActive || EyeTrackerSDK == EyeTrackerList.None) return;
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

            if (!isActive || EyeTrackerSDK == EyeTrackerList.None) return;
            UpdateErrorSettings(gazeSettings);
            UpdateErrorSettings(rightEyeSettings);
            UpdateErrorSettings(leftEyeSettings);
        }

        /// <summary>
        /// Toggle the error simulation on and off.
        /// </summary>
        public void ToggleErrors()
        {
            isActive = !isActive;
        }

        /// <summary>
        /// Add error to the gaze data based on the gaze mode.
        /// </summary>
        /// <param name="data">The current gaze data</param>
        /// <returns>The gaze data with error added</returns>
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

        /// <summary>
        /// Add no error to the gaze data meaning that the gaze data is not changed.
        /// </summary>
        /// <param name="data">The current gaze data</param>
        /// <returns>The gaze data with no error added</returns>
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

        /// <summary>
        /// Add dependent error to the gaze data meaning that the gaze error 
        /// is added to gaze data of the left and right gaze and the 
        /// (cyclopean) gaze set dependently.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Add independent error to the gaze data meaning that the gaze error 
        /// is added to gaze data of the left, right, and (cyclopean) gaze independently.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Add error to the gaze data based on the gaze error settings.
        /// </summary>
        /// <param name="data">The current gaze data</param>
        /// <param name="settings">The gaze error settings</param>
        /// <returns>The gaze data with error added</returns>
        private EyeErrorData AddErrorData(EyeErrorData data, GazeErrorSettings settings)
        {
            data.AccuracyError = settings.gazeAccuracyError;
            data.AccuracyErrorDirection = settings.gazeAccuracyErrorDirection;
            data.PrecisionError = settings.precisionError;
            data.PrecisionMode = settings.precisionErrorMode;
            data.DataLossProbability = settings.dataLossProbability;

            if (!data.isDataValid)
            {
                data.ErrorDirection = data.Direction;
                data.isErrorDataValid = data.isDataValid;
            }
            else
            {
                Vector3 errorDirection = AddError(data.Direction, settings);
                data.ErrorDirection = errorDirection;

                if (errorDirection == Vector3.zero)
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

        /// <summary>
        /// Add error to the gaze direction based on the gaze error settings.
        /// </summary>
        /// <param name="dir">The current gaze direction</param>
        /// <param name="settings">The gaze error settings</param>
        /// <returns>The gaze direction with error added</returns>
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

        /// <summary>
        /// Initialize the eye tracker based on the eye tracker SDK.
        /// </summary>
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

        /// <summary>
        /// Update the eye tracker variables based on the specified eye tracker SDK.
        /// </summary>
        private void UpdateEyeTracker()
        {
            _eyeTrackerName = SimulatorConstants.GetEyeTrackerName(EyeTrackerSDK);
            _compilerFlagString = SimulatorConstants.GetEyeTrackerCompilerFlag(EyeTrackerSDK);
        }

        /// <summary>
        /// Get the eye tracker based on the specified eye tracker SDK.
        /// </summary>
        /// <returns>The desired eye tracker</returns>
        private EyeTracker GetEyeTracker()
        {
            return GetEyeTracker(_eyeTrackerName);
        }

        /// <summary>
        /// Get the eye tracker based on the specified eye tracker name.
        /// </summary>
        /// <param name="eyeTrackerName">The name of the eye tracker</param>
        /// <returns>The desired eye tracker</returns>
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

        /// <summary>
        /// Update the error settings of the gaze error settings.
        /// </summary>
        /// <param name="settings">The gaze error settings</param>
        private void UpdateErrorSettings(GazeErrorSettings settings)
        {
            if (settings == null) return;
            SimulatorContainer container = simulators[settings];
            UpdateErrorSettings(container, settings);
        }

        /// <summary>
        /// Update the (left, right, gaze) error settings of the gaze error settings.
        /// </summary>
        /// <param name="container">The container of the gaze error settings, either left, right, or gaze</param>
        /// <param name="settings">The gaze error settings</param>
        private void UpdateErrorSettings(SimulatorContainer container, GazeErrorSettings settings)
        {
            container.accuracy.AccuracyAmplitude = settings.gazeAccuracyError;
            container.accuracy.AccuracyDirection = settings.gazeAccuracyErrorDirection;
            container.gaussian.error = settings.precisionError;
            container.uniform.error = settings.precisionError;
            container.dataLoss.dataLossProbability = settings.dataLossProbability;
        }

        /// <summary>
        /// Add error simulator components to a new game object using the specified 
        /// gaze error settings. Set the error simulator manager to this instance and
        /// initialize the error simulator components.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
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