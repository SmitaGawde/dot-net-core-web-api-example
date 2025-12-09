using System.ComponentModel.DataAnnotations;

namespace EmployeeDetails.Models
{
    public class Employee
    {
        [Key]
        public  int ID { get; set; } 
        public required string Name { get; set; }
        public required string Email { get; set; }
        public string Phone { get; set; }
        public Decimal Salary { get; set; }
    }
}
