using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AccountManagement.Startup))]
namespace AccountManagement
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
