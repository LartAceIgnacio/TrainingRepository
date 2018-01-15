using System;

namespace BlastAsia.DigiBook.Domain.Models.Departments
{
    public class Department
    {
        public string DepartmentName { get; set; }
        public Guid DeparmentMemberId { get; set; }
        public Guid DepartmentId { get; set; }
    }
}