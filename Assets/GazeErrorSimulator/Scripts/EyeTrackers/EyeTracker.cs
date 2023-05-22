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

        /// <summary>
        /// Initialize the eye tracker.
        /// This method must be called before calling GetGazeData.
        /// By default, this method returns false.
        /// </summary>
        /// <returns></returns>
        public virtual bool Initialize()
        {
            Debug.LogError($"Could not initialize {this.GetType().Name}!");
            return false;
        }

        /// <summary>
        /// Get the gaze data from the eye tracker.
        /// </summary>
        /// <returns>The gaze data from the eye tracker, empty by default</returns>
        public virtual GazeErrorData GetGazeData()
        {
            return new GazeErrorData();
        }

        /// <summary>
        /// Get the transform of the origin of the eye tracker.
        /// </summary>
        /// <returns>The transform of the origin of the eye tracker, null by default</returns>
        public virtual Transform GetOriginTransform()
        {
            return null;
        }
    }
}