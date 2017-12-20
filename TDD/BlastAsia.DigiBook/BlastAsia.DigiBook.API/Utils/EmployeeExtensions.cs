using BlastAsia.DigiBook.Domain.Models.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlastAsia.DigiBook.API.Utils
{
    public static class EmployeeExtensions
    {
        public static Employee ApplyChanges(this Employee employee,
              Employee from)
        {
            employee.FirstName = from.FirstName;
            employee.LastName = from.LastName;
            employee.MobilePhone = from.MobilePhone;
            employee.EmailAddress = from.EmailAddress;
            employee.OfficePhone = from.OfficePhone; 
            employee.Extension = from.Extension;
            employee.Notes = from.Notes;

            return employee;
        }
    }
}
