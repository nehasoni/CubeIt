using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CubeItMVC.Startup))]
namespace CubeItMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
