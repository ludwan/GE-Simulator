using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GazeErrorInjector
{
    public enum EyeTrackerList
    {
        HTCViveSranipal,
        TobiiXr,
        PupiLabs,
        Varjo,
        HoloLens2
    }

    public static class GazeErrorInjectorConstants
    {
        public const string SranipalCompilerFlag = "VIVE_SDK";
        public const string TobiiXrCompilerFlag = "TOBII_SDK";
        public const string PupilCompilerFlag = "PUPIL_SDK";
        public const string VarjoCompilerFlag = "VARJO_SDK";
        public const string HololensCompilerFlag = "HOLOLENS_SDK";

        public const string SranipalName = "GazeMetrics.SrAnipalEyeTracker";
        public const string TobiiXrName = "GazeMetrics.TobiiProEyeTracker";
        public const string PupilName = "GazeMetrics.PupilEyeTracker";
        public const string VarjoName = "GazeMetrics.VarjoEyeTracker";
        public const string HoloLensName = "GazeMetrics.HoloLensEyeTracker";

        public static string GetEyeTrackerCompilerFlag(EyeTrackerList eyeTracker)
        {
            switch(eyeTracker)
            {
                case EyeTrackerList.HTCViveSranipal:
                    return SranipalCompilerFlag;
                case EyeTrackerList.PupiLabs:
                    return PupilCompilerFlag;
                case EyeTrackerList.TobiiXr:
                    return TobiiXrCompilerFlag;
                case EyeTrackerList.Varjo:
                    return VarjoCompilerFlag;
                case EyeTrackerList.HoloLens2:
                    return HololensCompilerFlag;
                default:
                    return null; 
            }
        }

        public static string GetEyeTrackerName(EyeTrackerList eyeTracker)
        {
            switch(eyeTracker)
            {
                case EyeTrackerList.HTCViveSranipal:
                    return SranipalName;
                case EyeTrackerList.PupiLabs:
                    return PupilName;
                case EyeTrackerList.TobiiXr:
                    return TobiiXrName;
                case EyeTrackerList.Varjo:
                    return VarjoCompilerFlag;
                case EyeTrackerList.HoloLens2:
                    return HoloLensName;
                default:
                    return null; 
            }
        }

    }


}
