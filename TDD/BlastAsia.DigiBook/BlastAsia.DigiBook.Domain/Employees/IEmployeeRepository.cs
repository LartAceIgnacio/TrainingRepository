using System;
using BlastAsia.DigiBook.Domain.Models.Employees;

namespace BlastAsia.DigiBook.Domain.Employees
{
    public interface IEmployeeRepository:
        IRepository<Employee>
    {
    }
}