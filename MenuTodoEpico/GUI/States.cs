using UnityEngine;

namespace NexusMenu
{
    public struct States
    {
        // Self
        public static bool noClip;
        public static bool speedHack;
        public static float speed = 1.0f;
        public static bool tpToCursor;
        public static bool fullBright;
        public static bool noShadows;
        public static bool killImpostors;
        public static bool cooldownInVents;
        public static bool doTasksAsImpostor;

        // Game
        public static bool sabotageElectricty;
        public static bool sabotageComms;
        public static bool sabotageHeli;
        public static bool sabotageLab;
        public static bool sabotageMushroomMixup;
        public static bool sabotageOxygen;
        public static bool sabotageReactor;

        // Roles
        public static bool noShapeshiftAnimation;
        public static bool endlessShapeshift;
        public static bool endlessVentTime;
        public static bool noVentCooldown;
        public static bool endlessVanish;
        public static bool noPhantomCooldown;

        // Host
        public static bool noEndGame;
        public static bool unlockKillButton;

        // Misc
        public static bool gameOver;
        public static bool unlockSkins;
        public static bool forceReset;
        public static bool disableReport;

        // Spoofing
        public static bool spoofPlatform;
        public static bool spoofLevel;
        public static float spoofedLevel = 100f;

        // Panic Mode
        public static bool panicActive = false;

        public static void Panic()
        {
            panicActive = true;
            noClip = false;
            speedHack = false;
            speed = 1.0f;
            tpToCursor = false;
            fullBright = false;
            noShadows = false;
            killImpostors = false;
            cooldownInVents = false;

            sabotageElectricty = false;
            sabotageComms = false;
            sabotageHeli = false;
            sabotageLab = false;
            sabotageMushroomMixup = false;
            sabotageOxygen = false;
            sabotageReactor = false;

            noShapeshiftAnimation = false;
            endlessShapeshift = false;
            noVentCooldown = false;
            endlessVentTime = false;
            endlessVanish = false;
            noPhantomCooldown = false;

            noEndGame = false;
            unlockKillButton = false;

            gameOver = false;
            unlockSkins = false;
            forceReset = false;
            disableReport = false;

            spoofPlatform = false;
            spoofLevel = false;
        }
    }
}
