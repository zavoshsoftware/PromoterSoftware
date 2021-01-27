using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PromoterSoftware.Startup))]
namespace PromoterSoftware
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
