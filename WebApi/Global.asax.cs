using System.Web.Http;
using WebApi.Controllers;

namespace WebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            Cron.Iniciar();
        }
    }
}
