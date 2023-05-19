using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GazeErrorSimulator
{
    /// <summary>
    /// Data class for data from a single eye.
    /// </summary>
    public class EyeData
    {
        /// <summary>
        /// Timestamp of the eyetracker (Unity Time).
        /// </summary>
        public float Timestamp;

        /// <summary>
        /// Origin of the gaze ray.
        /// </summary>
        public Vector3 Origin;

        /// <summary>
        /// Direction of the gaze ray (World-space).
        /// </summary>
        public Vector3 Direction;

        /// <summary>
        /// Validity of data.
        /// </summary>
        public bool isDataValid;

        public EyeData() { }

        public EyeData(float timestamp, Vector3 origin, Vector3 direction, bool isValid) 
        {
            Timestamp = timestamp;
            Origin = origin;
            Direction = direction;
            isDataValid = isValid;
        }
    }
}