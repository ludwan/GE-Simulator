using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GazeErrorInjector
{
    [ExecuteInEditMode]
    public class ErrorInjectorMenu : MonoBehaviour
    {
        [SerializeField] private InjectorManager injectorManager;

        [Header("Menu Settings")]
        public string Title = "Gaze Error Injector";
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
            if (injectorManager == null)
                injectorManager = FindObjectOfType<InjectorManager>();

            SetupGazeSettingsUI();

            if (isActiveToggle != null)
                isActiveToggle.onValueChanged.AddListener((value) =>
                {
                    injectorManager.isActive = value;
                });

            if (gazeModeDropdown != null)
                gazeModeDropdown.onValueChanged.AddListener((value) =>
                {
                    injectorManager.gazeMode = (ErrorMode)System.Enum.ToObject(typeof(ErrorMode), value);
                    UpdateGazeErrorMode(injectorManager.gazeMode);
                });
        }

        void Update()
        {
            if (titleObject != null)
                titleObject.text = Title;

            if (Application.isEditor == false || Application.isPlaying)
                return;

            isActiveToggle.isOn = injectorManager.isActive;
            gazeModeDropdown.value = (int)injectorManager.gazeMode;
        }

        void UpdateGazeErrorMode(ErrorMode value)
        {
            switch (injectorManager.gazeMode)
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

            gazeSettingsUI.gameObject.SetActive(isVisible);
            gazeSettingsUI.transform.parent.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, isVisible ? defaultHeight : 0);
        }

        private void SetupGazeSettingsUI()
        {
            if (gazeSettingsUI != null)
            {
                gazeSettingsUIHeight = gazeSettingsUI.transform.parent.GetComponent<RectTransform>().rect.height;
                gazeSettingsUI.GazeErrorSettings = injectorManager.gazeSettings;
            }
            if (leftEyeSettingsUI != null)
            {
                leftEyeSettingsUIHeight = leftEyeSettingsUI.transform.parent.GetComponent<RectTransform>().rect.height;
                leftEyeSettingsUI.GazeErrorSettings = injectorManager.leftEyeSettings;
            }
            if (rightEyeSettingsUI != null)
            {
                rightEyeSettingsUIHeight = rightEyeSettingsUI.transform.parent.GetComponent<RectTransform>().rect.height;
                rightEyeSettingsUI.GazeErrorSettings = injectorManager.rightEyeSettings;
            }
        }
    }

}