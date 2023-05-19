using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GazeErrorSimulator
{
    [CreateAssetMenu(fileName = "New Error Settings", menuName = "Gaze Error Settings")]
    public class GazeErrorSettings : ScriptableObject
    {
        [Range(0f, 360f), Tooltip("Direction of offset in degrees (0-359). 0 = right, counterclockwise direction.")] 
        public float gazeAccuracyErrorDirection;
        
        [Min(0), Tooltip("Amplitude of offset in visual degrees.")] 
        public float gazeAccuracyError;

        [Tooltip("Type of precision error distribution.")] 
        public PrecisionErrorMode precisionErrorMode;

        [Min(0), Tooltip("Amplitude of precision error.")] public float precisionError;

        [Range(0f, 1f), Tooltip("Probability of data loss (increasing).")] public float dataLossProbability;
    }
}