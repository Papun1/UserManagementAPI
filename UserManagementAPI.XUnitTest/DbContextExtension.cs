using System;
using System.Collections.Generic;
using System.Text;
using UserManagementAPI.Data;
using UserManagementAPI.Entities;

namespace UserManagementAPI.XUnitTest
{
    public static class DbContextExtension
    {
        public static void Seed(this ApplicationDbContext dbContext)
        {
            dbContext.Users.Add(new Users
            {
                username="Snigdha18",
                fname="Snigdha",
                lname="Swain",
                Email="snig@gmail.com",
                Phone=78787878,
                dob= DateTime.ParseExact("31/05/1998", "dd/MM/yyyy", null),
                active =true,
                age=5,
                Password="Papun1"
            });
            dbContext.Users.Add(new Users
            {
                username = "Rinky1",
                fname = "Rinky",
                lname = "Swain",
                Email = "Rink@gmail.com",
                Phone = 787878789,
                dob = DateTime.ParseExact("31/05/1998", "dd/MM/yyyy", null),
                active = true,
                age = 20,
                Password = "papun1"
            });
            dbContext.SaveChanges();
        }
    }
}
