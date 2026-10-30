using AmongUs.Data;
using AmongUs.Data.Player;
using Hazel;
using Rewired.UI.ControlMapper;
using Steamworks;
using System.Threading.Tasks;
using UnityEngine;

namespace NexusMenu
{
    public partial class MenuUI : MonoBehaviour
    {
        public static bool showMenu = true;
        public Rect windowRect = new Rect(20, 20, 667, 467);
        int selectedGroup = 0;
        public string[] groups = new string[] { "Self", "Game", "Roles", "Host", "Misc", "Spoofs", "About" };
        public string version = "v1.0.1-tuff";

        void Start()
        {
            if (showMenu)
            {
                selectedGroup = 6; // shows the about tab so the user sees the new content/updates
            }
        }

        void Update()
        {
            if (States.panicActive) return;
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                showMenu = !showMenu;
            }
            if (PlayerControl.LocalPlayer != null)
            {
                // noclip
                if (States.noClip)
                {
                    PlayerControl.LocalPlayer.Collider.enabled = !States.noClip;
                }
                else
                {
                    PlayerControl.LocalPlayer.Collider.enabled = true;
                }
            }

            //calls
            SelfFeatures.Teleport();
            MiscFeatures.ForceReset();
            MiscFeatures.DisableReports();
            if (ShipStatus.Instance != null)
            {
                GameFeatures.Sabotages(ShipStatus.Instance);
            }

            // toggle calls
            States.unlockSkins = true;
            NexusSpoofer.Update();

            DataManager.Player.Ban.BanPoints = 0; // removes ban points so the player doesn't get banned
        }

        void OnGUI()
        {
            if (States.panicActive) return;

            if (showMenu)
            {
                windowRect = GUI.Window(0, windowRect, (GUI.WindowFunction)WindowFunction, $"NexusMenu {version}");
            }
        }

