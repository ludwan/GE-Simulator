using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GazeErrorSimulator
{
    /// <summary>
    /// Interface for eye trackers.
    /// </summary>
    public interface IEyeTracker
    {
        /// <summary>
        /// Initializes the eye tracker.
        /// </summary>
        /// <returns>True if initialization was succesful</returns>
        public bool Initialize();

        /// <summary>
        /// Get the latest gaze data from eye tracker. Does not add Error to gaze data.
        /// </summary>
        /// <returns>Latest gaze data (no added error).</returns>
        public GazeErrorData GetGazeData();

        /// <summary>
        /// Returns the Transform of the gaze origin.
        /// </summary>
        /// <returns>Gaze origin Transform.</returns>
        public Transform GetOriginTransform();
    }
}