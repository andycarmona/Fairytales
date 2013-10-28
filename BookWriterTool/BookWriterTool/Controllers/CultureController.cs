using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookWriterTool.Controllers
{
    public class CultureController : Controller
    {
        //
        // GET: /Culture/

        public ActionResult SetCulture(string culture)
        {
            var httpSessionStateBase = this.HttpContext.Session;
            if (httpSessionStateBase != null)
            {
                httpSessionStateBase["culture"] = culture;
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
