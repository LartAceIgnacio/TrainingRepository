using BlastAsia.DigiBook.Domain.Employees;
using BlastAsia.DigiBook.Domain.Employees.Exceptions;
using BlastAsia.DigiBook.Domain.Models.Employees;
using Castle.Core.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Employees
{
    public class EmployeeService : IEmployeeService
    {
        private IEmployeeRepository employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }
        public Employee Save(Guid id,Employee employee)
        {
            if (string.IsNullOrEmpty(employee.FirstName))
            {
                throw new NameRequiredException("FirstName is Required");
            }
            if (string.IsNullOrEmpty(employee.LastName))
            {
                throw new NameRequiredException("LastName is Required");
            }
            if (string.IsNullOrEmpty(employee.MobilePhone))
            {
                throw new MobilePhoneRequiredException("MobilePhone is Required");
            }
            if (string.IsNullOrEmpty(employee.EmailAddress))
            {
                throw new EmailAddressRequiredException("EmailAddress is Required");
            }
            //if (employee.Photo == null)
            //{
            //    throw new PhotoRequiredException("Photo is Required");
            //}
            if (string.IsNullOrEmpty(employee.OfficePhone))
            {
                throw new OfficePhoneRequiredException("OfficePhone is Required");
            }
            if (string.IsNullOrEmpty(employee.Extension))
            {
                throw new ExtensionRequiredException("Extension is Required");
            }

            Employee result = null;

            var found = employeeRepository.Retrieve(id);

            if (found == null)
            {
                result = employeeRepository.Create(employee);
            }
            else
            {
                result = employeeRepository.Update(id, employee);
            }
            return result;
        }
    }
}
