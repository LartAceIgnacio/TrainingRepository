using BlastAsia.DigiBook.Domain.Employees;
using BlastAsia.DigiBook.Domain.Models.Employees;

namespace BlastAsia.DigiBook.Infrastructure.Persistence.Repositories
{
    public class EmployeeRepository
        : RepositoryBase<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(IDigiBookDbContext context)
            : base(context)
        {

        }
    }
}