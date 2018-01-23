using System;
using BlastAsia.DigiBook.Domain.Employees;
using BlastAsia.DigiBook.Domain.Models.Departments;
using BlastAsia.DigiBook.Domain.Models.Employees;

namespace BlastAsia.DigiBook.Domain.Departments
{
    public class DepartmentService
    {
        private readonly IDepartmentRepository departmentRepository;
        private readonly IEmployeeRepository employeeRepository;

        public DepartmentService(IDepartmentRepository departmentRepository
            ,IEmployeeRepository employeeRepository)
        {
            this.departmentRepository = departmentRepository;
            this.employeeRepository = employeeRepository; 
        }

        public Department Save(Guid id, Department department)
        {
            if (string.IsNullOrEmpty(department.DepartmentName))
            {
                throw new RequiredDepartmentNameException();
            }

            var foundDepartmentHead = employeeRepository.Retrieve(department.DepartmentHeadId);
            if (foundDepartmentHead == null)
            {
                throw new NonExistingDepartmentHeadException();
            }

            Department result;
            var found = departmentRepository.Retrieve(id);

            if(found == null)
            {
                result = departmentRepository.Create(department);
            }
            result = departmentRepository.Update(id, department);
          
            return result;
        }
    }
}