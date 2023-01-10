using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GazeErrorInjector
{
    public abstract class EyeTracker : MonoBehaviour, IEyeTracker 
    {
        public delegate void NewGazeData(GazeData data);
        public event NewGazeData OnNewGazeData;


        private GazeErrorData _latestdata = new GazeErrorData();
        public GazeErrorData LatestData 
        {
            get
            {
                return _latestdata;
            }
            protected set
            {
                _latestdata = value;
                if(OnNewGazeData != null)
                {
                    OnNewGazeData(_latestdata);
                }
            }
        }

        public virtual GazeErrorData GetGazeData()
        {
            return new GazeErrorData();
        }

        public virtual Transform GetOriginTransform()
        {
            return null;
        }

        public virtual bool Initialize()
        {
            Debug.LogError("No Eye Tracker could be Initialized!");
            return false;
        }

        public virtual void Destroy()
        {
           Destroy(this);
        }
    }
}