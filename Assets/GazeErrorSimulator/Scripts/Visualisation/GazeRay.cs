using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GazeErrorSimulator
{
    [RequireComponent(typeof(LineRenderer))]
    public class GazeRay : GazeRenderer
    {  
        [SerializeField, Tooltip("How far along the gaze ray should the line be rendered?")] 
        private float rayDistance = 2f;

        private LineRenderer _lineRenderer;

        void Awake()
        {
            // Get a reference to the line renderer
            _lineRenderer = GetComponent<LineRenderer>();
        }

        /// <summary>
        /// Update the positions of the gaze ray based on the latest gaze data.
        /// </summary>

        /// <summary>
        /// Update the positions of the gaze ray based on the latest gaze data.
        /// </summary>
        /// <param name="data">The latest gaze data</param>
        public override void UpdatePosition(GazeErrorData data)
        {
            // Ensure that the line can and should be rendered
            if (!_isActive || data == null) return;

            // Get the latest gaze data based on the selected eye and data error type
            Ray ray = data.GetGazeRay(_eye, _type);

            // Set the start position of the ray to be 5cm below the origin such that it is visible
            _lineRenderer.SetPosition(0, ray.origin - Vector3.up * 0.05f);
            // Set the end position of the ray to be some distance along the gaze direction
            _lineRenderer.SetPosition(1, ray.origin + ray.direction * rayDistance);
        }

        /// <summary>
        /// Set the active state of the gaze ray.
        /// </summary>
        /// <param name="activate">The active state of the gaze ray</param>
        public override void SetActive(bool activate)
        {
            base.SetActive(activate);
            _lineRenderer.enabled = activate;
        }
    }
}