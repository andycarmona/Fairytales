using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookWriterTool.Controllers
{
    using System.Globalization;
    using System.Reflection;
    using System.Resources;
    using System.Threading;

    using BookWriterTool.Helpers;

    public class HomeController : Controller
    {
        private ResourceManager aManager;

        private Localization localizationHandler;

        public HomeController()
        {
          localizationHandler=new Localization();
          
        }

        public ActionResult Index()
        {  
            ViewBag.message = localizationHandler.Localize("ChooseOption");
       
            return View();
        }

    }
}
