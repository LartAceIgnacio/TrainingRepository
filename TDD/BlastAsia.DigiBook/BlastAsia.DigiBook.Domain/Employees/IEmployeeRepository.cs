using System;
using BlastAsia.DigiBook.Domain.Models.Employees;

namespace BlastAsia.DigiBook.Domain.Employees
{
    public interface IEmployeeRepository
    {
        Employee Retrieve(Guid employeeId);
        Employee Save(Employee employee);
        Employee Update(Guid contactId, Employee contact);
        Employee Create(Employee employee);
    }
}