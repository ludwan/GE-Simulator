using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GazeErrorSimulator
{
    /// <summary>
    /// Base class for error simulator components.
    /// </summary>
    public abstract class Simulator : MonoBehaviour
    {
        [SerializeField] protected Transform _hmd;
        [SerializeField] public ErrorSimulator Manager;

        /// <summary>
        /// Get Eye tracker origin.
        /// </summary>
        public void Init()
        {
            _hmd = Manager.EyeTracker.GetOriginTransform();
        }

        /// <summary>
        /// Adds the error of the error component to a gaze vector.
        /// </summary>
        /// <param name="direction">Gaze vector</param>
        /// <returns>Gaze vector with added error.</returns>
        public abstract Vector3 Inject(Vector3 direction);

        /// <summary>
        /// Applies an offset to a directional Vector3 based on a defined upwards direction, amplitude and angle.
        /// </summary>
        /// <param name="direction">Original directional vector.</param>
        /// <param name="up">The upwards direction, from which the error direction is calculated from (commonly HMD upwards direction).</param>
        /// <param name="errorAngle">Direction of offset in degrees (0-359). 0 = right, counterclockwise direction.</param>
        /// <param name="errorAmplitude">Amplitude of offset (angular degrees).</param>
        /// <returns>Directional vector with added offset.</returns>
        protected Vector3 ApplyOffset(Vector3 direction, Vector3 up, float errorAngle, float errorAmplitude)
        {
            Vector3 errorDirection = Quaternion.AngleAxis(errorAngle, direction) * up;
            Vector3 errorVector = Quaternion.AngleAxis(errorAmplitude, errorDirection) * direction;
            return errorVector;
        }
    }
}
