using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GazeErrorInjector
{
    public interface EyeTrackerProvider
    {
        public GazeErrorData LatestData {get;}       
        public bool Initialize();
        public void GetGazeData();
        public void Destroy();
    }
}