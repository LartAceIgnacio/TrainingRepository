using BlastAsia.DigiBook.Domain.Contacts.ContactExceptions;
using BlastAsia.DigiBook.Domain.Employees.EmployeeExceptions;
using BlastAsia.DigiBook.Domain.Models.Employees;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace BlastAsia.DigiBook.Domain.Employees.Services
{
    public class EmployeeService : IEmployeeService
    {
        private IEmployeeRepository _empRepository;
        const string rfc2822EmailPattern = @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";

        public EmployeeService(IEmployeeRepository repository)
        {
            this._empRepository = repository;
        }

        public Employee Save(Guid id, Employee employee)
        {
            if (string.IsNullOrEmpty(employee.Firstname)) throw new NameRequiredException("Firstname is required.");
            if (string.IsNullOrEmpty(employee.Lastname)) throw new NameRequiredException("Lastname is required.");
            if (string.IsNullOrEmpty(employee.MobilePhone)) throw new ContactNumberRequiredException("Mobile phone is required.");
            if (employee.MobilePhone.Length < 11) throw new ContactNumberMinimumLength("Mobile number invalid length.");
            if (!Regex.IsMatch(employee.EmailAddress, rfc2822EmailPattern)) throw new InvalidEmailFormatException("Invalid email format.");
            //if (employee.Photo == Stream.Null) throw new EmployeeImageException("Image is required.");
            if (string.IsNullOrEmpty(employee.OfficePhone)) throw new ContactNumberRequiredException("Office phone is required.");
            if (employee.OfficePhone.Length < 7) throw new ContactNumberMinimumLength("Office number invalid length.");
            if (string.IsNullOrEmpty(employee.Extension)) throw new ExtensionNumberException("Extension number is required");


            Employee emp = null;
            var employeeExists = _empRepository.Retrieve(id);

            if (employeeExists == null)
            {
                emp = _empRepository.Create(employee);
            }
            else
            {
                emp = _empRepository.Update(id, employee);
            }

            return emp;
        }
    }
}
