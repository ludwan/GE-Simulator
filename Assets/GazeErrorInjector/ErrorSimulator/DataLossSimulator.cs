using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GazeErrorSimulator
{
    public class DataLossSimulator : Simulator
    {
        public float dataLossProbability = 0.5f;
        public override Vector3 Inject(Vector3 direction)
        {
            float val = Random.Range(0, 1f);

            if (val <= dataLossProbability)
                return Vector3.zero;

            return direction;
        }
    }
}