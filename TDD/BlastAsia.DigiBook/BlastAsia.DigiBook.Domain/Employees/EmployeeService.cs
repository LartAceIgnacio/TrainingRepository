using System;
using BlastAsia.DigiBook.Domain.Models.Employees;
using System.Text.RegularExpressions;

namespace BlastAsia.DigiBook.Domain.Employees
{
    public class EmployeeService : IEmployeeService
    {
        
        private IEmployeeRepository employeeRepository;
        private readonly String emailPattern = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                                  + "@"
                                  + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$";

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }

        public Employee Save(Guid id, Employee employee)
        {

            if (string.IsNullOrEmpty(employee.FirstName))
            {
                throw new NameRequiredException("First name is required.");
            }
            if (string.IsNullOrEmpty(employee.LastName))
            {
                throw new NameRequiredException("Last name is required.");
            }
            if (string.IsNullOrEmpty(employee.MobilePhone))
            {
                throw new PhoneNumberRequiredException("Mobile Number is required");
            }
            if (string.IsNullOrEmpty(employee.EmailAddress))
            {
                throw new EmailRequiredException("Email is required");
            }
            if ((!Regex.IsMatch(employee.EmailAddress, emailPattern, RegexOptions.IgnoreCase)))
            {
                throw new EmailRequiredException("Invalid Email");
            }
            //if (employee.Photo == null)
            //{
            //    throw new PhotoRequiredException("Employee photo is required.");
            //}
            if (string.IsNullOrEmpty(employee.OfficePhone))
            {
                throw new PhoneNumberRequiredException("Office phone number is required.");
            }
            Employee result = null;

            var found = employeeRepository
                .Retrieve(id);

            if (found == null)
            {
                result = employeeRepository.Create(employee);
            }
            else
            {
                result = employeeRepository.Update(id,employee);
            }

            return result;

        }
    }
}