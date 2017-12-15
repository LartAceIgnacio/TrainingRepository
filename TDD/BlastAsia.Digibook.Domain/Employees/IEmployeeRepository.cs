using BlastAsia.Digibook.Domain.Models.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.Digibook.Domain.Employees
{
    public interface IEmployeeRepository
    {
        Employee Create(Employee employee);
        Employee Retrieve(Guid employeeId);
        Employee Update(Employee employee, Guid employeeId);
    }
}
