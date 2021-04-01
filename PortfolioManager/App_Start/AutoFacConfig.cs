using System.Configuration;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using PortfolioManager.Cache;
using PortfolioManager.Repository;
using PortfolioManager.Services;


// ReSharper disable once CheckNamespace
namespace PortfolioManager.App_Start
{
    public class AutoFacConfig
    {
        public static void Register()
        {
            var builder = new ContainerBuilder();
            var config = GlobalConfiguration.Configuration;
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterWebApiFilterProvider(config);
            builder.RegisterWebApiModelBinderProvider();
            builder.Register(c => new PortfolioRepository(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString)).As<IPortfolioRepository>().SingleInstance();
            builder.Register(c => new EquitiesPriceCache()).As<IEquitiesPriceCache>().SingleInstance();
            builder.Register(c => new ProfitLossReportBuilder(c.Resolve<IEquitiesPriceCache>())).As<ProfitLossReportBuilder>().SingleInstance();
            builder.Register(c => new NetWorthReportBuilder(c.Resolve<IEquitiesPriceCache>())).As<NetWorthReportBuilder>().SingleInstance();
            builder.Register(c => new ClientDashboardService(
                c.Resolve<IPortfolioRepository>(), 
                c.Resolve<ProfitLossReportBuilder>(), 
                c.Resolve<NetWorthReportBuilder>())).As<ClientDashboardService>().SingleInstance();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container)); //Set the MVC DependencyResolver
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container); //Set the WebApi DependencyResolver
        }
    }
}