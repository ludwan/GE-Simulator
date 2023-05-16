using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GazeErrorSimulator
{
    public class GazeTrail : GazeRenderer
    {
        [SerializeField]
        [Tooltip("A reference to a Unity Particle System used to render the gaze trail particles")]
        private ParticleSystem _particleSystem;

        [SerializeField, Range(0, 1000)]
        [Tooltip("The number of particles to allocate. Use zero to use only the last hit object")]
        private int _particleCount = 100;

        [SerializeField, Range(0.005f, 0.2f)]
        [Tooltip("The size of the particle")]
        private float _particleSize = 0.05f;

        [SerializeField]
        [Tooltip("How far along the the gaze direction should the particle be rendered?")]
        private float _particleDistance = 2f;

        private ParticleSystem.Particle[] _particles;
        private int _particleIndex;

        protected override void Start()
        {
            // Create a pool of particles to use for the gaze trail
            _particles = new ParticleSystem.Particle[_particleCount];
            base.Start();
        }

        public override void UpdatePosition(GazeErrorData data)
        {
            // Ensure that the line can and should be rendered
            if (!_isActive || data == null) return;

            Vector3 pos = Vector3.zero;
            // Get the latest gaze data based on the selected eye and data error type
            Ray ray = data.GetGazeRay(_eye, _type);

            if (ray.direction == Vector3.zero)
            {
                // Add an invisible particle if the ray direction is zero
                PlaceParticle(pos, Color.white, 0);
            }
            else
            {
                // Get the position at the specified distance along the gaze direction
                pos = ray.GetPoint(_particleDistance);
                // Add a particle at the calculated position
                PlaceParticle(pos, _color, _particleSize);
            }

            // Update the particle system with the new particles
            _particleSystem.SetParticles(_particles, _particles.Length);
        }

        private void RemoveParticles()
        {
            // Make all the particles invisible (zero size)
            for (int i = 0; i < _particles.Length; i++)
                PlaceParticle(Vector3.zero, Color.white, 0);

            // Update the particle system with the new particles
            _particleSystem.SetParticles(_particles, _particles.Length);
        }

        private void PlaceParticle(Vector3 pos, Color color, float size)
        {
            // Get the next particle in the pool
            var particle = _particles[_particleIndex];

            // Set the particle properties
            particle.position = pos;
            particle.startColor = color;
            particle.startSize = size;

            // Update the particle in the pool
            _particles[_particleIndex] = particle;

            // Increment the particle index to move to the next particle in the pool
            _particleIndex = (_particleIndex + 1) % _particles.Length;
        }

        public override void SetActive(bool activate)
        {
            base.SetActive(activate);

            // Remove all particles if the gaze trail is disabled
            if (!activate) RemoveParticles();
        }
    }
}