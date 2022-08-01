using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Models;

namespace UserManagement
{
    public class UserManagementContext:IdentityDbContext<AppUser>
    {
        public UserManagementContext(DbContextOptions options) : base(options) { }
    }
}
