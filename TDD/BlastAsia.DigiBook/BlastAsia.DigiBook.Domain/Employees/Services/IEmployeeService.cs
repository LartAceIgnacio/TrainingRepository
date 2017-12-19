using BlastAsia.DigiBook.Domain.Models.Employees;
using System;

namespace BlastAsia.DigiBook.Domain.Employees.Services
{
    public interface IEmployeeService
    {
        Employee Save(Guid id, Employee employee);
    }
}