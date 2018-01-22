using System;
using BlastAsia.DigiBook.Domain.Models.Employees;
using System.Linq;

namespace BlastAsia.DigiBook.Domain.Employees
{
    public class EmpolyeeService : IEmpolyeeService
    {
        private readonly IEmployeeRepository employeeRepository;
        public EmpolyeeService(IEmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }

        public Employee Save(Guid id, Employee employee)
        {
            if(string.IsNullOrEmpty(employee.FirstName))
            {
                throw new NameRequiredException();
            }
            if(string.IsNullOrEmpty(employee.LastName))
            {
                throw new NameRequiredException();
            }
            if(string.IsNullOrEmpty(employee.MobilePhone))
            {
                throw new MobilePhoneRequiredException();
            }
            if(string.IsNullOrEmpty(employee.EmailAddress))
            {
                throw new EmailAddressRequiredException();
            }
            //if(employee.photo == null)
            //{
            //    throw new PhotoRequiredException();
            //}
            if(string.IsNullOrEmpty(employee.OfficePhone))
            {
                throw new OfficePhoneRequiredException();
            }
            if(string.IsNullOrEmpty(employee.Extension))
            {
                throw new ExtensionRequiredException();
            }
            if(!employee.MobilePhone.All(Char.IsDigit))
            {
                throw new NumbersOnlyException();
            }
            if (!employee.OfficePhone.All(Char.IsDigit))
            {
                throw new NumbersOnlyException();
            }
            if (!employee.Extension.All(Char.IsDigit))
            {
                throw new NumbersOnlyException();
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