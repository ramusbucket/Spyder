﻿using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;
using System.Web.Http;
[assembly: OwinStartup(typeof(EMS.Web.KafkaSavers.Startup))]
namespace EMS.Web.KafkaSavers
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = GlobalConfiguration.Configuration;

            this.ConfigureAuth(app);

            app.UseCors(CorsOptions.AllowAll);
            app.UseWebApi(config);
        }
    }
}
