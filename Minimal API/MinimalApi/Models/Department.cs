using System;
using System.Collections.Generic;

namespace MinimalApi.Models;

public partial class Department
{
    public int DeptId { get; set; }

    public string Name { get; set; } = null!;

    public string Location { get; set; } = null!;

    public string Head { get; set; } = null!;

    public DateTime EstablishedDate { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
