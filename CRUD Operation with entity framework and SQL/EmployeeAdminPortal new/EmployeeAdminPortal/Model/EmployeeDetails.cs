using System.ComponentModel.DataAnnotations;

namespace EmployeeAdminPortal.Model
{
    public class EmployeeDetails
    {
        [Key]
        public  int ID { get; set; } 
        public required string Name { get; set; }
        public required string Email { get; set; }
        public string Phone { get; set; }
        public Decimal Salary { get; set; }
    }
}
