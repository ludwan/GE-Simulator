using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GazeErrorSimulator
{
    public class GazeRayRenderer : MonoBehaviour
    {

        public ErrorSimulator _manager;
        [SerializeField] private bool _isActive = true;
        [SerializeField] private Eye _eye = Eye.Gaze;
        [SerializeField] private DataType _type = DataType.Error;
        [SerializeField] private Color _color = Color.red;
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private float rayDistance = 2f;


        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            _manager.OnNewErrorData += UpdatePosition;
        }

        public bool IsActive()
        {
            return _isActive;
        }

        public void SetColor(Color color)
        {
            _color = color;
        }

        public void UpdatePosition(GazeErrorData data)
        {
            if (!_isActive || data == null) return;

            Vector3 pos = Vector3.zero;
            Ray ray = GetGazeRay(data);

            _lineRenderer.SetPosition(0, ray.origin - Vector3.up * 0.05f);
            _lineRenderer.SetPosition(1, ray.origin + ray.direction * rayDistance);
        }

        public void SetActive(bool activate)
        {
            _isActive = activate;
            _lineRenderer.enabled = activate;
        }

        private Ray GetGazeRay(GazeErrorData data)
        {
            Ray ray = new Ray();
            switch (_eye)
            {
                case Eye.Gaze:
                    ray.origin = data.Gaze.Origin;
                    switch (_type)
                    {
                        case DataType.Error:
                            ray.direction = data.Gaze.ErrorDirection;
                            break;
                        case DataType.Original:
                            ray.direction = data.Gaze.Direction;
                            break;
                    }
                    break;
                case Eye.Left:
                    ray.origin = data.LeftEye.Origin;
                    switch (_type)
                    {
                        case DataType.Error:
                            ray.direction = data.LeftEye.ErrorDirection;
                            break;
                        case DataType.Original:
                            ray.direction = data.LeftEye.Direction;
                            break;
                    }
                    break;
                case Eye.Right:
                    ray.origin = data.RightEye.Origin;
                    switch (_type)
                    {
                        case DataType.Error:
                            ray.direction = data.RightEye.ErrorDirection;
                            break;
                        case DataType.Original:
                            ray.direction = data.RightEye.Direction;
                            break;
                    }
                    break;
            }

            return ray;
        }
    }
}