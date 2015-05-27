using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Ulaznicar.Startup))]
namespace Ulaznicar
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
