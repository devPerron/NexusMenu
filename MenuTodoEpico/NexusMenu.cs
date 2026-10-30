using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using UnityEngine;
using UnityEngine.Analytics;

namespace NexusMenu
{
    [BepInAutoPlugin]
    [BepInProcess("Among Us.exe")]
    public partial class NexusMenu : BasePlugin
    {
        public static bool noTelemetry = true;
        public static HarmonyLib.Harmony? HarmonyInstance;

        public override void Load()
        {
            AddComponent<MenuUI>();
            HarmonyInstance = new HarmonyLib.Harmony("com.nexus.menu");
            HarmonyInstance.PatchAll();

            /* Disables Telemetry (haven't fully tested if it works, but according to Unity docs it should) 
            found here: https://github.com/scp222thj/MalumMenu/blob/main/src/MalumMenu.cs at line 205*/
            if (noTelemetry)
            {
                Analytics.enabled = false;
                Analytics.deviceStatsEnabled = false;
                PerformanceReporting.enabled = false;
            }
        }
    }

    [HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.Awake))]
    public class onGameLoad
    {
        public static void Postfix()
        {
            NexusLog.LogInfo("Adding ModStamp");
            ModManager.Instance.ShowModStamp();
        }
    }
}
