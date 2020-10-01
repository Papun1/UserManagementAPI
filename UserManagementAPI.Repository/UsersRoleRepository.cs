using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UserManagementAPI.Data;
using UserManagementAPI.Entities;
using UserManagementAPI.Repository.Contracts;
using Microsoft.AspNetCore.Identity;

namespace UserManagementAPI.Repository
{
    public class UsersRoleRepository : IUsersRoleRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly RolesDbContext _dbRole;

        public UsersRoleRepository(ApplicationDbContext db, RolesDbContext dbRole)
        {
            _db = db;
            _dbRole = dbRole;
        }


        public int AssignRoleUser(AssignUserRole assignUserRole, UserManager<IdentityUser> userManager)
        {

            var param = new SqlParameter[] {
                        new SqlParameter() {
                            ParameterName = "@username",
                            SqlDbType =  System.Data.SqlDbType.VarChar,
                            Size = 100,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = assignUserRole.username.ToString()
                        },
                        new SqlParameter() {
                            ParameterName = "@rolename",
                            SqlDbType =  System.Data.SqlDbType.VarChar,
                            Size = 100,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = assignUserRole.rolename.ToString()
                        },
                        new SqlParameter() {
                            ParameterName = "@IsInsert",
                            SqlDbType =  System.Data.SqlDbType.Bit,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = Convert.ToBoolean(assignUserRole.IsAssign)
                        },

            };
            //    IQueryable<Users> studentList = _dbRole.User_new.FromSqlRaw("EXEC ProcAddRoletoUser @username, @rolename, @IsDelete", param[0].Value, param[1].Value, param[2].Value).IgnoreQueryFilters();
            string SQLQuery = $"EXECUTE ProcAddRoletoUser @username='{param[0].Value}', @rolename='{param[1].Value}', @IsInsert={param[2].Value}";

            int Uid = _dbRole.Database.ExecuteSqlRaw(SQLQuery);
            if(userManager !=null)
            {
                SeedCreateUsers(userManager, assignUserRole).Wait();
            }

            
            //_dbRole.User_new.ExecuteSqlCommand("EXEC ProcAddRoletoUser @username, @rolename, @IsDelete", username.Value, rolename.Value, IsInsert.Value);
            return Uid;
        }

        public int DeleteAssignRoleUser(DeleteAssignUserRole deleteassignUserRole, UserManager<IdentityUser> userManager)
        {
            var param = new SqlParameter[] {
                        new SqlParameter() {
                            ParameterName = "@username",
                            SqlDbType =  System.Data.SqlDbType.VarChar,
                            Size = 100,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = deleteassignUserRole.username.ToString()
                        },
                        new SqlParameter() {
                            ParameterName = "@rolename",
                            SqlDbType =  System.Data.SqlDbType.VarChar,
                            Size = 100,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = deleteassignUserRole.rolename.ToString()
                        },
                        new SqlParameter() {
                            ParameterName = "@IsDelete",
                            SqlDbType =  System.Data.SqlDbType.Bit,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = Convert.ToBoolean(deleteassignUserRole.IsDelete)
                        },
            };
            //    IQueryable<Users> studentList = _dbRole.User_new.FromSqlRaw("EXEC ProcAddRoletoUser @username, @rolename, @IsDelete", param[0].Value, param[1].Value, param[2].Value).IgnoreQueryFilters();
            string SQLQuery = $"EXECUTE ProcAddRoletoUser @username='{param[0].Value}', @rolename='{param[1].Value}', @IsDelete={param[2].Value}";

            int result = _dbRole.Database.ExecuteSqlRaw(SQLQuery);
            if (userManager != null)
            {
                DeleteSeedUsers(userManager, deleteassignUserRole).Wait();
            }
            return result;
        }

