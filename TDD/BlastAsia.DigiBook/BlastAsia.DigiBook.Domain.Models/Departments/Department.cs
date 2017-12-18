using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Models.Departments
{
    public class Department
    {
        public Guid DeparmentId { get; set; }
        public string DepartmentName { get; set; }

        public Guid DepartmentHeadId { get; set; }
    }
}
