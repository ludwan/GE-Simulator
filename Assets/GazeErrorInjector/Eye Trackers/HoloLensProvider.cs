using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GazeErrorInjector
{
    public class HoloLensProvider : MonoBehaviour, EyeTrackerProvider
    {
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }
        
        public GazeErrorData LatestData 
        {
            get
            {
                throw new System.NotImplementedException();
            }
        }

        public bool Initialize()
        {
            throw new System.NotImplementedException();
        }

        public void GetGazeData()
        {
            throw new System.NotImplementedException();
        }
        public void Destroy()
        {
            throw new System.NotImplementedException();
        }
    }
}