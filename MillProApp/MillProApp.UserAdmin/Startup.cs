using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Microsoft.Owin;
using MillProApp.API;
using MillProApp.API.Services;
using MillProApp.API.Services.Interfaces;
using MillProApp.Data;
using MillProApp.Data.Models;
using MillProApp.Data.Repositories;
using Owin;
using System;
using System.Web.Http;
using System.Web.Mvc;

[assembly: OwinStartupAttribute(typeof(MillProApp.UserAdmin.Startup))]
namespace MillProApp.UserAdmin
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);


            var container = new ContainerBuilder();

            container.RegisterControllers(AppDomain.CurrentDomain.GetAssemblies());
            container.RegisterApiControllers(AppDomain.CurrentDomain.GetAssemblies());

            container.RegisterType<StockTakeRepository>().As<IStockTakeRepository>();
            container.RegisterType<StockTakeService>().As<IStockTakeService>();

            var builder = container.Build();// RegisterServices.Register(builder);


            DependencyResolver.SetResolver(new AutofacDependencyResolver(builder));

            var dependencyResolver = new AutofacWebApiDependencyResolver(builder);
            GlobalConfiguration.Configuration.DependencyResolver = dependencyResolver;

            app.UseAutofacMiddleware(builder);

            app.UseAutofacMvc();


            using (var context = new ApplicationDbContext())
            {
                AccountDbInitializer.Seed(context);
            }
        }
    }
}
