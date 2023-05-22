using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GazeErrorSimulator
{
    public abstract class GazeRenderer : MonoBehaviour
    {
        [Tooltip("A reference to an Error Simulator to visualise")]
        public ErrorSimulator _errorSimulator;

        [SerializeField]
        [Tooltip("Should the gaze visualisation should be rendered?")]
        protected bool _isActive = true;

        [SerializeField]
        [Tooltip("Do you want to visualise the left, right, or combined gaze?")]
        protected Eye _eye = Eye.Gaze;

        [SerializeField]
        [Tooltip("Do you want to visualise the original or error gaze?")]
        protected DataType _type = DataType.Error;

        [SerializeField]
        [Tooltip("The color of the gaze visualisation")]
        protected Color _color = Color.red;
        
        protected virtual void Start()
        {
            // Add a new data listener to the error simulator to get access
            // to the new (possibly errored) gaze data.
            _errorSimulator.OnNewErrorData += UpdatePosition;
        }

        /// <summary>
        /// Update the positions of the gaze visualisation based on the latest gaze data.
        /// </summary>
        /// <param name="data">The latest gaze data</param>
        public abstract void UpdatePosition(GazeErrorData data);

        
        /// <summary>
        /// Set the active state of the gaze visualisation.
        /// </summary>
        /// <param name="activate">The active state of the gaze visualisation</param>
        public virtual void SetActive(bool activate)
        {
            _isActive = activate;
        }

        /// <summary>
        /// Get the active state of the gaze visualisation.
        /// </summary>
        public virtual bool IsActive()
        {
            return _isActive;
        }

        /// <summary>
        /// Set the color of the gaze visualisation.
        /// </summary>
        /// <param name="color">The color of the gaze visualisation</param>
        public virtual void SetColor(Color color)
        {
            _color = color;
        }
    }
}