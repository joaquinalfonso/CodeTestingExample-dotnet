using System.Web.Http;
using WebApiCrud.Controllers;

namespace WebApiCrud
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
