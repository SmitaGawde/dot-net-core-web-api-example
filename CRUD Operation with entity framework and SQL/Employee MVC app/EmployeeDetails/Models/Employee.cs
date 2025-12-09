namespace EmployeeDetails.Models
{
    public class Employee
    {
        public Guid id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public string? Phone { get; set; }
        public decimal? salary { get; set; }
    }
}
