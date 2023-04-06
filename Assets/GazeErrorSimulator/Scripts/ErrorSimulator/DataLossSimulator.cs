using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GazeErrorSimulator
{
    /// <summary>
    /// Simulates data loss through the probability of data being invalid. 
    /// </summary>
    public class DataLossSimulator : Simulator
    {
        /// <summary>
        /// Probability of gaze vector being invalid.
        /// </summary>
        public float dataLossProbability = 0.5f;

        /// <summary>
        /// Simulates gaze error based on a set probability of there being data loss.
        /// </summary>
        /// <param name="direction">Gaze vector</param>
        /// <returns>Vector zero if data is invalid, otherwise returns original gaze vector.</returns>
        public override Vector3 Inject(Vector3 direction)
        {
            float val = Random.Range(0, 1f);

            if (val <= dataLossProbability)
                return Vector3.zero;

            return direction;
        }
    }
}