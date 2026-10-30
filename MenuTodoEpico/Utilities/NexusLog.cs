using BepInEx.Logging;
namespace NexusMenu
{
    public class NexusLog
    {
        // reference: https://docs.bepinex.dev/articles/dev_guide/plugin_tutorial/3_logging.html
        // got me crying that this is what the logger is lol. I thought it was gonna be more complicated
        public static void LogInfo(string message) 
        {
            var logger = BepInEx.Logging.Logger.CreateLogSource("NexusLog");
            logger.LogInfo(message);
        }
        public static void LogWarning(string message)
        {
            var logger = BepInEx.Logging.Logger.CreateLogSource("NexusLog");
            logger.LogWarning(message);
        }
        public static void LogError(string message)
        {
            var logger = BepInEx.Logging.Logger.CreateLogSource("NexusLog");
            logger.LogError(message);
        }
    }
}
