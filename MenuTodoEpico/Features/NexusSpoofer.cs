using AmongUs.Data;
using AmongUs.Data.Player;
using HarmonyLib;
using System;

namespace NexusMenu
{
    public static class NexusSpoofer
    {
        public static int selectedPlatform = 5;
        public static readonly Platforms[] platforms = new Platforms[]
        {
            Platforms.StandaloneSteamPC,
            Platforms.StandaloneEpicPC,
            Platforms.Playstation,
            Platforms.Xbox,
            Platforms.Switch,
            Platforms.Android,
            Platforms.IPhone
        };

        public static readonly string[] platformNames = new string[]
        {
            "Steam",
            "Epic Games",
            "PlayStation",
            "Xbox",
            "Nintendo Switch",
            "Android",
            "iOS"
        };

        public static void ApplyLevel(uint displayLevel)
        {
            try
            {
                uint internalLevel = (uint)Math.Max(0, (int)displayLevel - 1);

                if (DataManager.Player?.Stats != null)
                {
                    DataManager.Player.Stats.Level = internalLevel;
                }

                if (PlayerControl.LocalPlayer != null)
                {
                    if (PlayerControl.LocalPlayer.Data != null)
                    {
                        PlayerControl.LocalPlayer.Data.PlayerLevel = internalLevel;
                    }
                    PlayerControl.LocalPlayer.RpcSetLevel(internalLevel);
                    PlayerControl.LocalPlayer.SetLevel(internalLevel);
                }
            }
            catch {}
        }

        public static void Update()
        {
            try
            {
                if (States.spoofLevel)
                {
                    uint displayLevel = (uint)Math.Clamp((int)States.spoofedLevel, 1, 9999);
                    uint internalLevel = (uint)Math.Max(0, (int)displayLevel - 1);

                    if (DataManager.Player?.Stats != null && DataManager.Player.Stats.Level != internalLevel)
                    {
                        DataManager.Player.Stats.Level = internalLevel;
                    }
                    if (PlayerControl.LocalPlayer != null && PlayerControl.LocalPlayer.Data != null && PlayerControl.LocalPlayer.Data.PlayerLevel != internalLevel)
                    {
                        ApplyLevel(displayLevel);
                    }
                }
            }
            catch {}
        }

        [HarmonyPatch(typeof(Constants), nameof(Constants.GetPlatformType))]
        public static class SpoofPlatformTypePatch
        {
            public static bool Prefix(ref Platforms __result)
            {
                if (!States.spoofPlatform) return true;
                __result = platforms[selectedPlatform];
                return false;
            }
        }

        [HarmonyPatch(typeof(Constants), nameof(Constants.GetPlatformData))]
        public static class SpoofPlatformDataPatch
        {
            public static void Postfix(ref PlatformSpecificData __result)
            {
                if (!States.spoofPlatform) return;
                if (__result != null)
                {
                    __result.Platform = platforms[selectedPlatform];
                }
            }
        }
    }
}
