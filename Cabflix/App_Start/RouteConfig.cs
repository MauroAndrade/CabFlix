using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Cabflix
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            routes.MapRoute(
                name: "usuario",
                url: "Usuario",
                defaults: new { controller = "Usuario", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "painel",
                url: "Painel",
                defaults: new { controller = "Painel", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
              name: "relatorio",
              url: "Relatorio",
              defaults: new { controller = "Detalhamento", action = "Relatorio", id = UrlParameter.Optional }
          );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Login", action = "Login", id = UrlParameter.Optional }
            );


        }
    }
}
