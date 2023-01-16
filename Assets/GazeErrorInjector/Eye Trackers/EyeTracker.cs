using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GazeErrorInjector
{
    public abstract class EyeTracker : MonoBehaviour, IEyeTracker
    {
        public delegate void NewGazeData(GazeErrorData data);

        public virtual bool Initialize()
        {
            Debug.LogError($"Could not initialize {this.GetType().Name}!");
            return false;
        }

        public virtual GazeErrorData GetGazeData()
        {
            return new GazeErrorData();
        }

        public virtual Transform GetOriginTransform()
        {
            return null;
        }
    }
}