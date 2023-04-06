using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GazeErrorSimulator
{
    /// <summary>
    /// Base class for eye tracker. Inherit from this script if you wish to add a new eye tracker.
    /// </summary>
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