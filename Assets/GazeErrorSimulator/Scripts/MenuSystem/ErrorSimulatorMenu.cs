using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GazeErrorSimulator
{
    [ExecuteInEditMode]
    public class ErrorSimulatorMenu : MonoBehaviour
    {
        [SerializeField]
        private ErrorSimulator errorSimulator;

        [Header("Menu Settings")]
        [Tooltip("The UI title of the menu")]
        public string Title = "Gaze Error Simulator";
        [SerializeField, Tooltip("The UI label for the menu title")]
        private TextMeshProUGUI titleObject;
        [SerializeField, Tooltip("The UI toggle for the menu active state")]
        private Toggle isActiveToggle;
        [SerializeField, Tooltip("The UI dropdown for the gaze error mode")]
        private TMP_Dropdown gazeModeDropdown;

        [Header("Gaze Settings")]
        [SerializeField, Tooltip("The UI settings for the gaze error")]
        private GazeSettingsUI gazeSettingsUI;
        private float gazeSettingsUIHeight = 145;
        [SerializeField, Tooltip("The UI settings for the left eye error")]
        private GazeSettingsUI leftEyeSettingsUI;
        private float leftEyeSettingsUIHeight = 145;
        [SerializeField, Tooltip("The UI settings for the right eye error")]
        private GazeSettingsUI rightEyeSettingsUI;
        private float rightEyeSettingsUIHeight = 145;

        void Start()
        {
            if (errorSimulator == null)
                errorSimulator = FindObjectOfType<ErrorSimulator>();
            if (errorSimulator == null)
            {
                Debug.LogError($"ErrorSimulatorMenu: No ErrorSimulator found in the scene. Please add an ErrorSimulator to the scene.");
                this.enabled = false;
                return;
            }

            SetupGazeSettingsUI();
            SetupIsActiveToggle();
            SetupGazeModeDropdown();
        }

        void Update()
        {
            if (titleObject != null)
                titleObject.text = Title;

            isActiveToggle.isOn = errorSimulator.isActive;
            gazeModeDropdown.value = (int)errorSimulator.gazeMode;
        }

        /// <summary>
        /// Setup the gaze settings UI.
        /// Get the height of the gaze settings UI objects and
        /// set the gaze error settings of the gaze settings UI objects.
        /// </summary>
        private void SetupGazeSettingsUI()
        {
            if (gazeSettingsUI != null)
            {
                gazeSettingsUIHeight = gazeSettingsUI.transform.parent.GetComponent<RectTransform>().rect.height;
                gazeSettingsUI.GazeErrorSettings = errorSimulator.gazeSettings;
            }
            if (leftEyeSettingsUI != null)
            {
                leftEyeSettingsUIHeight = leftEyeSettingsUI.transform.parent.GetComponent<RectTransform>().rect.height;
                leftEyeSettingsUI.GazeErrorSettings = errorSimulator.leftEyeSettings;
            }
            if (rightEyeSettingsUI != null)
            {
                rightEyeSettingsUIHeight = rightEyeSettingsUI.transform.parent.GetComponent<RectTransform>().rect.height;
                rightEyeSettingsUI.GazeErrorSettings = errorSimulator.rightEyeSettings;
            }
        }

        /// <summary>
        /// Setup the active state toggle.
        /// When the value of the toggle changes,
        /// the active state of the error simulator is updated.
        /// </summary>
        private void SetupIsActiveToggle()
        {
            if (isActiveToggle != null)
                isActiveToggle.onValueChanged.AddListener((value) =>
                {
                    errorSimulator.isActive = value;
                });
        }

        /// <summary>
        /// Setup the gaze mode dropdown.
        /// When the value of the dropdown changes, 
        /// the gaze mode of the error simulator is updated.
        /// </summary>
        private void SetupGazeModeDropdown()
        {
            if (gazeModeDropdown == null)
                return;

            gazeModeDropdown.onValueChanged.AddListener((value) =>
            {
                errorSimulator.gazeMode = (ErrorMode)System.Enum.ToObject(typeof(ErrorMode), value);
                UpdateGazeErrorMode(errorSimulator.gazeMode);
            });
        }

        /// <summary>
        /// Make the desired gaze settings UI visible or invisible
        /// depending on the gaze error mode.
        /// </summary>
        /// <param name="value">The desired gaze error mode (dependent, independent, or none)</param>
        private void UpdateGazeErrorMode(ErrorMode value)
        {
            switch (errorSimulator.gazeMode)
            {
                case ErrorMode.Dependent:
                    SetGazeSettingsUIVisibility(gazeSettingsUI, false, gazeSettingsUIHeight);
                    SetGazeSettingsUIVisibility(leftEyeSettingsUI, true, leftEyeSettingsUIHeight);
                    SetGazeSettingsUIVisibility(rightEyeSettingsUI, true, rightEyeSettingsUIHeight);
                    break;
                case ErrorMode.Independent:
                    SetGazeSettingsUIVisibility(gazeSettingsUI, true, gazeSettingsUIHeight);
                    SetGazeSettingsUIVisibility(leftEyeSettingsUI, true, leftEyeSettingsUIHeight);
                    SetGazeSettingsUIVisibility(rightEyeSettingsUI, true, rightEyeSettingsUIHeight);
                    break;
                default:
                    SetGazeSettingsUIVisibility(gazeSettingsUI, false, gazeSettingsUIHeight);
                    SetGazeSettingsUIVisibility(leftEyeSettingsUI, false, leftEyeSettingsUIHeight);
                    SetGazeSettingsUIVisibility(rightEyeSettingsUI, false, rightEyeSettingsUIHeight);
                    break;
            }
        }

        /// <summary>
        /// Set the visibility of the gaze settings UI.
        /// </summary>
        /// <param name="gazeSettingsUI">The gaze settings UI to set the visibility of</param>
        /// <param name="isVisible">The desired visibility of the gaze settings UI</param>
        /// <param name="defaultHeight">The default height of the gaze settings UI</param>
        private void SetGazeSettingsUIVisibility(GazeSettingsUI gazeSettingsUI, bool isVisible, float defaultHeight)
        {
            if (gazeSettingsUI == null)
                return;

            gazeSettingsUI.transform.parent.gameObject.SetActive(isVisible);
        }
    }

}