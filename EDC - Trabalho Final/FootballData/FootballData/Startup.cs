using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FootballData.Startup))]
namespace FootballData
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
