using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GazeErrorSimulator
{
    /// <summary>
    /// Precision error based on a Gaussian distribution Precision error is picked from a Gaussian distribution between of std = error. 
    /// </summary>
    public class GaussianPrecisionSimulator : PrecisionSimulator
    {
        /// <summary>
        /// Adds a precision error based on a random direction, and a random amplitude from a gaussian distribution with std=error.
        /// </summary>
        /// <param name="direction">Gaze vector</param>
        /// <returns>Gaze vector with added error.</returns
        public override Vector3 Inject(Vector3 direction)
        {
            float x = Gaussian(error);
            float y = Gaussian(error);

            float theta = (Mathf.Rad2Deg * Mathf.Atan2(y, x) + 360f) % 360f;
            float r = Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(y, 2));

            return ApplyOffset(direction, _hmd.up, theta, r);
        }

        /// <summary>
        /// Sample a random value from a Gaussian distribtion with a set standard distribution.
        /// </summary>
        /// <param name="sd">Standard deviation of distribution.</param>
        /// <returns>Value sampled from distribtion</returns>
        protected float Gaussian(float sd)
        {
            return Gaussian() * sd;
        }

        /// <summary>
        /// Marsaglia-polar method for simulating a Gaussian distribution.
        /// https://doi.org/10.1137/1006063
        /// </summary>
        /// <returns>Returns a value from a uniform distribution with avg = 0, and std=1.</returns>
        protected float Gaussian()
        {
            float v1 = 0f;
            float v2 = 0f;
            float s = 0f;

            while (s >= 1.0f || s == 0f)
            {
                v1 = 2.0f * Random.Range(0f, 1f) - 1.0f;
                v2 = 2.0f * Random.Range(0f, 1f) - 1.0f;
                s = v1 * v1 + v2 * v2;
            }
            s = Mathf.Sqrt((-2.0f * Mathf.Log(s)) / s);
            return v1 * s;
        }
    }
}
