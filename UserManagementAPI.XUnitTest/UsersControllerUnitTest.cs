using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UserManagementAPI.Controllers;
using UserManagementAPI.Entities;
using UserManagementAPI.Repository;
using UserManagementAPI.Repository.Contracts;
using Xunit;

namespace UserManagementAPI.XUnitTest
{
    public class UsersControllerUnitTest
    {
       

        [Fact]
        public async Task TestGetUsersAsync()
        {
            string failureResult = "";
            try
            {
                // Arrange
                var dbContext = DbContextMocker.UserDbContext();
                var controller = new UserRepository(dbContext);
                // Act
                var response = await controller.FindAll(); 
                dbContext.Dispose();
            }
            catch (Exception ex)
            {
                failureResult = ex.Message;
            }
            // Assert
            // Assert.IsType<OkObjectResult>(value);
            //var result = Assert.IsType<OkObjectResult>(response);
            Assert.Equal("", failureResult);
        }

        [Fact]
        public async Task TestGetUsersbyIdAsync()
        {
            string failureResult = "";
            try
            {
                // Arrange
                var dbContext = DbContextMocker.UserDbContext();
                var controller = new UserRepository(dbContext);
                var id = 1;
                // Act
                var response = await controller.FindById(id);
                dbContext.Dispose();
            }
            catch (Exception ex)
            {
                failureResult = ex.Message;
            }

            // Assert
            //Assert.IsType<OkObjectResult>(response);
            // var result = Assert.IsType<OkObjectResult>(response);
            Assert.Equal("", failureResult);
        }

        [Fact]
        public async Task TestPostUsersAsync()
        {
            string failureResult = "";
            try
            {
                // Arrange
                var dbContext = DbContextMocker.UserDbContext();
                var controller = new UserRepository(dbContext);
                var request = new Users
                {
                    username = "Rohit1",
                    fname = "Rohit",
                    lname = "Swain",
                    Email = "rr@gmail.com",
                    Phone = 7878787877,
                    dob = DateTime.ParseExact("31/05/1998", "dd/MM/yyyy", null),
                    active = true,
                    age = 25,
                    Password = "papun1"
                };

                // Act
                var response = await controller.Create(request);


                dbContext.Dispose();
            }
            catch (Exception ex)
            {
                failureResult = ex.Message;
            }

            // Assert
            //Assert.IsType<OkObjectResult>(response);
            // var result = Assert.IsType<OkObjectResult>(response);
            Assert.Equal("", failureResult);


        }

        [Fact]
        public async Task TestPutUsersAsync()
        {
            string failureResult = "";
            try
            {
                // Arrange
                var dbContext = DbContextMocker.UserDbContext();
                var controller = new UserRepository(dbContext);
                var request = new Users
                {
                    username = "Rohit1",
                    fname = "Rohit",
                    lname = "Swain",
                    Email = "rr@gmail.com",
                    Phone = 7878787877,
                    dob = DateTime.ParseExact("31/05/1998", "dd/MM/yyyy", null),
                    active = true,
                    age = 25,
                    Password = "papun1"
                };

                // Act
                var response = await controller.Update(request);
                dbContext.Dispose();
            }
            catch (Exception ex)
            {
                failureResult = ex.Message;
            }
            // Assert
            //Assert.IsType<OkObjectResult>(response);
            // var result = Assert.IsType<OkObjectResult>(response);
            Assert.Equal("", failureResult);

        }

        [Fact]
        public async Task TestUserDeleteAsync()
        {
            string failureResult = "";
            try
            {
                // Arrange
                var dbContext = DbContextMocker.UserDbContext();
                var controller = new UserRepository(dbContext);
                var request = new Users
                {
                    id = 15,
                    username = "Rohit1",
                    fname = "Rohit",
                    lname = "Swain",
                    Email = "rr@gmail.com",
                    Phone = 7878787877,
                    dob = DateTime.ParseExact("31/05/1998", "dd/MM/yyyy", null),
                    active = true,
                    age = 25,
                    Password = "papun1"
                };

                // Act
                var response = await controller.Delete(request);
                dbContext.Dispose();
            }
            catch (Exception ex)
            {
                failureResult = ex.Message;
            }
            // Assert
            //Assert.IsType<OkObjectResult>(response);
            // var result = Assert.IsType<OkObjectResult>(response);
            Assert.Equal("", failureResult);
        }

        //Roles Test Cases
        [Fact]
        public void TestPostAssignRoleUser()
        {
            string failureResult = "";

        
            try
            {
                // Arrange
                var dbContext = DbContextMocker.UserRoleDbContext();
                var dbUserContext = DbContextMocker.UserDbContext();
                var controller = new UsersRoleRepository(dbUserContext,dbContext);
                var request = new AssignUserRole
                {
                    username = "Rohit1",
                    rolename="user",
                    IsAssign=true
                };

                // Act
                var response = controller.AssignRoleUser(request, null);
                dbContext.Dispose();
            }
            catch (Exception ex)
            {
                failureResult = ex.Message;
            }

            // Assert
            //Assert.IsType<OkObjectResult>(response);
            // var result = Assert.IsType<OkObjectResult>(response);
            Assert.Equal("", failureResult);
        }

        [Fact]
        public void TestUpdaateUsersRole()
        {
            string failureResult = "";
            
            try
            {
                // Arrange
                var dbContext = DbContextMocker.UserRoleDbContext();
                var dbUserContext = DbContextMocker.UserDbContext();
                var controller = new UsersRoleRepository(dbUserContext, dbContext);
                var request = new UpdateAssignUserRole
                {
                    username = "Rohit1",
                    rolename = "user",
                    IsEdit = true
                };

                // Act
                var response = controller.UpdateAssignRoleUser(request,null );
                dbContext.Dispose();
            }
            catch (Exception ex)
            {
                failureResult = ex.Message;
            }

            // Assert
            //Assert.IsType<OkObjectResult>(response);
            // var result = Assert.IsType<OkObjectResult>(response);
            Assert.Equal("", failureResult);

        }

        [Fact]
        public  void TestDeleteUserRole()
        {
            string failureResult = "";
            try
            {
                // Arrange
                var dbContext = DbContextMocker.UserRoleDbContext();
                var dbUserContext = DbContextMocker.UserDbContext();
                var controller = new UsersRoleRepository(dbUserContext, dbContext);
                var request = new DeleteAssignUserRole
                {
                    username = "Rohit1",
                    rolename = "user",
                    IsDelete = true
                };

                // Act
                var response = controller.DeleteAssignRoleUser(request, null);
                dbContext.Dispose();
            }
            catch (Exception ex)
            {
                failureResult = ex.Message;
            }

            // Assert
            //Assert.IsType<OkObjectResult>(response);
            // var result = Assert.IsType<OkObjectResult>(response);
            Assert.Equal("", failureResult);
        }

    }
}
