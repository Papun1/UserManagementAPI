using System;
using System.Collections.Generic;
using System.Text;
using UserManagementAPI.Entities;

namespace UserManagementAPI.Repository.Contracts
{
    public interface IUsersRoleRepository
    {
        int AssignRoleUser(AssignUserRole assignUserRole);
        int UpdateAssignRoleUser(UpdateAssignUserRole updateassignUserRole);
        int DeleteAssignRoleUser(DeleteAssignUserRole updateassignUserRole);
    }
}
