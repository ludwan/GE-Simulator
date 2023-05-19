using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GazeErrorSimulator
{
    /// <summary>
    /// Data class for data from a single eye. With error data.
    /// </summary>

    public class EyeErrorData : EyeData
    {
        /// <summary>
        /// Gaze Direction with error.
        /// </summary>
        public Vector3 ErrorDirection;

        /// <summary>
        /// Valid after data loss simulation.
        /// </summary>
        public bool isErrorDataValid;

        /// <summary>
        /// Direction of the accuracy error in degrees (0-359). 0 = right, counterclockwise direction
        /// </summary>
        public float AccuracyErrorDirection;

        /// <summary>
        /// Amplitude of offset in visual degrees.
        /// </summary>
        public float AccuracyError;

        /// <summary>
        /// Type of precision error simulation.
        /// </summary>
        public PrecisionErrorMode PrecisionMode;

        /// <summary>
        /// Magnitude of precision error in visual degrees.
        /// </summary>
        public float PrecisionError;

        /// <summary>
        /// Probability of data loss (0-1).
        /// </summary>
        public float DataLossProbability;

        public EyeErrorData () { }

        public EyeErrorData (EyeData data) 
        {
            this.Timestamp = data.Timestamp;
            Origin = data.Origin;
            Direction = data.Direction;
            isDataValid = data.isDataValid;

            ErrorDirection = Direction;
        }
    }
}

