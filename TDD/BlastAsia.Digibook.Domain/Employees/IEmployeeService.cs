using BlastAsia.Digibook.Domain.Models.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.Digibook.Domain.Employees
{
    public interface IEmployeeService
    {
        Employee Save(Employee employee);
    }
}
