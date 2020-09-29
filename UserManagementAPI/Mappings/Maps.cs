using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagementAPI.Entities;

namespace UserManagementAPI.Mappings
{
    /// <summary>
    /// Mapping Entities
    /// </summary>
    public class Maps:Profile
    {
        /// <summary>
        /// Constructor for mapping
        /// </summary>
        public Maps()
        {
            CreateMap<Users, UsersDTO>().ReverseMap();
            CreateMap<Users, UsersCreateDTO>().ReverseMap();
            CreateMap<Users, UsersUpdateDTO>().ReverseMap();
            CreateMap<Users, AssignUserRole>().ReverseMap();
            CreateMap<Users, UpdateAssignUserRole>().ReverseMap();
            CreateMap<Users, DeleteAssignUserRole>().ReverseMap();
           

        }
    }
}
