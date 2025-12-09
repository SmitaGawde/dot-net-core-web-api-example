using Azure.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JWTRefreshToken.Models
{
    public class Employee
    {
        [Key]
        public int EmpID { get; set; }
        [Required]
        public string Name { get; set; }
        public string Position { get; set; }
        public decimal Salary { get; set; }
        public DateTime DateOfJoining { get; set; }
        public DateTime dateofBirth { get; set; }

        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
       
        public string Password { get; set; } 


        public int DeptId   { get; set; }


        public Department department { get; set; } = null!;
    }
}
