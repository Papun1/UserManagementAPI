using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace UserManagementAPI.Entities
{
    [Table("Users")]
    public class Users
    {
        public int id { get; set; }
        public string username { get; set; }
        public string fname { get; set; }
        public string lname { get; set; }
        public string Email { get; set; }
        public DateTime dob { get; set; }
        public int age { get; set; }
        public Int64 Phone { get; set; }
        public bool active { get; set; }
        //public DateTime lastmodifiedon { get; set; }
        public string Password { get; set; }
    }
    //[Table("User_Roles")]
    //public class User_Roles
    //{
       
    //    public string user_id { get; set; }
    //    public string role_id { get; set; }
      
    //}
}
