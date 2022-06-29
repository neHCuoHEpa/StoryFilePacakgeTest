using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace StoryFiles
{

    public class StoryFilesInfo
    {
        public static Version Version = new Version(0, 1, 1);

        public const string DEPTHKIT_DEFINE = "SF_USING_DEPTHKIT";

        public const string AR_DEFINE = "SF_USING_ARFOUNDATION";

        public const string AV1 = "SF_USING_AVPRO1";
        public const string AV2 = "SF_USING_AVPRO2";


        public static Dictionary<string, string> AssetSearchDict = new Dictionary<string, string>()
        {
            {"Depthkit_Info", StoryFilesInfo.DEPTHKIT_DEFINE},
            {"ARSession", StoryFilesInfo.AR_DEFINE},
            {"DebugOverlay", StoryFilesInfo.AV1},
            {"MediaPlayer_Upgrade", StoryFilesInfo.AV2},
        };

        [SerializeField]
        public static DebugLevel debugLevelFlag = 0;

        [Flags, Serializable]
        public enum DebugLevel
        {
            Nothing = 0,
            Logs = 1 << 0,
            Warnings = 1 << 1,
            Errors = 1 << 2,
            Everything = ~0,
        }

        [SerializeField]
        public static DebugComponent debugComponentsFlag = 0;

        [Flags, Serializable]
        public enum DebugComponent
        {
            Nothing = 0,
            StoryFiles = 1 << 0,
            WebSocket = 1 << 1,
            REST = 1 << 2,
            Everything = ~0,
        }


        public static bool CheckDebug(DebugLevel level, DebugComponent component)
        {
            if ((level == debugLevelFlag || debugLevelFlag == DebugLevel.Everything) && (component == debugComponentsFlag || debugComponentsFlag == DebugComponent.Everything))
            {
                return true;
            }
            return false;
        }

#if UNITY_EDITOR
        public static BuildTargetGroup[] SupportedPlatforms = new BuildTargetGroup[] {
            BuildTargetGroup.Android,
            BuildTargetGroup.Standalone,
            BuildTargetGroup.iOS,
            BuildTargetGroup.PS4,
            BuildTargetGroup.tvOS,
            BuildTargetGroup.XboxOne,
            BuildTargetGroup.WSA
        };
#endif
    }

}
