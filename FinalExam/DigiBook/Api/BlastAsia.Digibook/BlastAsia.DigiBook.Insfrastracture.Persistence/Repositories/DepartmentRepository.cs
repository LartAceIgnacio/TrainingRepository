using BlastAsia.DigiBook.Domain.Departments;
using BlastAsia.DigiBook.Domain.Models.Departments;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Infrastracture.Persistence.Repositories
{
    public class DepartmentRepository
        : RepositoryBase<Department>, IDepartmentRepository
    {
        private readonly IDigiBookDbContext context;
        public DepartmentRepository(IDigiBookDbContext context) 
            : base(context)
        {
            this.context = context;
        }
    }
}
