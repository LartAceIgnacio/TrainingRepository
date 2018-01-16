using System;
using BlastAsia.DigiBook.Domain.Models.Departments;
using BlastAsia.DigiBook.Domain.Departments.Exceptions;
using BlastAsia.DigiBook.Domain.Employees;

namespace BlastAsia.DigiBook.Domain.Departments
{
    public class DepartmentService
    {
        private IDepartmentRepository departmentRepo;
        private IEmployeeRepository employeeRepo;


        public DepartmentService(IDepartmentRepository departmentRepo, IEmployeeRepository employeeRepo)
        {
            this.departmentRepo = departmentRepo;
            this.employeeRepo = employeeRepo;
        }

        public Department Save(Guid id, Department department)
        {

            if (department.DepartmentName.Length < 6)
            {
                throw new InvalidDepartmentNameException("Invalid Name Length");
            }

            var DepartmentHead = this.employeeRepo.Retrieve(department.DepartmentHeadId);

            if (DepartmentHead == null)
            {

                throw new InvalidDepartmentIdException("No Head Found");
            }

            Department result = null;

            var Department = this.departmentRepo.Retrieve(id);

            if (Department == null) {
                result = this.departmentRepo.Create(department);
            } else
            {
                result = this.departmentRepo.Update(id, department);
            }

            return result;
        }
    }
}