using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using PortfolioManager.App_Start;
using Serilog;

namespace PortfolioManager
{
    public class MvcApplication : HttpApplication
    {
        const string LogFileName = "app.log";
        protected void Application_Start()
        {
            SetUpLogger();
            AutoFacConfig.Register();
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        private static void SetUpLogger()
        {
            var logPath = ConfigurationManager.AppSettings["Logging.Path"] ?? @"logs";
            logPath = Path.IsPathRooted(logPath)
                ? Path.Combine(logPath, LogFileName)
                : Path.Combine(HostingEnvironment.ApplicationPhysicalPath, logPath, LogFileName);
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(path: logPath, rollingInterval: RollingInterval.Day)
                .Enrich.WithMvcRouteTemplate()
                .Enrich.WithMvcRouteData()
                .Enrich.WithMvcControllerName()
                .Enrich.WithMvcActionName()
                .CreateLogger();
        }
    }
}
