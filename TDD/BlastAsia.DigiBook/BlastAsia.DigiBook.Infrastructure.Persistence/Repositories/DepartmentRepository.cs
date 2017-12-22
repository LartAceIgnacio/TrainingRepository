using BlastAsia.DigiBook.Domain.Departments;
using BlastAsia.DigiBook.Domain.Models.Departments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlastAsia.DigiBook.Infrastructure.Persistence.Repositories
{
    public class DepartmentRepository
        : IDepartmentRepository
    {
        private DigiBookDbContext dbContext;

        public DepartmentRepository(DigiBookDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Department Create(Department department)
        {
            dbContext.Set<Department>().Add(department);
            dbContext.SaveChanges();

            return department;
        }

        public Department Retrieve(Guid Id)
        {
            return dbContext.Set<Department>().Find(Id);
        }

        public Department Update(Guid Id, Department department)
        {
            Retrieve(Id);
            dbContext.Set<Department>().Update(department);
            dbContext.SaveChanges();
            return department;
        }

        public void Delete(Guid departmentId)
        {
            var department = this.Retrieve(departmentId);
            dbContext.Set<Department>().Remove(department);
            dbContext.SaveChanges();
        }

        public IEnumerable<Department> Retrieve()
        {
            return dbContext.Set<Department>().ToList();
        }
    }
}
