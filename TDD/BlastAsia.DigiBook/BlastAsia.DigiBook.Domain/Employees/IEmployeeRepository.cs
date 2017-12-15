using System;
using BlastAsia.DigiBook.Domain.Models.Employees;

namespace BlastAsia.DigiBook.Domain.Employees
{
    public interface IEmployeeRepository
    {
        Employee Create(Employee employee);

        Employee Retrieve(Guid id);

        Employee Update(Guid id, Employee employee);
    }
}