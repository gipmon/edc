using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EDC_Trabalho2.Startup))]
namespace EDC_Trabalho2
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
