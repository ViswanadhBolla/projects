using System;
using System.Collections.Generic;

namespace EmployeeAPI.Models
{
    public partial class Employee
    {
        public int Eid { get; set; }
        public string? Ename { get; set; }
        public int? Age { get; set; }
        public string? City { get; set; }
        public int? Deptid { get; set; }

        public virtual Department? Dept { get; set; }
    }
}
