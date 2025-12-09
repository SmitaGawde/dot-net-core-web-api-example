using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace JWTRefreshToken.Models
{
    public class Department
    {
        [Key]
        public int DeptId { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Head { get; set; }
        public DateTime EstablishedDate { get; set; }

        // Navigation property for Employees
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
