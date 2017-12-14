using System;
using BlastAsia.DigiBook.Domain.Models.Employees;
using System.Text.RegularExpressions;

namespace BlastAsia.DigiBook.Domain.Employees
{
    public class EmployeeService
    {
        private IEmployeeRepository employeeRepository;
        private readonly string regex = @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }

        public void Save(Employee employee)
        {
            if (string.IsNullOrEmpty(employee.FirstName) || string.IsNullOrEmpty(employee.LastName))
                throw new NameRequiredException(string.IsNullOrEmpty(employee.FirstName) ? "First Name is Required!" : "Last Name is Required!");
            
            if (string.IsNullOrEmpty(employee.MobilePhone) || string.IsNullOrEmpty(employee.OfficePhone))
                throw new PhoneRequiredException(string.IsNullOrEmpty(employee.MobilePhone) ? "Mobile Phone is Required!" : "Office Phone is Required!");
            
            if (string.IsNullOrEmpty(employee.EmailAddress))
                throw new EmailAddressRequiredException("Email Address is Required!");

            if (!Regex.IsMatch(employee.EmailAddress, regex))
                throw new InvalidEmailAddressException("Invalid Email Address!");

            if (employee.Photo == null)
                throw new PhotoRequiredException("Photo is Required!");

            if (string.IsNullOrEmpty(employee.Extension))
                throw new ExtensionRequiredException("Photo is Required!");

            Employee retrieveEmployee = null;
            var found = employeeRepository.Retrieve(employee.EmployeeId);

            if (found == null)
                retrieveEmployee = employeeRepository.Create(employee);
            else
                retrieveEmployee = employeeRepository.Update(employee.EmployeeId, employee);
        }
    }
}