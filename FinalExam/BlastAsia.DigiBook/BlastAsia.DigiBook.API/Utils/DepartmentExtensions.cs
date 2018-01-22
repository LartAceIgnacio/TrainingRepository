using BlastAsia.DigiBook.Domain.Models.Departments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlastAsia.DigiBook.API.Utils
{
    public static class DepartmentExtensions
    {
        public static Department ApplyChanges(this Department department, Department from)
        {
            department.DepartmentName = from.DepartmentName;
            department.DepartmentHeadId = from.DepartmentHeadId;

            return department;
        }
    }
}
