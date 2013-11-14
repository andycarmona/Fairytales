using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookWriterTool.Helpers
{

    public static class GlobalVariables
    {
        const bool  LocalEnvironment=true ;//false when running against resources on server 

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
    }
}