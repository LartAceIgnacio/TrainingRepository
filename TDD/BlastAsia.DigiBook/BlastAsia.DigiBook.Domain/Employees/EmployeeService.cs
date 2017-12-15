using System;
using BlastAsia.DigiBook.Domain.Models.Employees;
using System.Text.RegularExpressions;

namespace BlastAsia.DigiBook.Domain.Employees
{
    public class EmployeeService
    {
        private IEmployeeRepository employeeRepository;
        private readonly string EmailValid = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }

        public Employee Save(Employee employee)
        {

            if (string.IsNullOrEmpty(employee.FirstName))
            {
                throw new FirstNameRequiredException("First name is required");
            }
            if (string.IsNullOrEmpty(employee.LastName))
            {
                throw new LastNameRequiredException("Last name is required");
            }
            if (string.IsNullOrEmpty(employee.MobilePhone))
            {
                throw new MobilePhoneRequiredException("Mobile phone is required");
            }
            if (string.IsNullOrEmpty(employee.EmailAddress))
            {
                throw new EmailAddressRequiredException("Email address is required");
            }
            else 
            {
                if (!Regex.IsMatch(employee.EmailAddress, EmailValid, RegexOptions.IgnoreCase))
                {
                    throw new ValidEmailAddressRequiredException("Valid email address is required");
                }
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

            var found = employeeRepository.Retrieve(employee.EmployeeId);

            if(found != null)
            {
                result =  employeeRepository.Update(employee.EmployeeId, employee);
            }
            else
            {
                result = employeeRepository.Create(employee);
            }

            return result;
        }
    }
}