using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Im.Startup))]
namespace Im
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
