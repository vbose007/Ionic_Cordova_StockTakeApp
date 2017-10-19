using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup("APIStartup",typeof(MillProApp.API.ApiStartup))]

namespace MillProApp.API
{
    public partial class ApiStartup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