        void WindowFunction(int windowID)
        {
            // Sidebar tabs (pure original Unity IMGUI)
            GUILayout.BeginArea(new Rect(10, 25, 150, windowRect.height - 35));
            GUILayout.BeginVertical();
            for (int i = 0; i < groups.Length; i++)
            {
                if (GUILayout.Button(groups[i], GUILayout.Height(24)))
                {
                    selectedGroup = i;
                }
            }
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Panic Mode", GUILayout.Height(24)))
            {
                Panic();
            }
            GUILayout.EndVertical();
            GUILayout.EndArea();

            // Tab content
            GUILayout.BeginArea(new Rect(170, 25, 480, 430));
            switch (selectedGroup)
            {
                case 0: // Self
                    GUILayout.Label("Movement:");
                    GUI.Label(new Rect(0, 20, 450, 20), $"Speed: {States.speed:F1}x");
                    States.speed = GUI.HorizontalSlider(new Rect(0, 40, 450, 20), States.speed, 1.0f, 10.0f);
                    States.noClip = GUI.Toggle(new Rect(0, 65, 450, 20), States.noClip, "No Clip");
                    States.tpToCursor = GUI.Toggle(new Rect(0, 90, 450, 20), States.tpToCursor, "Teleport to Cursor");

                    GUI.Label(new Rect(0, 115, 450, 20), "Visuals:");
                    States.fullBright = GUI.Toggle(new Rect(0, 135, 450, 20), States.fullBright, "Full Bright");
                    States.noShadows = GUI.Toggle(new Rect(0, 160, 450, 20), States.noShadows, "No Shadows");

                    GUI.Label(new Rect(0, 185, 450, 20), "Game:");
                    States.killImpostors = GUI.Toggle(new Rect(0, 205, 450, 20), States.killImpostors, "Kill Impostors");
                    States.cooldownInVents = GUI.Toggle(new Rect(0, 230, 450, 20), States.cooldownInVents, "Cooldown In Vents");
                    States.doTasksAsImpostor = GUI.Toggle(new Rect(0, 255, 450, 20), States.doTasksAsImpostor, "Do Tasks As Impostor");
                    break;

                case 1: // Game
                    GUILayout.Label("Ship:");
                    if (GUI.Button(new Rect(0, 20, 450, 25), "Call Meeting"))
                    {
                        GameFeatures.CallMeeting();
                    }
                    if (GUI.Button(new Rect(0, 50, 450, 25), "Kick Players of Vent"))
                    {
                        GameFeatures.KickVent();
                    }

                    GUI.Label(new Rect(0, 80, 450, 20), "Sabotages:");
                    States.sabotageElectricty = GUI.Toggle(new Rect(0, 100, 450, 20), States.sabotageElectricty, "Sabotage Electricity");
                    States.sabotageReactor = GUI.Toggle(new Rect(0, 125, 450, 20), States.sabotageReactor, "Sabotage Reactor");
                    States.sabotageOxygen = GUI.Toggle(new Rect(0, 150, 450, 20), States.sabotageOxygen, "Sabotage Oxygen");
                    States.sabotageComms = GUI.Toggle(new Rect(0, 175, 450, 20), States.sabotageComms, "Sabotage Comms");
                    States.sabotageMushroomMixup = GUI.Toggle(new Rect(0, 200, 450, 20), States.sabotageMushroomMixup, "Mushroom Mixup");
                    break;

                case 2: // Roles
                    GUILayout.Label("Shapeshifter:");
                    States.noShapeshiftAnimation = GUI.Toggle(new Rect(0, 20, 450, 20), States.noShapeshiftAnimation, "No Shapeshift Animation");
                    States.endlessShapeshift = GUI.Toggle(new Rect(0, 45, 450, 20), States.endlessShapeshift, "Endless Shapeshift");

                    GUI.Label(new Rect(0, 70, 450, 20), "Engineer:");
                    States.endlessVentTime = GUI.Toggle(new Rect(0, 90, 450, 20), States.endlessVentTime, "Endless Vent Time");
                    States.noVentCooldown = GUI.Toggle(new Rect(0, 115, 450, 20), States.noVentCooldown, "No Vent Cooldown");

                    GUI.Label(new Rect(0, 140, 450, 20), "Phantom:");
                    States.endlessVanish = GUI.Toggle(new Rect(0, 160, 450, 20), States.endlessVanish, "Endless Vanish");
                    States.noPhantomCooldown = GUI.Toggle(new Rect(0, 185, 450, 20), States.noPhantomCooldown, "No Phantom Cooldown");
                    break;

                case 3: // Host
                    GUILayout.Label("Game:");
                    if (GUI.Button(new Rect(0, 20, 450, 25), "Reset Tasks"))
                    {
                        if (ShipStatus.Instance != null && AmongUsClient.Instance.AmHost)
                        {
                            ShipStatus.Instance.Begin();
                        }
                    }
                    States.unlockKillButton = GUI.Toggle(new Rect(0, 50, 450, 20), States.unlockKillButton, "Unlock Kill Button");

                    GUI.Label(new Rect(0, 75, 450, 20), "Lobby:");
                    States.noEndGame = GUI.Toggle(new Rect(0, 95, 450, 20), States.noEndGame, "Force No End Game");
                    if (GUI.Button(new Rect(0, 120, 450, 25), "Force Start Game"))
                    {
                        if (AmongUsClient.Instance.AmHost)
                        {
                            AmongUsClient.Instance.StartGame();
                        }
                    }
                    if (GUI.Button(new Rect(0, 150, 450, 25), "Force End Game"))
                    {
                        if (AmongUsClient.Instance.AmHost)
                        {
                            GameManager.Instance.RpcEndGame(GameOverReason.CrewmateDisconnect, true);
                        }
                    }
                    break;

                case 4: // Misc
                    GUILayout.Label("<color=red>Warning: Some features may cause a kick or ban.</color>");
                    if (GUI.Button(new Rect(0, 20, 450, 25), "Invisible"))
                    {
                        AmongUsClient.Instance.Despawn(PlayerControl.LocalPlayer.MyPhysics);
                    }
                    if (GUI.Button(new Rect(0, 50, 450, 25), "Send Red Messages"))
                    {
                        MiscFeatures.RedChat();
                    }
                    if (GUI.Button(new Rect(0, 80, 450, 25), "VoteKick Host"))
                    {
                        DestroyableSingleton<VoteBanSystem>.Instance.CmdAddVote(AmongUsClient.Instance.HostId);
                    }
                    States.forceReset = GUI.Toggle(new Rect(0, 110, 450, 20), States.forceReset, "Force Reset Lobby");
                    States.disableReport = GUI.Toggle(new Rect(0, 140, 450, 20), States.disableReport, "Disable Reports");
                    break;

                case 5: // Spoofs
                    GUILayout.Label("Level Spoof:");
                    States.spoofLevel = GUI.Toggle(new Rect(0, 20, 450, 20), States.spoofLevel, "Enable Level Spoof");
                    GUI.Label(new Rect(0, 45, 450, 20), $"Level: {(int)States.spoofedLevel}");
                    States.spoofedLevel = GUI.HorizontalSlider(new Rect(0, 65, 450, 20), States.spoofedLevel, 1f, 100f);

                    if (GUI.Button(new Rect(0, 90, 105, 25), "Lvl 1"))
                    {
                        States.spoofedLevel = 1f;
                        NexusSpoofer.ApplyLevel(1);
                    }
                    if (GUI.Button(new Rect(115, 90, 105, 25), "Lvl 50"))
                    {
                        States.spoofedLevel = 50f;
                        NexusSpoofer.ApplyLevel(50);
                    }
                    if (GUI.Button(new Rect(230, 90, 105, 25), "Lvl 100"))
                    {
                        States.spoofedLevel = 100f;
                        NexusSpoofer.ApplyLevel(100);
                    }
                    if (GUI.Button(new Rect(345, 90, 105, 25), "Lvl 999"))
                    {
                        States.spoofedLevel = 999f;
                        NexusSpoofer.ApplyLevel(999);
                    }

                    GUI.Label(new Rect(0, 125, 450, 20), "Platform Spoof:");
                    States.spoofPlatform = GUI.Toggle(new Rect(0, 145, 450, 20), States.spoofPlatform, "Enable Platform Spoof");
                    if (GUI.Button(new Rect(0, 170, 450, 25), $"Platform: {NexusSpoofer.platformNames[NexusSpoofer.selectedPlatform]}"))
                    {
                        NexusSpoofer.selectedPlatform = (NexusSpoofer.selectedPlatform + 1) % NexusSpoofer.platforms.Length;
                    }
                    break;

                case 6: // About
                    GUILayout.Label($"Welcome to NexusMenu!\nVersion: {version}\nDeveloper: https://github.com/devPerron");
                    if (GUI.Button(new Rect(0, 60, 450, 25), "Github"))
                    {
                        Application.OpenURL("");
                    }
                    break;
            }
            GUILayout.EndArea();

            GUI.DragWindow();
        }

        public void Panic()
        {
            States.Panic();
            try
            {
                if (PlayerControl.LocalPlayer != null && PlayerControl.LocalPlayer.Collider != null)
                {
                    PlayerControl.LocalPlayer.Collider.enabled = true;
                }
                if (DestroyableSingleton<HudManager>.InstanceExists && DestroyableSingleton<HudManager>.Instance.ShadowQuad != null)
                {
                    DestroyableSingleton<HudManager>.Instance.ShadowQuad.gameObject.SetActive(true);
                }
            }
            catch { }
            try
            {
                NexusMenu.HarmonyInstance?.UnpatchSelf();
            }
            catch { }
            showMenu = false;
            enabled = false;
            ModManager.Instance.ModStamp.enabled = false;
            Destroy(this);
        }
    }
}
