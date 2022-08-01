using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Models;

namespace UserManagement.Models
{
    public class DataSeeder
    {
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        public DataSeeder(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task SeedRoles()
        {
            if (!await roleManager.RoleExistsAsync("Customer"))
                await roleManager.CreateAsync(new IdentityRole { Name = "Customer" });
            if (!await roleManager.RoleExistsAsync("Admin"))
                await roleManager.CreateAsync(new IdentityRole { Name = "Admin" });
           
        }
        public async Task SeedAdmin()
        {
            var obj = await userManager.FindByNameAsync("admin@gmail.com");
          
            if (obj == null)
            {
                var admin = new AppUser
                {
                    FirstName="john",
                    LastName = "mathew",
                    UserName= "admin@gmail.com",
                    Email = "admin@gmail.com",
                    PhoneNumber = "1232312123",
                };
                await userManager.CreateAsync(admin, "Admin@123");
                await userManager.AddToRoleAsync(admin, "Admin");
            }
            



        }
    }
}
