using Microsoft.AspNet.Identity.EntityFramework;
using MillProApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MillProApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("name=MillProAppData", throwIfV1Schema: false)
        {
            //Database.SetInitializer<ApplicationDbContext>(new DropCreateDatabaseAlways<ApplicationDbContext>());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<ApplicationRole> IdentityRoles { get; set; }

        public DbSet<Company> Companys { get; set; }

        public DbSet<WorkOrder> WorkOrders { get; set; }

        public DbSet<InventoryItem> InventoryItems { get; set; }

        public DbSet<StockTake> StockTakes { get; set; }
    }
}
