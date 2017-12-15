

using BlastAsia.DigiBook.Domain.Models.Employees;
using System;

namespace BlastAsia.DigiBook.Domain.Employees
{
    public interface IEmployeeRepository
    {
        Employee Create(Employee employee);
        Employee Retrieve(Guid id);
        Employee Update(Guid id, Employee employee);
    }

}