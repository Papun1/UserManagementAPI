using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using UserManagementAPI.Entities;

namespace UserManagementAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Users> Users { get; set; }
        public DbSet<Audit_logs> audit_Logs { get; set; }
       // public DbSet<User_Roles> user_Roles { get; set; }
        //   public DbSet<UsersLoginDTO> usersloginDTO { get; set; }

    }
   
}
