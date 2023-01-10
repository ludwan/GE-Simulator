using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GazeErrorInjector
{
    public class GazeErrorData
    {
        public ErrorMode Mode;

        public EyeErrorData Gaze;

        public EyeErrorData RightEye;

        public EyeErrorData LeftEye;

        public GazeErrorData() { }

        public GazeErrorData (ErrorMode mode, EyeData gaze, EyeData leftEye, EyeData rightEye) 
        {
            Mode = mode;
            Gaze = new EyeErrorData(gaze);
            LeftEye = new EyeErrorData(leftEye);
            RightEye = new EyeErrorData(rightEye);
        }
    }
}