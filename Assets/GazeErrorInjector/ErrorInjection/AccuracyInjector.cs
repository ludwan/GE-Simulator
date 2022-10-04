using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GazeErrorInjector
{
    public class AccuracyInjector : Injector
    {
        [Range(0f, 360f)] public float AccuracyDirection = 0;
        [Range(0f, 10f)] public float AccuracyAmplitude = 0;

        public override void Inject(Vector3 direction)
        {
            throw new System.NotImplementedException();
        }
    }
}