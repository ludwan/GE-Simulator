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

        public abstract void UpdatePosition(GazeErrorData data);

        public virtual void SetActive(bool activate)
        {
            _isActive = activate;
        }

        public virtual bool IsActive()
        {
            return _isActive;
        }

        public virtual void SetColor(Color color)
        {
            _color = color;
        }
    }
}