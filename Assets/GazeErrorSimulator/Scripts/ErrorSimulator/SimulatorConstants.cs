using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GazeErrorSimulator
{
    /// <summary>
    /// List of eye trackers supported by toolkit.
    /// </summary>
    public enum EyeTrackerList
    {
        None,
        Camera,
        HTCViveSranipal,
        Varjo,
        HoloLens2,
        QuestPro
    }

    /// <summary>
    /// Constants for toolkit. Update this script to support more eye trackers.
    /// </summary>
    public static class SimulatorConstants
    {
        //List of Compiler flags for each eye tracker.
        public const string NoEyeTrackerCompilerFlag = "NO_SDK";
        public const string SranipalCompilerFlag = "VIVE_SDK";
        public const string VarjoCompilerFlag = "VARJO_SDK";
        public const string HololensCompilerFlag = "HOLOLENS_SDK";
        public const string QuestProCompilerFlag = "QUESTPRO_SDK";

        //List of eye tracker scripts.
        public const string CameraName = "GazeErrorSimulator.CameraEyeTracker";
        public const string SranipalName = "GazeErrorSimulator.SrAnipalEyeTracker";
        public const string VarjoName = "GazeErrorSimulator.VarjoEyeTracker";
        public const string HoloLensName = "GazeErrorSimulator.HoloLensEyeTracker";
        public const string QuestProName = "GazeErrorSimulator.QuestProEyeTracker";


        /// <summary>
        /// Get the compiler flag based on eye tracker.
        /// </summary>
        /// <param name="eyeTracker">Defined eye tracker.</param>
        /// <returns>Corresponding compiler flag.</returns>
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

        /// <summary>
        /// Return the script name based on eye tracker.
        /// </summary>
        /// <param name="eyeTracker">Defined eye tracker.</param>
        /// <returns>Corresponding eye tracker.</returns>
        public static string GetEyeTrackerName(EyeTrackerList eyeTracker)
        {
            switch (eyeTracker)
            {
                case EyeTrackerList.Camera:
                    return CameraName;
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
