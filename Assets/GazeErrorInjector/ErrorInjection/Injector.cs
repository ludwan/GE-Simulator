using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GazeErrorInjector
{
    public abstract class Injector : MonoBehaviour
    {
        public abstract void Inject(Vector3 direction);
    }
}
