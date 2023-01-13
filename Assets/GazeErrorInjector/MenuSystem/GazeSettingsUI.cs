using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GazeErrorInjector
{
    public class GazeSettingsUI : MonoBehaviour
    {
        [SerializeField] private Slider gazeAccuracyErrorDirectionSlider;
        [SerializeField] private TextMeshProUGUI gazeAccuracyErrorDirectionValueLabel;

        [SerializeField] private Slider gazeAccuracyErrorSlider;
        [SerializeField] private TextMeshProUGUI gazeAccuracyErrorValueLabel;

        [SerializeField] private TMP_Dropdown precisionErrorModeDropdown;

        [SerializeField] private Slider precisionErrorSlider;
        [SerializeField] private TextMeshProUGUI precisionErrorValueLabel;

        [SerializeField] private Slider dataLossProbabilitySlider;
        [SerializeField] private TextMeshProUGUI dataLossProbabilityValueLabel;

        private GazeErrorSettings gazeErrorSettings;
        public GazeErrorSettings GazeErrorSettings    
        {
            get { return gazeErrorSettings; }
            set
            {
                gazeErrorSettings = value;
                gazeAccuracyErrorDirectionSlider.value = value.gazeAccuracyErrorDirection;
                gazeAccuracyErrorDirectionValueLabel.text = value.gazeAccuracyErrorDirection.ToString();
                gazeAccuracyErrorSlider.value = value.gazeAccuracyError;
                gazeAccuracyErrorValueLabel.text = value.gazeAccuracyError.ToString("0.00");
                precisionErrorModeDropdown.value = (int)value.precisionErrorMode;
                precisionErrorSlider.value = value.precisionError;
                precisionErrorValueLabel.text = value.precisionError.ToString("0.00");
                dataLossProbabilitySlider.value = value.dataLossProbability;
                dataLossProbabilityValueLabel.text = value.dataLossProbability.ToString("0.00");
            }
        }

        void Start()
        {
            if (gazeAccuracyErrorDirectionSlider != null)
                gazeAccuracyErrorDirectionSlider.onValueChanged.AddListener((value) =>
                {
                    int v = Mathf.RoundToInt(value);
                    gazeErrorSettings.gazeAccuracyErrorDirection = v;
                    gazeAccuracyErrorDirectionValueLabel.text = v.ToString();
                });

            if (gazeAccuracyErrorSlider != null)
                gazeAccuracyErrorSlider.onValueChanged.AddListener((value) =>
                {
                    gazeErrorSettings.gazeAccuracyError = value;
                    gazeAccuracyErrorValueLabel.text = value.ToString("0.00");
                });

            if (precisionErrorModeDropdown != null)
                precisionErrorModeDropdown.onValueChanged.AddListener((value) =>
                {
                    gazeErrorSettings.precisionErrorMode = (PrecisionErrorMode)System.Enum.ToObject(typeof(PrecisionErrorMode), value);
                });

            if (precisionErrorSlider != null)
                precisionErrorSlider.onValueChanged.AddListener((value) =>
                {
                    gazeErrorSettings.precisionError = value;
                    precisionErrorValueLabel.text = value.ToString("0.00");
                });

            if (dataLossProbabilitySlider != null)
                dataLossProbabilitySlider.onValueChanged.AddListener((value) =>
                {
                    gazeErrorSettings.dataLossProbability = value;
                    dataLossProbabilityValueLabel.text = value.ToString("0.00");
                });
        }

        void Update()
        {

        }
    }
}