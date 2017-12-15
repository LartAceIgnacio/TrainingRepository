using System;
using BlastAsia.DigiBook.Domain.Models.Employees;
using BlastAsia.DigiBook.Domain.Employees.Exceptions;

namespace BlastAsia.DigiBook.Domain.Employees
{
    public class EmployeeService
    {
        private IEmployeeRepository employeeRepository;
        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }
        public Employee Create(Employee employee)
        {
            if (string.IsNullOrEmpty(employee.FirstName))
            {
                throw new NameRequiredException("First Name is required");
            }
            if (string.IsNullOrEmpty(employee.LastName))
            {
                throw new NameRequiredException("Last Name is required");
            }
            if (string.IsNullOrEmpty(employee.MobileNumber))
            {
                throw new MobilePhoneRequiredException("Mobile Phone is required");
            }
            if (string.IsNullOrEmpty(employee.EmailAddress))
            {
                throw new EmailAddressRequiredException("Email Address is required");
            }
            if (employee.Photo == null)
            {
                throw new PhotoRequiredException("Photo is required");
            }
            if (string.IsNullOrEmpty(employee.OfficePhone))
            {
                throw new OfficePhoneRequiredException("Office Phone is required");
            }
            if (string.IsNullOrEmpty(employee.Extension))
            {
                throw new ExtensionRequiredException("Extension is required");
            }
            Employee result = null;
            var found = employeeRepository
                .Retrieve(employee.EmployeeId);
            if(found == null)
            {
                result = employeeRepository
                    .Create(employee);
            }else
            {
                result = employeeRepository
                    .Update(employee.EmployeeId, employee);
            }

            return result;
        }
    }
}