using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if VIVE_SDK

#endif

namespace GazeErrorInjector
{
    public class SrAnipalEyeTracker : MonoBehaviour, EyeTracker
    {
        private GazeErrorData _latestdata = new GazeErrorData();
        public GazeErrorData LatestData 
        {
            get
            {
                return _latestdata;
            }
        }

        #if VIVE_SDK
            public bool Initialize()
            {
                //TODO: ADD GAMOBJECT.
                if (!SRanipal_Eye_API.IsViveProEye()) return false;

                return (SRanipal_Eye_Framework.Status == SRanipal_Eye_Framework.FrameworkStatus.WORKING);                
            }
            
            public void GetGazeData()
            {
                if (SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.WORKING &&
                            SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.NOT_SUPPORT) return;
                
                _localEyeGazeData.Timestamp = Time.unscaledTime;
                _localEyeGazeData.isRayValid = SRanipal_Eye.GetGazeRay(GazeIndex.COMBINE, out _localEyeGazeData.Origin, out _localEyeGazeData.Direction);
            }            

            public void Destroy() { }
        #else
            public bool Initialize()
            {
                Debug.LogError("Could not initialize SRanipal Eye Tracker.");
                return false;
            }

            public void GetGazeData() { }

            public void Destroy() { }
        #endif
        
    }
}