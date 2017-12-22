using System;

namespace BlastAsia.DigiBook.Domain.Models.Departments
{
    public class Department
    {
        public Guid DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public Guid DepartmentHead { get; set; }
    }
}