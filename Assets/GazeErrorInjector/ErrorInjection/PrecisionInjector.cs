using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GazeErrorInjector
{
    public abstract class PrecisionInjector : Injector
    {
        [Range(0f, 10f)] public float error;
        public abstract override Vector3 Inject(Vector3 direction);
    }
}