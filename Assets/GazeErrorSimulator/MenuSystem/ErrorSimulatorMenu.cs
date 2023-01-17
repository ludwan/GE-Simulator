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
        [SerializeField] private ErrorSimulator errorSimulator;

        [Header("Menu Settings")]
        public string Title = "Gaze Error Simulator";
        [SerializeField] private TextMeshProUGUI titleObject;
        [SerializeField] private Toggle isActiveToggle;
        [SerializeField] private TMP_Dropdown gazeModeDropdown;

        [Header("Gaze Settings")]
        [SerializeField] private GazeSettingsUI gazeSettingsUI;
        private float gazeSettingsUIHeight = 145;
        [SerializeField] private GazeSettingsUI leftEyeSettingsUI;
        private float leftEyeSettingsUIHeight = 145;
        [SerializeField] private GazeSettingsUI rightEyeSettingsUI;
        private float rightEyeSettingsUIHeight = 145;

        void Start()
        {
            if (errorSimulator == null)
                errorSimulator = FindObjectOfType<ErrorSimulator>();

            SetupGazeSettingsUI();

            if (isActiveToggle != null)
                isActiveToggle.onValueChanged.AddListener((value) =>
                {
                    errorSimulator.isActive = value;
                });

            if (gazeModeDropdown != null)
                gazeModeDropdown.onValueChanged.AddListener((value) =>
                {
                    errorSimulator.gazeMode = (ErrorMode)System.Enum.ToObject(typeof(ErrorMode), value);
                    UpdateGazeErrorMode(errorSimulator.gazeMode);
                });
        }

        void Update()
        {
            if (titleObject != null)
                titleObject.text = Title;

            if (Application.isEditor == false || Application.isPlaying)
                return;

            isActiveToggle.isOn = errorSimulator.isActive;
            gazeModeDropdown.value = (int)errorSimulator.gazeMode;
        }

        void UpdateGazeErrorMode(ErrorMode value)
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

        void SetGazeSettingsUIVisibility(GazeSettingsUI gazeSettingsUI, bool isVisible, float defaultHeight)
        {
            if (gazeSettingsUI == null)
                return;

            gazeSettingsUI.transform.parent.gameObject.SetActive(isVisible);
        }

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
    }

}