using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using MillProApp.Data.Models;
using MillProApp.Data;

namespace MillProApp.UserAdmin
{
    public class AccountDbInitializer
    {
        public static void Seed(ApplicationDbContext context)
        {
            //var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();

            //var roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(context)); //new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(context));

            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context)); //new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            var roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(context)); //new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(context));

            userManager.SetupValidatorConfigurations();


            if (!roleManager.RoleExists(RoleTypes.Admin.ToString()))

            {

                var roleresult = roleManager.Create(new ApplicationRole(RoleTypes.Admin.ToString()));

            }

            if (!roleManager.RoleExists(RoleTypes.User.ToString()))

            {

                var roleresult = roleManager.Create(new ApplicationRole(RoleTypes.User.ToString()));

            }

            var companyEmail = ConfigurationManager.AppSettings.Get("DefaultAdminCompanyEmail");
            var companyName = ConfigurationManager.AppSettings.Get("DefaultAdminCompanyName");

            var company = context.Companys.FirstOrDefault(c => c.Email.Equals(companyEmail, StringComparison.OrdinalIgnoreCase));

            if (company == null)
            {
                company = new Company()
                {
                    Email = companyEmail,
                    Name = companyName
                };

                company = context.Companys.Add(company);
                context.SaveChanges();

            }


            //Create default admin user
            var userName = ConfigurationManager.AppSettings.Get("DefaultAdminUserName");
            var password = ConfigurationManager.AppSettings.Get("DefaultAdminPassword");
            var firstName = "Admin";
            var lastName = "Admin";
            var role = RoleTypes.Admin.ToString();

            CreateUser(userManager, userName, password, role, company.Id, firstName ,lastName);

            //Create default non-admin user
            userName = ConfigurationManager.AppSettings.Get("DefaultUserName");
            password = ConfigurationManager.AppSettings.Get("DefaultUserPassword");
            firstName = "Dummy";
            lastName = "User";
            role = RoleTypes.User.ToString();

            CreateUser(userManager, userName, password, role, company.Id, firstName, lastName);


        }


        private static void CreateUser(UserManager<ApplicationUser> userManager, string userName, string password, string role, int companyId, string firstName, string lastName)
        {

            var user = userManager.FindByName(userName);

            if (user == null)
            {
                user = new ApplicationUser()
                {
                    UserName = userName,
                    Email = userName,
                    FirstName = firstName,
                    LastName = lastName,
                    CompanyId = companyId,
                    EmailConfirmed = true
                };

                var userResult = userManager.Create(user, password);

                user = userManager.FindByName(user.UserName);

                var roleResult = userManager.AddToRole(user.Id, role);
            }
        }
    }
}