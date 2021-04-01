using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(PortfolioManager.Startup))]

namespace PortfolioManager
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
