using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CardChequeModule.Startup))]
namespace CardChequeModule
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
