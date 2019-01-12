using Newtonsoft.Json.Serialization;
using System.Web.Http;

namespace GigHub
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //Code for using camel case notifcation in web api
            var settings = GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings;
            //CamelCasePropertyNamesContractResolver in in Newtonsoft.Json.Serialization Namespace
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            settings.Formatting = Newtonsoft.Json.Formatting.Indented; //optional

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
