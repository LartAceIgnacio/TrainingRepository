using BlastAsia.DigiBook.Domain.Models.Employees;
using System;

namespace BlastAsia.DigiBook.Domain.Employees
{
    public interface IEmployeeService
    {
        Employee Save(Guid id,Employee employee);
    }
}