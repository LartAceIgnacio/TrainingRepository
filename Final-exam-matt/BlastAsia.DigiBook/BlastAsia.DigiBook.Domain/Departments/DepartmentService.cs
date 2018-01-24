using System;
using BlastAsia.DigiBook.Domain.Models.Departments;

namespace BlastAsia.DigiBook.Domain.Departments
{
    public class DepartmentService : IDepartmentService
    {
        private IDepartmentRepository departmentRepository;

        public DepartmentService(IDepartmentRepository departmentRepository)
        {
            this.departmentRepository = departmentRepository;
        }

        public Department Save(Guid id, Department department)
        {
            Department result = null;
            var found = departmentRepository.Retrieve(department.Id);
            result = departmentRepository.Create(found);
            return result;
        }
    }
}