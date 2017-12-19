using System;
using BlastAsia.DigiBook.Domain.Models.Employees;
using System.Linq;

namespace BlastAsia.DigiBook.Domain.Employees
{
    public class EmpolyeeService : IEmpolyeeService
    {
        private IEmployeeRepository employeeRepository;
        public EmpolyeeService(IEmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }

        public Employee Save(Guid id, Employee employee)
        {
            if(string.IsNullOrEmpty(employee.firstName))
            {
                throw new NameRequiredException();
            }
            if(string.IsNullOrEmpty(employee.lastName))
            {
                throw new NameRequiredException();
            }
            if(string.IsNullOrEmpty(employee.mobilePhone))
            {
                throw new MobilePhoneRequiredException();
            }
            if(string.IsNullOrEmpty(employee.emailAddress))
            {
                throw new EmailAddressRequiredException();
            }
            //if(employee.photo == null)
            //{
            //    throw new PhotoRequiredException();
            //}
            if(string.IsNullOrEmpty(employee.officePhone))
            {
                throw new OfficePhoneRequiredException();
            }
            if(string.IsNullOrEmpty(employee.extension))
            {
                throw new ExtensionRequiredException();
            }
            if(!employee.mobilePhone.All(Char.IsDigit))
            {
                throw new NumbersOnlyException();
            }
            if (!employee.officePhone.All(Char.IsDigit))
            {
                throw new NumbersOnlyException();
            }
            if (!employee.extension.All(Char.IsDigit))
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