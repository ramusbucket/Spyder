using Owin;
using Microsoft.Owin;
[assembly: OwinStartup(typeof(EMS.Web.Worker.MongoSaver.Startup))]
namespace EMS.Web.Worker.MongoSaver
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Any connection or hub wire up and configuration should go here
            app.MapSignalR();
        }
    }
}