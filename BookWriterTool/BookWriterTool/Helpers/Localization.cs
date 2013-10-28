using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookWriterTool.Helpers
{
    using System.Globalization;
    using System.Threading;

    public class Localization
    {
        public string Localize(string valueToLocalize)
        {
            string localizedValue = "";
            string lang=(string)HttpContext.Current.Session["culture"];
            if (lang != null)
            {
                switch (lang)
                {
                    case "en-GB":
                        localizedValue = Resources.Resource_en_GB.ResourceManager.GetString(valueToLocalize);
                        break;
                    case "es-ES":
                        localizedValue = Resources.Resource_es_ES.ResourceManager.GetString(valueToLocalize);
                        break;
                    case "sv-SE":
                        localizedValue = Resources.Resource_sv_SE.ResourceManager.GetString(valueToLocalize);
                        break;
                }
            }
            return localizedValue;

        }
    }
}