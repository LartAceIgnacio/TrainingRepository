using System;
using BlastAsia.DigiBook.Domain.Employees;
using BlastAsia.DigiBook.Domain.Models.Departments;

namespace BlastAsia.DigiBook.Domain.Departments
{
    public class DepartmentService: IDepartmentService
    {
        private IDepartmentRepository departmentRepository;
        private IEmployeeRepository employeeRepository;

        public DepartmentService(IDepartmentRepository departmentRepository, IEmployeeRepository employeeRepository)
        {
            this.departmentRepository = departmentRepository;
            this.employeeRepository = employeeRepository;
        }

        public Department Save(Guid id, Department department)
        {
            if (String.IsNullOrEmpty(department.DepartmentName))
            {
                throw new DepartmentNameRequiredException("Department name is required.");
            }

            var departmentHead = employeeRepository.Retrieve(department.DepartmentHeadId);
            if (departmentHead == null)
            {
                throw new DepartmentHeadRequiredException("Department head is required");
            }

            var found = departmentRepository.Retrieve(id);
            Department result = null;
            if (found == null)
            {
                result = departmentRepository.Create(department);
            }
            else
            {
                result = departmentRepository.Update(id, department);
            }
            
            return result;
        }
    }
}