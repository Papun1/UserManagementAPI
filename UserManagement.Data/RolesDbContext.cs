using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using UserManagementAPI.Entities;

namespace UserManagementAPI.Data
{
   public class RolesDbContext:DbContext
    {
        public RolesDbContext(DbContextOptions<RolesDbContext> options)
            : base(options)
        {

        }
        public DbSet<Users> User_new { get; set; }
    }
}
