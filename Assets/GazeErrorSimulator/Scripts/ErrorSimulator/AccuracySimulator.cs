using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GazeErrorSimulator
{
    /// <summary>
    /// Accuracy simulator. Adds a constant offset to the gazevector based on a defined direction and amplitude.
    /// </summary>
    public class AccuracySimulator : Simulator
    {
        /// <summary>
        /// Direction of offset in degrees (0-359). 0 = right, counterclockwise direction.
        /// </summary>
        [Range(0f, 360f)] public float AccuracyDirection = 0;
        /// <summary>
        /// Amplitude of offset in visual degrees.
        /// </summary>
        [Range(0f, 45f)] public float AccuracyAmplitude = 0;

        /// <summary>
        /// Adds a predefined offset to the gaze vector.
        /// </summary>
        /// <param name="direction">Gaze direction</param>
        /// <returns>Gaze direction with offset.</returns>
        public override Vector3 Inject(Vector3 direction)
        {
            return ApplyOffset(direction, _hmd.up, AccuracyDirection, AccuracyAmplitude);
        }
    }
}