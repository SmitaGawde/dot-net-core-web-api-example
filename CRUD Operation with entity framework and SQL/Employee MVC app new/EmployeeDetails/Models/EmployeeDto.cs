namespace EmployeeDetails.Models
{
    public class EmployeeDto
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public string Phone { get; set; }
        public Decimal Salary { get; set; }
    }
}
