using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UserManagementAPI.Data;

namespace UserManagementAPI.XUnitTest
{
    public static class DbContextMocker
    {
        public static ApplicationDbContext UserDbContext()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            // var connectionString = configuration.GetConnectionString("DefaultConnection");
            var connectionString = @"Server=DESKTOP-RPP6IHN\SQLEXPRESS;Database=UserManagmentDbTest;Trusted_Connection=True;MultipleActiveResultSets=true";
            builder.UseSqlServer(connectionString);
            return new ApplicationDbContext(builder.Options);
        }
        public static RolesDbContext UserRoleDbContext()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            var builder = new DbContextOptionsBuilder<RolesDbContext>();
            // var connectionString = configuration.GetConnectionString("DefaultConnection");
            var connectionString = @"Server=DESKTOP-RPP6IHN\SQLEXPRESS;Database=UserManagmentDbTest;Trusted_Connection=True;MultipleActiveResultSets=true";
            builder.UseSqlServer(connectionString);
            return new RolesDbContext(builder.Options);
        }
    }
}
