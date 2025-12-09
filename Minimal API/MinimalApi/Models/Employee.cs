using System;
using System.Collections.Generic;

namespace MinimalApi.Models;

public partial class Employee
{
    public int EmpId { get; set; }

    public string Name { get; set; } = null!;

    public string Position { get; set; } = null!;

    public decimal Salary { get; set; }

    public DateTime DateOfJoining { get; set; }

    public DateTime DateofBirth { get; set; }

    public string Email { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public int DeptId { get; set; }

    public string Password { get; set; } = null!;

    public string Username { get; set; } = null!;

    public virtual Department Dept { get; set; } = null!;
}
