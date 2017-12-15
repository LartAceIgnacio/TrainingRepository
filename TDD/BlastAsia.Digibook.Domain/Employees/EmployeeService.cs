using BlastAsia.Digibook.Domain.Models.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.Digibook.Domain.Employees
{
    public class EmployeeService
    {
        private IEmployeeRepository employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }

        public Employee Save(Employee employee)
        {
            if (string.IsNullOrEmpty(employee.FirstName))
            {
                throw new InvalidNameFormatException("First name cannot be blank.");
            }

            if (string.IsNullOrEmpty(employee.LastName))
            {
                throw new InvalidNameFormatException("Last name cannot be blank.");
            }

            if (string.IsNullOrEmpty(employee.MobilePhone))
            {
                throw new InvalidPhoneFormatException("Mobile phone cannot be blank.");
            }

            if (string.IsNullOrEmpty(employee.EmailAddress))
            {
                throw new InvalidEmailAddressFormatException();
            }

            if(employee.Photo == null)
            {
                throw new InvalidPhotoException();
            }

            if (string.IsNullOrEmpty(employee.OfficePhone))
            {
                throw new InvalidPhoneFormatException("Office phone cannot be blank.");
            }

            if (string.IsNullOrEmpty(employee.Extension))
            {
                throw new InvalidExtensionFormatException();
            }

            Employee result = null;

            var found = employeeRepository.Retrieve(employee.EmployeeId);

            if(found == null)
            {
                result = employeeRepository.Create(employee);
            }
            else
            {
                result = employeeRepository.Update(employee,employee.EmployeeId);
            }

            return result;
        }
    }
}
