using BlastAsia.DigiBook.Domain.Models.Employees;
using System;
using System.Text.RegularExpressions;

namespace BlastAsia.DigiBook.Domain.Employees
{
    public class EmployeeService : IEmployeeService
    {
        private IEmployeeRepository employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }

        private readonly string validEmail = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";

        public Employee Save(Guid id, Employee employee)
        {
            if (string.IsNullOrEmpty(employee.FirstName))
            {
                throw new NameRequiredException("First name required");
            }
            if (string.IsNullOrEmpty(employee.LastName))
            {
                throw new NameRequiredException("Last name required");
            }
            if (string.IsNullOrEmpty(employee.MobilePhone))
            {
                throw new PhoneNumberRequiredException("Mobile phone required");
            }
            if (string.IsNullOrEmpty(employee.EmailAddress))
            {
                throw new EmailAddressRequiredException("Email address required");
            }
            //if (employee.Photo == null)
            //{
            //    throw new PhotoRequiredException("Photo required");
            //}
            if (string.IsNullOrEmpty(employee.OfficePhone))
            {
                throw new PhoneNumberRequiredException("Office phone required");
            }
            if (string.IsNullOrEmpty(employee.Extension))
            {
                throw new ExtensionRequiredException("Extension required");
            }
            if (!Regex.IsMatch(employee.EmailAddress, validEmail, RegexOptions.IgnoreCase))
            {
                throw new EmailAddressRequiredException("Valid email address required");
            }

            Employee currentEmployee = null;
            var found = employeeRepository.Retrieve(employee.EmployeeId);

            if (found == null)
            {
                currentEmployee = employeeRepository.Create(employee);
            }
            else
            {
                currentEmployee = employeeRepository.Update(employee.EmployeeId, employee);
            }

            return currentEmployee;
        }
    }
}