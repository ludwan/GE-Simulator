using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GazeErrorSimulator
{
    /// <summary>
    /// Adds a precision error to the gaze vector (base class).
    /// </summary>
    public abstract class PrecisionSimulator : Simulator
    {
        /// <summary>
        /// Magnitude of precision error.
        /// </summary>
        [Range(0f, 10f)] public float error;

        public abstract override Vector3 Inject(Vector3 direction);
    }
}