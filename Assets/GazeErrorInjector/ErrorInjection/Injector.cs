using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GazeErrorInjector
{
    public abstract class Injector : MonoBehaviour
    {
        [SerializeField] protected Transform _hmd;
        [SerializeField] public InjectorManager Manager;

        public void Init()
        {
            _hmd = Manager.EyeTracker.GetOriginTransform();
        }

        public abstract Vector3 Inject(Vector3 direction);

        protected Vector3 ApplyOffset(Vector3 direction, Vector3 up, float errorAngle, float errorAmplitude)
        {
            Vector3 errorDirection = Quaternion.AngleAxis(errorAngle, direction) * up;
            Vector3 errorVector = Quaternion.AngleAxis(errorAmplitude, errorDirection) * direction;
            return errorVector;
        }
    }
}
