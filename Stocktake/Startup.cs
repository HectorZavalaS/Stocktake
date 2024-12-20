using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Stocktake.Startup))]
namespace Stocktake
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
