using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GazeErrorSimulator
{
    public class GaussianPrecisionSimulator : PrecisionSimulator
    {
        public override Vector3 Inject(Vector3 direction)
        {
            float x = Gaussian(error);
            float y = Gaussian(error);

            float theta = (Mathf.Rad2Deg * Mathf.Atan2(y, x) + 360f) % 360f;
            float r = Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(y, 2));

            return ApplyOffset(direction, _hmd.up, theta, r);
        }

        protected float Gaussian(float sd)
        {
            return Gaussian() * sd;
        }

        //Estimated through https://en.wikipedia.org/wiki/Marsaglia_polar_method
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
