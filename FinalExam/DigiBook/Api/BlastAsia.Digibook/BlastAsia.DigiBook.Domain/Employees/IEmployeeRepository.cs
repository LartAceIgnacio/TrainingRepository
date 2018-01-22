using System;
using System.Collections.Generic;
using BlastAsia.DigiBook.Domain.Models.Employees;
using BlastAsia.DigiBook.Domain.Models.Pagination;

namespace BlastAsia.DigiBook.Domain.Employees
{
    public interface IEmployeeRepository
        : IRepository<Employee>
    {
        Pagination<Employee> Retrieve(int pageNumber, int recordNumber, string keyWord);
        //IEnumerable<Employee> Retrieve(int pageNumber, int recordNumber, string keyWord);
        //IEnumerable<Employee> Retrieve(string keyWord);
    }
}