using Owin;
using Microsoft.Owin;
[assembly: OwinStartup(typeof(Project.Startup))]
namespace Project
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