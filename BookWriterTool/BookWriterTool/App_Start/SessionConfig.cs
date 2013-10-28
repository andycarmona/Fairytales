using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookWriterTool.App_Start
{
    public static class SessionConfig
    {
        public static void Session_Start(object sender, EventArgs e)
        {
            HttpContext.Current.Session["culture"] = "en-GB";
        }
    }
}