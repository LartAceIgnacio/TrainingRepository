 using System;
using BlastAsia.DigiBook.Domain.Models.Employees;
using BlastAsia.DigiBook.Domain.Contacts;
using BlastAsia.DigiBook.Domain.Contacts.Exception;
using System.Text.RegularExpressions;

namespace BlastAsia.DigiBook.Domain.Employees
{
    public class EmployeeService : IEmployeeService
    {
        private readonly string regex = @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";

        private IEmployeeRepository employeeRepo;
        public EmployeeService(IEmployeeRepository employeeService)
        {
            this.employeeRepo = employeeService;
        }

        public Employee Save(Guid id , Employee employee)
        {
            // First Name Validation
            if (string.IsNullOrWhiteSpace(employee.FirstName) || string.IsNullOrEmpty(employee.FirstName)) throw new NameRequiredException("First Name is required");
            // Last Name Validation
            if (string.IsNullOrWhiteSpace(employee.LastName) || string.IsNullOrEmpty(employee.LastName)) throw new NameRequiredException("Last Name is required");
            // Mobile number Validation
            if (string.IsNullOrWhiteSpace(employee.MobilePhone) || string.IsNullOrEmpty(employee.MobilePhone)) throw new MobilePhoneRequiredException("Mobile Number is required");
            // Email Address Validation
            if (string.IsNullOrWhiteSpace(employee.EmailAddress) || string.IsNullOrEmpty(employee.EmailAddress))
            {
                throw new InvalidEmailException("Email Address is required");
            } else
            {
                if (!Regex.IsMatch(employee.EmailAddress, regex))
                {
                    throw new InvalidEmailException("Invalid email format");
                }
            }
            // Photo Stream Validation
           // if (employee.Photo == null) throw new PhotoRequiredException("Photo is required");
            // Office Phone validation
            if (string.IsNullOrWhiteSpace(employee.OfficePhone) || string.IsNullOrEmpty(employee.OfficePhone)) throw new MobilePhoneRequiredException("Mobile Number is required");
            // Extention validation
            if (string.IsNullOrWhiteSpace(employee.Extension) || string.IsNullOrEmpty(employee.Extension)) throw new MobilePhoneRequiredException("Mobile Number is required");


            // If Everything is good then  go
            Employee result;
            var existing = this.employeeRepo.Retrieve(employee.EmployeeId);

            result = existing != null ? this.employeeRepo.Update(employee.EmployeeId, employee) : this.employeeRepo.Create(employee);

            return result;
        }
    }
}