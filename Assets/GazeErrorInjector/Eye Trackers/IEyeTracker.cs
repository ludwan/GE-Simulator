using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GazeErrorInjector
{
    public interface IEyeTracker
    {
        public bool Initialize();
        public GazeErrorData GetGazeData();
        public Transform GetOriginTransform();
    }
}