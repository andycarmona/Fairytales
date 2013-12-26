using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookWriterTool.Helpers
{

    public static class GlobalVariables
    {
        const bool LocalEnvironment = true;//false when running against resources on server 

        public static string ConfigResource(string value)
        {
            if (LocalEnvironment == true)
            {
                return Resources.LocalConfig.ResourceManager.GetString(value);
            }
            else
            {
                return Resources.RemoteConfig.ResourceManager.GetString(value);
            }
        }

        public static Dictionary<string, List<string[]>> GenericCharacter2Directory()
        {
            var genericCharacter2Directory = new Dictionary<string, List<string[]>>();
            var anime = new string[] { "female", "male" };
            genericCharacter2Directory.Add("anime", new List<string[]>() { anime });
            return genericCharacter2Directory;
        }

        public static List<string[]> GenericCharacterDirectory()
        {

            var background = new string[] { };
            var character = new string[] { "animals", "expressions" };

            var genericCharacterDirectory = new List<string[]>() { character };
            return genericCharacterDirectory;
        }
    }
}