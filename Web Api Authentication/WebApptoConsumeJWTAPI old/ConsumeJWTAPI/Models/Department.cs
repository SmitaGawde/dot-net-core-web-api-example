using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ConsumeJWTAPI.Models;

[Table("Department")]
public partial class Department
{
    [Key]
    public int DeptId { get; set; }

    public string Name { get; set; } = null!;

    public string Location { get; set; } = null!;

    public string Head { get; set; } = null!;

    public DateTime EstablishedDate { get; set; }

    [InverseProperty("Dept")]
    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
