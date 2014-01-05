using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BookWriterTool
{
    using System.Web.Http;

    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
              name: "Book",
              url: "{controller}/{action}/{id}",
              defaults: new { controller = "Book", action = "Index", id = UrlParameter.Optional }
          );
            routes.MapRoute(
              name: "Page",
              url: "{controller}/{action}/{id}",
              defaults: new { controller = "Book", action = "AddPage", id = UrlParameter.Optional }
          );
            routes.MapRoute(
              name: "Preview",
              url: "{controller}/{action}/{id}",
              defaults: new { controller = "Book", action = "ViewBookFlip", id = UrlParameter.Optional }
          );
            routes.MapHttpRoute(
    name: "ActionApi",
    routeTemplate: "api/{controller}/{action}/{userName}",
    defaults: new { userName = RouteParameter.Optional }
);
            routes.MapHttpRoute(
name: "Api model",
routeTemplate: "api/{controller}/{action}/{userName}/{fileName}",
defaults: new { userName = RouteParameter.Optional, fileName = RouteParameter.Optional }
);
        }
    }
}