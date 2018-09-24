using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ABadRestAPI_IQVIA.Startup))]
namespace ABadRestAPI_IQVIA
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
