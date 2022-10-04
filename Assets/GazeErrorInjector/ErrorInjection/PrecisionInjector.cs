using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GazeErrorInjector
{
    public abstract class PrecisionInjector : Injector
    {
        public abstract override void Inject(Vector3 direction);

        protected float Gaussian(float std)
        {
            return Gaussian() * std;  
        }

        //Estimated through https://en.wikipedia.org/wiki/Marsaglia_polar_method
        protected float Gaussian() 
        {
            float v1 = 0f; 
            float v2 = 0f; 
            float s = 0f;

            while (s>=1.0f || s == 0f)
            {
                v1 = 2.0f * Random.Range(0f,1f) - 1.0f;
                v2 = 2.0f * Random.Range(0f,1f) - 1.0f;
                s = v1 * v1 + v2 * v2;
            }
            s = Mathf.Sqrt((-2.0f * Mathf.Log(s)) / s);
            return v1 * s;
        }
    }
}