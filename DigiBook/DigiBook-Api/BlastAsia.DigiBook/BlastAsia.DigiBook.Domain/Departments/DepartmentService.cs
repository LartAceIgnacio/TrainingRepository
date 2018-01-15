using BlastAsia.DigiBook.Domain.Models.Departments;
using BlastAsia.DigiBook.Domain.Departments;
using System;
using BlastAsia.DigiBook.Domain.Departments.Exceptions;

namespace BlastAsia.DigiBook.Domain.Departments
{
    public class DepartmentService : IDepartmentService
    {

        private IDepartmentRepository departmentRepository;

        public DepartmentService(IDepartmentRepository departmentRepository)
        {
            this.departmentRepository = departmentRepository;
        }
        public Department Save(Guid id,Department department)
        {
            if (string.IsNullOrEmpty(department.DepartmentName))
            {
                throw new DepartmentNameRequiredException("Department name is required");
                
            }

            Department result = null;

            var found = departmentRepository.Retrieve(department.DepartmentId);

            if (found == null)
            {
                 result = departmentRepository.Create(department);
            }
            else
            {
                result = departmentRepository.Update(department.DepartmentId, department);
            }

            return result;
        }
    }
}