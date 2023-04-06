using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GazeErrorSimulator
{
    /// <summary>
    /// Precision error simulator based on a uniform distribution. Precision error is picked from a uniform distribution between 0 and error.
    /// </summary>
    public class UniformPrecisionSimulator : PrecisionSimulator
    {
        /// <summary>
        /// Adds a precision error based on a random direction, and a random amplitude between 0 and error from a uniform distribution.
        /// </summary>
        /// <param name="direction">Gaze vector</param>
        /// <returns>Gaze vector with added error.</returns>
        public override Vector3 Inject(Vector3 direction)
        {
            Vector2 pos = Random.insideUnitCircle * error;

            float theta = (Mathf.Rad2Deg * Mathf.Atan2(pos.y, pos.x) + 360f) % 360f;
            float r = Mathf.Sqrt(Mathf.Pow(pos.x, 2) + Mathf.Pow(pos.y, 2));

            return ApplyOffset(direction, _hmd.up, theta, r);
        }
    }
}