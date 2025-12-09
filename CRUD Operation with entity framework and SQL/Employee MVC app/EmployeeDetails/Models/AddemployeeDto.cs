namespace EmployeeAdminPortal.Models.Entities
{
    public class AddemployeeDto
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public string? Phone { get; set; }
        public decimal? salary { get; set; }
    }
}
