using System.Web.Http;
using WebApiNHibernateCrudPagination.Seeds;

namespace WebApiNHibernateCrudPagination
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            DbSeeder.Seed();
        }
    }
}