        public int UpdateAssignRoleUser(UpdateAssignUserRole updateassignUserRole, UserManager<IdentityUser> userManager)
        {
            var param = new SqlParameter[] {
                        new SqlParameter() {
                            ParameterName = "@username",
                            SqlDbType =  System.Data.SqlDbType.VarChar,
                            Size = 100,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = updateassignUserRole.username.ToString()
                        },
                        new SqlParameter() {
                            ParameterName = "@rolename",
                            SqlDbType =  System.Data.SqlDbType.VarChar,
                            Size = 100,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = updateassignUserRole.rolename.ToString()
                        },
                        new SqlParameter() {
                            ParameterName = "@IsEdit",
                            SqlDbType =  System.Data.SqlDbType.Bit,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = Convert.ToBoolean(updateassignUserRole.IsEdit)
                        },
            };
            //    IQueryable<Users> studentList = _dbRole.User_new.FromSqlRaw("EXEC ProcAddRoletoUser @username, @rolename, @IsDelete", param[0].Value, param[1].Value, param[2].Value).IgnoreQueryFilters();
            string SQLQuery = $"EXECUTE ProcAddRoletoUser @username='{param[0].Value}', @rolename='{param[1].Value}', @IsEdit={param[2].Value}";

            int v = _dbRole.Database.ExecuteSqlRaw(SQLQuery);
            if (userManager != null)
            {
                UpdateSeedUsers(userManager, updateassignUserRole).Wait();
            }
            return v;
        }
        public async Task<IList<Users>> FindByUserName(string Username)
        {
            var Users = await _db.Users.Where(x => x.username == Username).ToListAsync();
            return Users;
        }
        private async Task SeedCreateUsers(UserManager<IdentityUser> userManager, AssignUserRole assignUsername)
        {

            IList<Users> Users = await FindByUserName(assignUsername.username);
           
            if (await userManager.FindByNameAsync(assignUsername.username) == null)
            {
                var user = new IdentityUser
                {
                    UserName = assignUsername.username,
                    Email = Users[0].Email
                };
                var result = await userManager.CreateAsync(user, Users[0].Password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, assignUsername.rolename);
                }
            }
            

        }
        private async static Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync("admin"))
            {
                var role = new IdentityRole
                {
                    Name = "admin"
                };
                await roleManager.CreateAsync(role);
            }
            if (!await roleManager.RoleExistsAsync("user"))
            {
                var role = new IdentityRole
                {
                    Name = "user"
                };
                await roleManager.CreateAsync(role);
            }
        }
        private async Task UpdateSeedUsers(UserManager<IdentityUser> userManager, UpdateAssignUserRole updateassignUsername)
        {
            IList<Users> Users = await FindByUserName(updateassignUsername.username);

            if (await userManager.FindByNameAsync(updateassignUsername.username) != null)
            {
                var user = new IdentityUser
                {
                    UserName = updateassignUsername.username,
                    Email = Users[0].Email,
 
                };
                var result = await userManager.FindByNameAsync(updateassignUsername.username);
                if (result != null)
                {
                    IdentityResult deletionResult = await userManager.RemoveFromRolesAsync(result, await userManager.GetRolesAsync(result));
                    if (deletionResult != null)
                    {
                        await userManager.AddToRoleAsync(result, updateassignUsername.rolename);
                    }
                }
            }

        }

        private async Task DeleteSeedUsers(UserManager<IdentityUser> userManager, DeleteAssignUserRole deleteassignUsername)
        {
            IList<Users> Users = await FindByUserName(deleteassignUsername.username);

            if (await userManager.FindByNameAsync(deleteassignUsername.username) != null)
            {
                var user = new IdentityUser
                {
                    UserName = deleteassignUsername.username,
                    Email = Users[0].Email
                };
                var result = await userManager.FindByNameAsync(deleteassignUsername.username);
                if (result != null)
                {
                    IdentityResult deletionResult = await userManager.RemoveFromRoleAsync(result, deleteassignUsername.rolename);
                    IdentityResult deletionUser = await userManager.DeleteAsync(result);

                }
            }

        }
    }
}
