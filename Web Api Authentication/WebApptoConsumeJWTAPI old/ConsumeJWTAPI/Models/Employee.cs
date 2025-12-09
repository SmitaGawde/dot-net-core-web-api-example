using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ConsumeJWTAPI.Models;

[Table("Employee")]
[Index("DeptId", Name = "IX_Employee_departmentDeptId")]
public partial class Employee
{
    [Key]
    [Column("EmpID")]
    public int EmpId { get; set; }

    public string Name { get; set; } = null!;

    public string Position { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Salary { get; set; }

    public DateTime DateOfJoining { get; set; }

    [Column("dateofBirth")]
    public DateTime DateofBirth { get; set; }

    public string Email { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public int DeptId { get; set; }

    public string Password { get; set; } = null!;

    public string Username { get; set; } = null!;

    [ForeignKey("DeptId")]
    [InverseProperty("Employees")]
    public virtual Department Dept { get; set; } = null!;
}
