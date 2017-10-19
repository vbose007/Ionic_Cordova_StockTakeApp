using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Http;
using MillProApp.API;
using Autofac.Integration.WebApi;
using Autofac;
using Autofac.Integration.Mvc;
using MillProApp.Data.Repositories;
using MillProApp.API.Services;
using MillProApp.API.Services.Interfaces;
using MillProApp.Data;


namespace MillProApp.UserAdmin
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //ServiceMapperConfig.Init();
            MapperConfig.ConfigureMappings();
        }
    }
}
