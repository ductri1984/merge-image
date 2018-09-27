using System.Web.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace APIBusiness
{
    public class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}"
            );

            config.Formatters.JsonFormatter.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;     
        }
    }
}





