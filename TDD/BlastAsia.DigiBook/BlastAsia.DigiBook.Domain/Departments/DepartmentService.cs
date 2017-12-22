using System;
using BlastAsia.DigiBook.Domain.Models.Departments;
using BlastAsia.DigiBook.Domain.Employees;

namespace BlastAsia.DigiBook.Domain.Departments
{
    public class DepartmentService : IDepartmentService
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
            if (string.IsNullOrEmpty(department.DepartmentName))
            {
                throw new DeparmentNameRequiredException("Department name is required");
            }

            var deparmentHeadFound = employeeRepository.Retrieve(department.DepartmentHeadId);
            if (deparmentHeadFound == null)
            {
                throw new DepartmentHeadIdNotFoundException("Department HeadId not found.");
            }

            Department result = null;
            var departmentFound = departmentRepository.Retrieve(department.DepartmentId);

            if (departmentFound == null)
            {
                result = departmentRepository.Create(department);
            }
            else
            {
                result = departmentRepository.Update(department.DepartmentId,department);
            }
            

            return result;
        }
    }
}