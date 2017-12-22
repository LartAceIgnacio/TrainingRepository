using BlastAsia.DigiBook.Domain.Departments;
using BlastAsia.DigiBook.Domain.Models.Departments;

namespace BlastAsia.DigiBook.Infrastructure.Persistence.Repositories
{
    public class DepartmentRepository 
        : RepositoryBase<Department>, IDepartmentRepository
    {
        public DepartmentRepository(IDigiBookDbContext context): base(context)
        {

        }
    }
}