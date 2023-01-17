using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GazeErrorSimulator
{
    [CreateAssetMenu(fileName = "New Error Settings", menuName = "Gaze Error Settings")]
    public class GazeErrorSettings : ScriptableObject
    {
        [Range(0f, 360f)] public float gazeAccuracyErrorDirection;
        [Min(0)] public float gazeAccuracyError;
        public PrecisionErrorMode precisionErrorMode;
        [Min(0)] public float precisionError;
        [Range(0f, 1f)] public float dataLossProbability;
    }
}