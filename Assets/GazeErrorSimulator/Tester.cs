using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GazeErrorSimulator
{
    public class Tester : MonoBehaviour
    {
        public AccuracySimulator accSimulator;
        public PrecisionSimulator precSimulator;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            Debug.DrawRay(Vector3.zero, Vector3.forward * 10f, Color.red);

            Vector3 errDir = accSimulator.Inject(Vector3.forward);
            Debug.DrawRay(Vector3.zero, errDir * 10f, Color.blue);

            errDir = precSimulator.Inject(errDir);
            Debug.DrawRay(Vector3.zero, errDir * 10f, Color.green);
        }
    }
}