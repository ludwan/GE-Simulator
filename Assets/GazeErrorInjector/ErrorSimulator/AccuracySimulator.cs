using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GazeErrorSimulator
{
    public class AccuracySimulator : Simulator
    {
        [Range(0f, 360f)] public float AccuracyDirection = 0;
        [Range(0f, 45f)] public float AccuracyAmplitude = 0;

        public override Vector3 Inject(Vector3 direction)
        {
            return ApplyOffset(direction, _hmd.up, AccuracyDirection, AccuracyAmplitude);
        }
    }
}