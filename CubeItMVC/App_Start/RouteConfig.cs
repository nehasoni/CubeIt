using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CubeItMVC
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "User",
                url: "user",
                defaults: new { controller = "User", action = "CreateUser" }
            );
            routes.MapRoute(
           name: "getcube",
           url: "user/{userid}/cube",
           defaults: new { controller = "Cube", action = "GetAllCubes" },
           constraints: new { HttpMethod = new HttpMethodConstraint(new[] { "GET" }) }


       );
            routes.MapRoute(
             name: "getcontent",
             url: "user/{userid}/content",
             defaults: new { controller = "Cube", action = "GetAllContents" },
             constraints: new { HttpMethod = new HttpMethodConstraint(new[] { "GET" }) }
         );
            routes.MapRoute(
                name: "createcube",
                url: "user/{userid}/cube",
                defaults: new { controller = "Cube", action = "CreateCube" }
            );
            routes.MapRoute(
                name: "createcontent",
                url: "user/{userid}/content",
                defaults: new { controller = "Cube", action = "CreateContent" }
            );
            routes.MapRoute(
               name: "addcontent",
               url: "user/{userid}/cube/{cubeid}/content",
               defaults: new { controller = "Cube", action = "AddContentToCube" }
           );
            routes.MapRoute(
             name: "sharecontent",
             url: "user/{userid}/content/{contentid}/share",
             defaults: new { controller = "Cube", action = "ShareContent" }
         );
            routes.MapRoute(
               name: "deletecontent",
               url: "user/{userid}/cube/{cubeid}/content/{contentid}",
               defaults: new { controller = "Cube", action = "DeleteContent" },
               constraints: new { HttpMethod = new HttpMethodConstraint(new[] { "GET" })}
           );
            routes.MapRoute(
               name: "deletecube",
               url: "user/{userid}/cube/{cubeid}",
               defaults: new { controller = "Cube", action = "DeleteCube" },
               constraints: new { HttpMethod = new HttpMethodConstraint(new[] { "GET" }) }
           );
           
            routes.MapRoute(
              name: "sharecube",
              url: "user/{userid}/cube/{cubeid}/share",
              defaults: new { controller = "Cube", action = "ShareCube" }
          );
           
          
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            
        }
    }
}
