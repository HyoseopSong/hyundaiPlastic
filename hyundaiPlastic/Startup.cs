using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(hyundaiPlastic.Startup))]
namespace hyundaiPlastic
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
