using BlastAsia.DigiBook.Domain.Models;

namespace BlastAsia.DigiBook.Domain.Employees
{
    public interface IEmployeeRepository
        : IRepository<Employee>
    {
        Pagination<Employee> Retrieve(int pageNo, int numRec, string filterValue);
    }
}