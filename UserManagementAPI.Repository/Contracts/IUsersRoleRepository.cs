using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using UserManagementAPI.Entities;

namespace UserManagementAPI.Repository.Contracts
{
    public interface IUsersRoleRepository
    {
        int AssignRoleUser(AssignUserRole assignUserRole, UserManager<IdentityUser> userManager);
        int UpdateAssignRoleUser(UpdateAssignUserRole updateassignUserRole, UserManager<IdentityUser> userManager);
        int DeleteAssignRoleUser(DeleteAssignUserRole updateassignUserRole, UserManager<IdentityUser> userManager);
    }
}
