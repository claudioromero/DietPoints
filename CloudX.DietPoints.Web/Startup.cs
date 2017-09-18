using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof (CloudX.DietPoints.Web.Startup))]

namespace CloudX.DietPoints.Web
{
    public partial class Startup
    {
        public virtual void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}