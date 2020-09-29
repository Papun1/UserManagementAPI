using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagementAPI.Data;
using UserManagementAPI.Entities;
using UserManagementAPI.Repository.Contracts;

namespace UserManagementAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;

        public UserRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<bool> Create(Users entity)
        {
            
                await _db.Users.AddAsync(entity);
            
            return await Save();
        }

        public async Task<bool> Delete(Users entity)
        {
            _db.Users.Remove(entity);
            return await Save();
        }

        public async Task<IList<Users>> FindAll()
        {
            var Users = await _db.Users.ToListAsync();
            return Users;
        }

        public async Task<Users> FindById(int id)
        {
            var Users = await _db.Users.FindAsync(id);
            return Users;
        }
       

        public async Task<bool> isExist(int id)
        {
            return await _db.Users.AnyAsync(q => q.id == id);
        }

        public async Task<bool> Save()
        {
            var changes = await _db.SaveChangesAsync();
            return changes > 0;
        }
        public async Task<IList<Users>> FindByUserName(string Username)
        {
            var Users = await _db.Users.Where(x => x.username == Username).ToListAsync();
            return Users;
        }

        public async Task<bool> Update(Users entity)
        {
            _db.Users.Update(entity);
            return await Save();
        }
    
       
    }
}
