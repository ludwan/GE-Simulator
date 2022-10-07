using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GazeErrorInjector
{
    public class Tester : MonoBehaviour
    {
        public AccuracyInjector injector;
        public PrecisionInjector precInjector;
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            Debug.DrawRay(Vector3.zero, Vector3.forward * 10f, Color.red);

            Vector3 errDir = injector.Inject(Vector3.forward);
            Debug.DrawRay(Vector3.zero, errDir * 10f, Color.blue);

            errDir = precInjector.Inject(errDir);
            Debug.DrawRay(Vector3.zero, errDir * 10f, Color.green);
        }
    }
}