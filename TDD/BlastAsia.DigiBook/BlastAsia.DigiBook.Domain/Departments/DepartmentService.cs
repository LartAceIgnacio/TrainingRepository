using System;
using BlastAsia.DigiBook.Domain.Models.Departments;

namespace BlastAsia.DigiBook.Domain.Departments
{
    public class DepartmentService
    {
        private readonly IDepartmentRepository departmentRepository;

        public DepartmentService(IDepartmentRepository departmentRepository)
        {
            this.departmentRepository = departmentRepository;    
        }

        public Department Save(Department department)
        {
            if (string.IsNullOrEmpty(department.DepartmentName))
            {
                throw new RequiredDepartmentNameException();
            }
            var foundDepartmentHead = departmentRepository.Retrieve(department.DepartmentHead);
            if (foundDepartmentHead == null)
            {
                throw new NonExistingDepartmentHeadException();
            }
            var result = departmentRepository.Create(department);
            return result;
        }
    }
}