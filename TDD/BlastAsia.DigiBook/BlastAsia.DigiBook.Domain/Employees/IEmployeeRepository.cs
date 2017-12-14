using BlastAsia.DigiBook.Domain.Models.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Employees
{
    public interface IEmployeeRepository
    {
        Employee Retrieve(Guid EmployeeId);
        Employee Create(Employee employee);
        Employee Update(Guid id, Employee employee);
    }
}
