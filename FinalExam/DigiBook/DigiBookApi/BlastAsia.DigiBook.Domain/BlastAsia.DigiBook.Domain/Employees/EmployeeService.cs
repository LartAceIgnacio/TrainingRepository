using System;
using BlastAsia.DigiBook.Domain.Models.Employees;
using BlastAsia.DigiBook.Domain.Employees.Exceptions;
using System.Text.RegularExpressions;
using System.Linq;

namespace BlastAsia.DigiBook.Domain.Employees
{
    public class EmployeeService : IEmployeeService
    {
        private IEmployeeRepository _employeeRepository;
        private readonly string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
          @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
          @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public Employee Save(Guid id,Employee employee)
        {
            if(string.IsNullOrEmpty(employee.FirstName))
            {
                throw new NameRequiredException("Firstname is required!");
            }
            if (string.IsNullOrEmpty(employee.LastName))
            {
                throw new NameRequiredException("Lastname is required!");
            }
            if (string.IsNullOrEmpty(employee.MobilePhone))
            {
                throw new MobilePhoneException("Mobilephone is required!");
            }
            if (string.IsNullOrEmpty(employee.EmailAddress))
            {
                throw new EmailAddressException("Email address is required!");
            }
            if (!Regex.IsMatch(employee.EmailAddress, strRegex, RegexOptions.IgnoreCase))
            {
                throw new EmailAddressException("Valid Email address is required!");
            }
            //if (employee.Photo == null)
            //{
            //    throw new PhotoRequiredException("Photo is required!");
            //}
            if (string.IsNullOrEmpty(employee.OfficePhone))
            {
                throw new OfficePhoneException("Office phone is required!");
            }
            //if(!employee.MobilePhone.All(c => char.IsDigit(c)))
            //{
            //    throw new MobilePhoneException("Valid Mobilephone is required!");
            //}
            //if (!employee.OfficePhone.All(c => char.IsDigit(c)))
            //{
            //    throw new OfficePhoneException("Valid Officephone is required!");
            //}
            if (string.IsNullOrEmpty(employee.Extension))
            {
                throw new ExtensionRequiredException("Extension is required!");
            }


            Employee result = null;


            var found = _employeeRepository.Retrieve(employee.EmployeeId);
            if (found == null)
            {
                result = _employeeRepository.Create(employee);
            }
            else
            {
                result = _employeeRepository.Update(employee.EmployeeId, employee);
            }

            return result;
        }
    }
}