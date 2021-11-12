using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace Boss_Timer_Overlay.ManagerClasses
{
    public static class IoManager
    {
        public static string JsonFromFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return string.Empty;
            }

            try
            {
                return File.ReadAllText(filePath);
            }
            catch (Exception)
            {
                // todo: Handle error in the case where loading a json string from a file fails
                throw;
            }
            return string.Empty;
        }

        public static JObject SerializedJsonObject(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return null;
            try
            {
                return JObject.Parse(json);
            }
            catch (Exception)
            {
                // todo: Handle error in the case where parsing a json string into a JObject fails
                throw;
            }

            return null;
        }

        public static JObject JObjectFromFile(string filePath)
        {
            return SerializedJsonObject(JsonFromFile(filePath));
        }

        public static JObject JObjectFromArgs()
        {
            var args = Environment.GetCommandLineArgs();

            if (args.Length <= 1)
                return null;

            // This assumes that the first user-supplied argument to this program is the path to a valid json file
            return SerializedJsonObject(JsonFromFile(args[1]));
        }
    }
}
