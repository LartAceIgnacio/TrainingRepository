using System;
using BlastAsia.DigiBook.Domain.Models.Departments;

namespace BlastAsia.DigiBook.Domain.Departments
{
    public class DepartmentService
    {
        private IDepartmentRepository departmentRepo;

        public DepartmentService(IDepartmentRepository departmentRepo)
        {
            this.departmentRepo = departmentRepo;
        }

        public Department Save(Department department)
        {
            return departmentRepo.Create(department);
        }
    }
}