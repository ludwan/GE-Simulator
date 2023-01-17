using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GazeErrorSimulator
{
    public abstract class PrecisionSimulator : Simulator
    {
        [Range(0f, 10f)] public float error;
        public abstract override Vector3 Inject(Vector3 direction);
    }
}