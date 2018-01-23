using BlastAsia.DigiBook.Domain.Employees;
using BlastAsia.DigiBook.Domain.Models.Departments;
using System;

namespace BlastAsia.DigiBook.Domain.Departments
{
    public class DepartmentService : IDepartmentService
    {
        private IDepartmentRepository departmentRepository;
        public IEmployeeRepository employeeRepository;

        public DepartmentService(IDepartmentRepository departmentRepository,
                IEmployeeRepository employeeRepository)
        {
            this.departmentRepository = departmentRepository;
            this.employeeRepository = employeeRepository;
        }
        public Department Save(Guid id,Department department)
        {
            if (string.IsNullOrEmpty(department.DepartmentName))
            {
                throw new DepartmentNameRequiredException("DepartmentName is required");
            }

            Department result = null;

            //Repositories
            var foundDepartmentMemberId = employeeRepository
                .Retrieve(department.DeparmentMemberId);

            // Check if foundDepartmentMemberId is null
            if (foundDepartmentMemberId == null)
            {
                throw new DepartmentMemberIdRequiredException("Department MemberID is required!");
            }

            var foundDepartment = departmentRepository.Retrieve(id);

            if(foundDepartment == null)
            {
                result = departmentRepository.Create(department);
            }
            else
            {
                result = departmentRepository.Update(id,department);
            }

            return result;
        }
    }
}