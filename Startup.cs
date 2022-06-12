using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GrabFit2.Startup))]
namespace GrabFit2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
