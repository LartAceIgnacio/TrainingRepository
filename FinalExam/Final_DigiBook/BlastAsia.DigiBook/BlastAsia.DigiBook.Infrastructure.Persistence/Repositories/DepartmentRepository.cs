using System;
using System.Collections.Generic;
using System.Linq;
using BlastAsia.DigiBook.Domain.Departments;
using BlastAsia.DigiBook.Domain.Models.Departments;
using Microsoft.EntityFrameworkCore;

namespace BlastAsia.DigiBook.Infrastructure.Persistence.Repositories
{
    public class DepartmentRepository
        : IDepartmentRepository
    {
        public readonly IDigiBookDbContext context;
        public DepartmentRepository(IDigiBookDbContext context)
        {
            this.context = context;
        }
        public Department Create(Department department)
        {
            context.Set<Department>().Add(department);
            context.SaveChanges();
            return department;
        }
        public void Delete(Guid id)
        {
            var found = this.Retrieve(id);
            context.Set<Department>().Remove(found);
            context.SaveChanges();
        }

        public Department Retrieve(Guid id)
        {
            return context.Set<Department>().Find(id);
        }

        public IEnumerable<Department> Retrieve()
        {
            return context.Set<Department>().ToList();
        }
        public Department Update(Guid id, Department department)
        {
            //context.Set<Department>().Update(department);
            context.SaveChanges();
            return department;
        }

        
    }

}