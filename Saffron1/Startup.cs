using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Saffron1.Startup))]
namespace Saffron1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
