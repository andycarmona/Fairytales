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
    using System.Web.Routing;

    using BookWriterTool.Helpers;
    using BookWriterTool.Models;
    using BookWriterTool.Repositories;

    public class HomeController : Controller
    {

        //private Localization localizationHandler;

        public HomeController()
            : this(new BookRepository())
        {

        }
        public HomeController(IBookRepository bookRepository)
        {
    
           

        }
        [Authorize]
        public ActionResult Index(string user)
        {
          //  this.ViewBag.message = this.localizationHandler.Localize("ChooseOption");

            //return this.View();
            return RedirectToAction("EditBook", "Book");
        }
    }
}
