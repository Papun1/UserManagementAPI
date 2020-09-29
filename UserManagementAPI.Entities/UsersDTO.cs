using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace UserManagementAPI.Entities
{
    public class UsersDTO
    {
        [Key]
        public int id { get; set; }
        [Required(ErrorMessage = "Please enter username")]
        public string username { get; set; }
        [Required(ErrorMessage = "Please enter firstname")]
        public string fname { get; set; }
        [Required(ErrorMessage = "Please enter lastname")]
        public string lname { get; set; }
        [Required(ErrorMessage = "Please enter Email"),EmailAddress]
        public string Email { get; set; }
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime dob { get; set; }
        public int age { get; set; }
        [Required(ErrorMessage = "Please enter phone number")]
        public Int64 Phone { get; set; }
        public bool active { get; set; }
        //  public DateTime lastmodifiedon { get; set; }
       
    }
    public class UsersCreateDTO
    {
        [Key]
       
        [Required(ErrorMessage = "Please enter username")]
        public string username { get; set; }
        [Required(ErrorMessage = "Please enter firstname")]
        public string fname { get; set; }
        [Required(ErrorMessage = "Please enter lastname")]
        public string lname { get; set; }
        [Required(ErrorMessage = "Please enter Email"), EmailAddress]
        public string Email { get; set; }
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime dob { get; set; }
        public int age { get; set; }
        [Required(ErrorMessage = "Please enter phone number")]
        public Int64 Phone { get; set; }
        public bool active { get; set; }
        public string Password { get; set; }
    }
    public class UsersUpdateDTO
    {
       [Key]
        public int id { get; set; }
        [Required(ErrorMessage = "Please enter username")]
        public string username { get; set; }
        [Required(ErrorMessage = "Please enter firstname")]
        public string fname { get; set; }
        [Required(ErrorMessage = "Please enter lastname")]
        public string lname { get; set; }
        [Required(ErrorMessage = "Please enter Email"), EmailAddress]
        public string Email { get; set; }
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime dob { get; set; }
        public int age { get; set; }
        [Required(ErrorMessage = "Please enter phone number")]
        public Int64 Phone { get; set; }
        //  public DateTime lastmodifiedon { get; set; }
        public string Password { get; set; }
    }
    public class UsersLoginDTO
    {
        public string username { get; set; }
        public string Password { get; set; }

    }
    public class AssignUserRole
    {
        public string username { get; set; }
        public string rolename { get; set; }
        public bool IsAssign { get; set; }
    }
    public class UpdateAssignUserRole
    {
        public string username { get; set; }
        public string rolename { get; set; }
        public bool IsEdit { get; set; }

    }
    public class DeleteAssignUserRole
    {
        public string username { get; set; }
        public string rolename { get; set; }
        public bool IsDelete { get; set; }

    }
}
