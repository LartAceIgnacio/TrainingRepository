using BlastAsia.DigiBook.Domain.Models.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlastAsia.DigiBook.API.Utils
{
    public static class EmployeeExtensions
    {
        public static Employee ApplyChanges(this Employee employee, Employee from)
        {
            employee.FirstName = from.FirstName;
            employee.LastName = from.LastName;
            employee.MobilePhone = from.MobilePhone;
            employee.EmailAddress = from.EmailAddress;
            employee.Photo = from.Photo;
            employee.OfficePhone = from.OfficePhone;
            employee.Extension = from.Extension;

            return employee;
        }
    }
}
