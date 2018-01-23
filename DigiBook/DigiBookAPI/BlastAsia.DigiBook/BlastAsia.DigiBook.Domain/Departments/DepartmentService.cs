using System;
using BlastAsia.DigiBook.Domain.Departments.Exceptions;
using BlastAsia.DigiBook.Domain.Models.Departments;

namespace BlastAsia.DigiBook.Domain.Departments
{
    public class DepartmentService
    {
        private IDepartmentRepository departmentRepository;

        public DepartmentService(IDepartmentRepository departmentRepository)
        {
            this.departmentRepository = departmentRepository;
        }

        public Department Save(Guid id, Department department)
        {
            if (string.IsNullOrEmpty(department.DepartmentName))
            {
                throw new DepartmentNameRequiredException("Department name is required.");
            }
            if (string.IsNullOrEmpty(department.Description))
            {
                throw new DepartmentDescriptionException("Department is required.");
            }

            Department result = null;
            var found = departmentRepository
                .Retrieve(id);

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