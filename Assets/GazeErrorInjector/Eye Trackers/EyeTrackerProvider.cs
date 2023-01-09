using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GazeErrorInjector
{
    public interface EyeTracker
    {
        public GazeErrorData LatestData {get;}       
        public bool Initialize();
        public void GetGazeData();
        public Transform GetOriginTransform();
        public void Destroy();
    }
}