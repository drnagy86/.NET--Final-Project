using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RubricMVC.Startup))]
namespace RubricMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
