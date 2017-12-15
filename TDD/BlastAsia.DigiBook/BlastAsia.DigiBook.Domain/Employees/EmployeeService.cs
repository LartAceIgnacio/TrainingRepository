

using BlastAsia.DigiBook.Domain.Models.Employees;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace BlastAsia.DigiBook.Domain.Employees
{
    public class EmployeeService
    {
        private IEmployeeRepository employeeRepository;
        private readonly string pattern = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }

        public Employee Save(Employee employee)
        {
            if (string.IsNullOrEmpty(employee.FirstName))
            {
                throw new EmployeeDetailRequiredException("First Name is required");
            }
            if (string.IsNullOrEmpty(employee.LastName))
            {
                throw new EmployeeDetailRequiredException("Last Name is required");
            }
            if (string.IsNullOrEmpty(employee.MobilePhone))
            {
                throw new EmployeeDetailRequiredException("Mobile Phone is required");
            }
            if (string.IsNullOrEmpty(employee.EmailAddress))
            {
                throw new EmployeeDetailRequiredException("Email Address is required");
            }
            if (string.IsNullOrEmpty(employee.OfficePhone))
            {
                throw new EmployeeDetailRequiredException("Office Phone is required");
            }
            if (string.IsNullOrEmpty(employee.Extension))
            {
                throw new EmployeeDetailRequiredException("Extension is required");
            }
            if (!Regex.IsMatch(employee.EmailAddress, pattern, RegexOptions.IgnoreCase))
            {
                throw new EmailInvalidFormatException();
            }
            if (employee.Photo == null)
            {
                throw new EmployeeDetailRequiredException("Photo is required");
            }
           
            Employee result = null;
            var found = employeeRepository
                        .Retrieve(employee.EmployeeeId);

            if (found == null)
            {
                result = employeeRepository.Create(employee);
            }
            else
            {
                result = employeeRepository
                    .Update(employee.EmployeeeId, employee);
            }

            return result;
        }
    }
}