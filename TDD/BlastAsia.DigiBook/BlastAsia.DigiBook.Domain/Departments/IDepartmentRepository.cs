using BlastAsia.DigiBook.Domain.Models.Departments;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Departments
{
    public interface IDepartmentRepository
        : IRepository<Department>
    {
        void Save(Department department);
    }
}
