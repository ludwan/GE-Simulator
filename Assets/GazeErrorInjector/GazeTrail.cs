using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GazeErrorSimulator
{
    public enum DataType
    {
        Error,
        Original
    }

    public enum Eye
    {
        Gaze,
        Right,
        Left
    }

    public class GazeTrail : MonoBehaviour
    {

        public ErrorSimulator _manager;
        [SerializeField] private bool _isActive = true;
        [SerializeField] private Eye _eye = Eye.Gaze;
        [SerializeField] private DataType _type = DataType.Error;
        [SerializeField] private Color _color = Color.red;
        [SerializeField] private ParticleSystem _particleSystem;

        [SerializeField, Range(0, 1000)]
        [Tooltip("The number of particles to allocate. Use zero to use only the last hit object.")]
        private int _particleCount = 100;

        [SerializeField, Range(0.005f, 0.2f)]
        [Tooltip("The size of the particle.")]
        private float _particleSize = 0.05f;
        [SerializeField] private float _particleDistance = 2f;

        private ParticleSystem.Particle[] _particles;
        private int _particleIndex;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            _particles = new ParticleSystem.Particle[_particleCount];
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
            if (!_isActive) return;

            Vector3 pos = Vector3.zero;
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


            if (ray.direction == Vector3.zero)
            {
                PlaceParticle(pos, Color.white, 0);
            }
            else
            {
                pos = ray.GetPoint(_particleDistance);
                PlaceParticle(pos, _color, _particleSize);
            }
            _particleSystem.SetParticles(_particles, _particles.Length);
        }

        private void RemoveParticles()
        {
            for (int i = 0; i < _particles.Length; i++)
            {
                PlaceParticle(Vector3.zero, Color.white, 0);
            }
            _particleSystem.SetParticles(_particles, _particles.Length);
        }

        private void PlaceParticle(Vector3 pos, Color color, float size)
        {
            var particle = _particles[_particleIndex];
            particle.position = pos;
            particle.startColor = color;
            particle.startSize = size;
            _particles[_particleIndex] = particle;
            _particleIndex = (_particleIndex + 1) % _particles.Length;
        }

        public void SetActive(bool activate)
        {
            _isActive = activate;
            if (!activate)
            {
                RemoveParticles();
            }
        }

        private Ray GetRay()
        {
            return new Ray();
        }
    }
}