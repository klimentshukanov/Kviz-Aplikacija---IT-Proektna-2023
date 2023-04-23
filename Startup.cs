using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Kviz_Aplikacija___IT_Proektna_2023.Startup))]
namespace Kviz_Aplikacija___IT_Proektna_2023
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
