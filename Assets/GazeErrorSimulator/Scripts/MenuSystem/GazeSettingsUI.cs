using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GazeErrorSimulator
{
    public class GazeSettingsUI : MonoBehaviour
    {
        [SerializeField, Tooltip("The UI slider for the gaze accuracy error direction")]
        private Slider gazeAccuracyErrorDirectionSlider;
        [SerializeField, Tooltip("The UI label for the gaze accuracy error direction value")]
        private TextMeshProUGUI gazeAccuracyErrorDirectionValueLabel;

        [SerializeField, Tooltip("The UI slider for the gaze accuracy error")]
        private Slider gazeAccuracyErrorSlider;
        [SerializeField, Tooltip("The UI label for the gaze accuracy error value")] 
        private TextMeshProUGUI gazeAccuracyErrorValueLabel;

        [SerializeField, Tooltip("The UI dropdown for the precision error mode")] 
        private TMP_Dropdown precisionErrorModeDropdown;

        [SerializeField, Tooltip("The UI slider for the precision error")] 
        private Slider precisionErrorSlider;
        [SerializeField, Tooltip("The UI label for the precision error value")] 
        private TextMeshProUGUI precisionErrorValueLabel;

        [SerializeField, Tooltip("The UI slider for the data loss probability")] 
        private Slider dataLossProbabilitySlider;
        [SerializeField, Tooltip("The UI label for the data loss probability value")] 
        private TextMeshProUGUI dataLossProbabilityValueLabel;

        private GazeErrorSettings gazeErrorSettings;
        public GazeErrorSettings GazeErrorSettings
        {
            get { return gazeErrorSettings; }
            set
            {
                // Set all the sliders, dropdown, and labels to the values of the gaze error settings
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
            SetupGazeAccErrDirSlider();
            SetupGazeAccErrSlider();
            SetupPrecErrModeDropdown();
            SetupPrecErrSlider();
            SetupDataLossPrbSlider();
        }

        /// <summary>
        /// Add a listener to the gaze accuracy error direction slider.
        /// When the slider value changes, the gaze accuracy error direction
        /// is updated to the value of the slider (rounded to int).
        /// </summary>
        private void SetupGazeAccErrDirSlider()
        {
            if (gazeAccuracyErrorDirectionSlider == null)
                return;

            gazeAccuracyErrorDirectionSlider.onValueChanged.AddListener((value) =>
            {
                int v = Mathf.RoundToInt(value);
                gazeErrorSettings.gazeAccuracyErrorDirection = v;
                gazeAccuracyErrorDirectionValueLabel.text = v.ToString();
            });
        }

        /// <summary>
        /// Add a listener to the gaze accuracy error slider.
        /// When the slider value changes, the gaze accuracy error
        /// is updated to the value of the slider.
        /// </summary>
        private void SetupGazeAccErrSlider()
        {
            if (gazeAccuracyErrorSlider != null)
            {
                gazeAccuracyErrorSlider.onValueChanged.AddListener((value) =>
                {
                    gazeErrorSettings.gazeAccuracyError = value;
                    gazeAccuracyErrorValueLabel.text = value.ToString("0.00");
                });
            }
        }

        /// <summary>
        /// Add a listener to the precision error mode dropdown.
        /// When the dropdown value changes, the precision error mode
        /// is updated to the value of the dropdown.
        /// </summary>
        private void SetupPrecErrModeDropdown()
        {
            if (precisionErrorModeDropdown != null)
            {
                precisionErrorModeDropdown.onValueChanged.AddListener((value) =>
                {
                    gazeErrorSettings.precisionErrorMode = (PrecisionErrorMode)System.Enum.ToObject(typeof(PrecisionErrorMode), value);
                });
            }
        }

        /// <summary>
        /// Add a listener to the precision error slider.
        /// When the slider value changes, the precision error
        /// is updated to the value of the slider.
        /// </summary>
        private void SetupPrecErrSlider()
        {
            if (precisionErrorSlider != null)
            {
                precisionErrorSlider.onValueChanged.AddListener((value) =>
                {
                    gazeErrorSettings.precisionError = value;
                    precisionErrorValueLabel.text = value.ToString("0.00");
                });
            }
        }

        /// <summary>
        /// Add a listener to the data loss probability slider.
        /// When the slider value changes, the data loss probability
        /// is updated to the value of the slider.
        /// </summary>
        private void SetupDataLossPrbSlider()
        {
            if (dataLossProbabilitySlider != null)
            {
                dataLossProbabilitySlider.onValueChanged.AddListener((value) =>
                {
                    gazeErrorSettings.dataLossProbability = value;
                    dataLossProbabilityValueLabel.text = value.ToString("0.00");
                });
            }
        }
    }
}