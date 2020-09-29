using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using UserManagementAPI.Data;
using UserManagementAPI.Entities;
using UserManagementAPI.Mappings;
using UserManagementAPI.Repository;
using UserManagementAPI.Repository.Contracts;

namespace UserManagementAPI
{
    /// <summary>
    /// Project Start UP
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Project Start UP Constructor
        /// </summary>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        /// <summary>
        /// Configuration
        /// </summary>
        public IConfiguration Configuration { get; }
        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(
                   Configuration.GetConnectionString("DefaultConnection")));
            services.AddDbContext<RolesDbContext>(options =>
              options.UseSqlServer(
                  Configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>() ;


            services.AddCors(o => {
                o.AddPolicy("CrosPolicy",
                    builder => builder.AllowAnyOrigin().
                    AllowAnyMethod().
                    AllowAnyHeader());
            });
            services.AddAutoMapper(typeof(Maps));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(o => {
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["jwt:Issuer"],
                        ValidAudience = Configuration["jwt:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["jwt:Key"]))
                    };
                });
        

            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "User Management API",
                    Version = "v1",
                    Description = "User Management"
                });
                var xfile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xpath = Path.Combine(AppContext.BaseDirectory, xfile);
                c.IncludeXmlComments(xpath);
            });
            services.AddSingleton<ILoggerService, LoggerService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAudit_logs, AuditService>();
            services.AddScoped<IUsersRoleRepository, UsersRoleRepository>();
            services.AddControllers();
        }
        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="userManager"></param>
        /// <param name="roleManager"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "User Management API");
                //c.RoutePrefix = "";
            });
           
            app.UseHttpsRedirection();
            //    app.UseStaticFiles();
            app.UseCors("CrosPolicy");
           // SeedData.Seed(userManager, roleManager).Wait();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
