using System;
using System.Configuration;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MillProApp.Data.Models;

namespace MillProApp.Data.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration :
        //DropCreateDatabaseAlways<ApplicationDbContext>
        DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            //AutomaticMigrationsEnabled = false;
            
        }

        protected override void Seed(ApplicationDbContext context)
        {
            base.Seed(context);
        }

    }
}
