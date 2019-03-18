using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GOTech.Startup))]
namespace GOTech
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
