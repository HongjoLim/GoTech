using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace GOTech
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapMvcAttributeRoutes();

            // Customers controller do not use id parameter. Instead, it uses email parameter (string)
            routes.MapRoute(
                name: "Customer",
                url: "External/{action}",
                defaults: new { controller = "Customers", action = "Index" }
            );

            // Employees controller do not use id parameter. Instead, it uses email parameter (string)
            routes.MapRoute(
                name: "Employee",
                url: "Internal/{action}",
                defaults: new { controller = "Employees", action = "Index"}
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
