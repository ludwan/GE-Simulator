using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GazeErrorSimulator
{
    public class UniformPrecisionSimulator : PrecisionSimulator
    {
        public override Vector3 Inject(Vector3 direction)
        {
            Vector2 pos = Random.insideUnitCircle * error;

            float theta = (Mathf.Rad2Deg * Mathf.Atan2(pos.y, pos.x) + 360f) % 360f;
            float r = Mathf.Sqrt(Mathf.Pow(pos.x, 2) + Mathf.Pow(pos.y, 2));

            return ApplyOffset(direction, _hmd.up, theta, r);
        }
    }
}