using System;
using System.Collections.Generic;

namespace WebApplication1.db;

public partial class Employee
{
    public int Employeeid { get; set; }

    public string? Firstname { get; set; }

    public string? Lastname { get; set; }

    public string? Department { get; set; }
}
