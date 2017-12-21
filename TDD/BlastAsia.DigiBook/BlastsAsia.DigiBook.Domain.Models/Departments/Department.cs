using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Models.Departments
{
    public class Department
    {
        public Guid Id { get; set; }
        public string DepartmentName { get; set; }
        public Guid EmployeeId { get; set; }
    }
}
