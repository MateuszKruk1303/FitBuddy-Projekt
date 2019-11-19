using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace FitBuddy
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "FitBuddy", action="Welcome", id = UrlParameter.Optional }
            );

            routes.MapRoute(
            "MyRoute", // Route name
            "FitBuddy/Confirm/{MyString}", // URL with parameters
            new { controller = "FitBuddy", action = "Confirm", MyString = UrlParameter.Optional } // Parameter defaults
        );
        }
    }
}
