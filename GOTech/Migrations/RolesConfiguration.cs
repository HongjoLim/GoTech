using GOTech.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Linq;
using System.Threading.Tasks;
/* 
* Name: Jo Lim
* Date: Apr 2, 2019
* Last Modified: Apr 2, 2019
* Description: This class is used to create application roles and seed admin account
*                   It will be used in 2 cases
*                      1. Migration
*                      2. Database seeding
* */

namespace GOTech.Migrations
{
    public class RolesConfiguration
    {
        public static void ConfigureUserRoles(ApplicationDbContext context)
        {
            var store = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(store);

            // Create "Admin", "Manager", "Employee", and "Customer" roles
            var admin = new IdentityRole("Admin");
            var manager = new IdentityRole("Manager");
            var employee = new IdentityRole("Employee");
            var customer = new IdentityRole("Customer");

            // Check if the "Admin" role exists
            if (!roleManager.RoleExists(admin.Name))
            {
                roleManager.Create(admin);
            }
            
            // Check if the "Manager" role exists
            if (!roleManager.RoleExists(manager.Name))
            {
                roleManager.Create(manager);
            }

            // Check if the "Employee" role exists
            if (!roleManager.RoleExists(employee.Name))
            {
                roleManager.Create(employee);
            }

            // Check if the "Customer" role exists
            if (!roleManager.RoleExists(customer.Name))
            {
                roleManager.Create(customer);
            }

            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            // Seed Admin User
            var adminUser = new ApplicationUser
            {
                UserName = "admin@gotech.com",
                HiringDate = DateTime.Now,
                Email = "admin@gotech.com"
            };

            if(context.Users.FirstOrDefault(x=>x.UserName == "admin@gotech.com") ==null)
            {
                userManager.Create(adminUser, "@Admin1234");
                userManager
                    .AddToRole(adminUser.Id, roleManager.Roles.FirstOrDefault(x => x.Name == "Admin").Name);
            }
        }
    }
}