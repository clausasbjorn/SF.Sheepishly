using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using Owin;

namespace Sheepishly.Ingest
{
    public class Startup : IOwinAppBuilder
    {
        public static void ConfigureFormatters(MediaTypeFormatterCollection formatters)
        {
            formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
        }

        public void Configuration(IAppBuilder appBuilder)
        {
            var config = new HttpConfiguration();

            config.MapHttpAttributeRoutes();
            ConfigureFormatters(config.Formatters);

            appBuilder.UseWebApi(config);
        }
    }
}
