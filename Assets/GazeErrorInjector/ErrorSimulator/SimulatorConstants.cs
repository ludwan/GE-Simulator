using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GazeErrorSimulator
{
    public enum EyeTrackerList
    {
        HTCViveSranipal,
        Varjo,
        HoloLens2,
        QuestPro
    }

    public static class SimulatorConstants
    {
        public const string SranipalCompilerFlag = "VIVE_SDK";
        public const string VarjoCompilerFlag = "VARJO_SDK";
        public const string HololensCompilerFlag = "HOLOLENS_SDK";
        public const string QuestProCompilerFlag = "QUESTPRO_SDK";

        public const string SranipalName = "GazeErrorSimulator.SrAnipalEyeTracker";
        public const string VarjoName = "GazeErrorSimulator.VarjoEyeTracker";
        public const string HoloLensName = "GazeErrorSimulator.HoloLensEyeTracker";
        public const string QuestProName = "GazeErrorSimulator.QuestProEyeTracker";

        public static string GetEyeTrackerCompilerFlag(EyeTrackerList eyeTracker)
        {
            switch (eyeTracker)
            {
                case EyeTrackerList.HTCViveSranipal:
                    return SranipalCompilerFlag;
                case EyeTrackerList.Varjo:
                    return VarjoCompilerFlag;
                case EyeTrackerList.HoloLens2:
                    return HololensCompilerFlag;
                case EyeTrackerList.QuestPro:
                    return QuestProCompilerFlag;
                default:
                    return null;
            }
        }

        public static string GetEyeTrackerName(EyeTrackerList eyeTracker)
        {
            switch (eyeTracker)
            {
                case EyeTrackerList.HTCViveSranipal:
                    return SranipalName;
                case EyeTrackerList.Varjo:
                    return VarjoName;
                case EyeTrackerList.HoloLens2:
                    return HoloLensName;
                case EyeTrackerList.QuestPro:
                    return QuestProName;
                default:
                    return null;
            }
        }

    }


}
