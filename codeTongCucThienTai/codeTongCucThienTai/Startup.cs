using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(codeTongCucThienTai.Startup))]
namespace codeTongCucThienTai
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
