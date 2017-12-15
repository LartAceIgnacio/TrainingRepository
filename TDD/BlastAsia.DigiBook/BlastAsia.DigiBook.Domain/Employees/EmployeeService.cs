using System;
using BlastAsia.DigiBook.Domain.Models.Employees;
using System.Text.RegularExpressions;

namespace BlastAsia.DigiBook.Domain.Employees
{
    public class EmployeeService
    {
        private IEmployeeRepository employeeRepository;
        private readonly string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
         @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
         @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }
        public Employee Save(Employee employee)
        {
            if (string.IsNullOrEmpty(employee.FirstName))
            {
                throw new NameRequiredException("Firstname is required");
            }
            if (string.IsNullOrEmpty(employee.LastName))
            {
                throw new NameRequiredException("Lastname is required");
            }
            if (string.IsNullOrEmpty(employee.MobilePhone))
            {
                throw new MobilePhoneRequiredException("Mobile phone is required");
            }
            if ((!string.IsNullOrWhiteSpace(employee.EmailAddress)))
            {
                if (!Regex.IsMatch(employee.EmailAddress, strRegex, RegexOptions.IgnoreCase))
                    throw new InvalidEmailAddressException("Valid Email address is required!");
            }
            if (string.IsNullOrEmpty(employee.Photo))
            {
                throw new PhotoRequiredException("Photo is required");
            }
            if (string.IsNullOrEmpty(employee.OfficePhone))
            {
                throw new OfficePhoneRequiredException("Office phone is required");
            }
            if (string.IsNullOrEmpty(employee.Extension))
            {
                throw new ExtensionRequiredException("Extension is required");
            }

            Employee result = null;
            var found = employeeRepository
                .Retrieve(employee.EmployeeId);
            if (found == null)
            {
                result = employeeRepository.Create(employee);
            }
            else
            {
                result = employeeRepository
                    .Update(employee.EmployeeId, employee);
            }
            return result;
        }

    }
}