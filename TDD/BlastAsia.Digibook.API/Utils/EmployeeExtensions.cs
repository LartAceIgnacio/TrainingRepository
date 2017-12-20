using BlastAsia.Digibook.Domain.Models.Employees;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BlastAsia.Digibook.API.Utils
{
    public static class EmployeeExtensions
    {
        public static Employee ApplyEmployeeChanges(this Employee sourceEmployee,Employee destinationEmployee)
        {
            destinationEmployee.FirstName = sourceEmployee.FirstName;
            destinationEmployee.LastName = sourceEmployee.LastName;
            destinationEmployee.MobilePhone = sourceEmployee.MobilePhone;
            destinationEmployee.EmailAddress = sourceEmployee.EmailAddress;
            destinationEmployee.OfficePhone = sourceEmployee.OfficePhone;
            destinationEmployee.Extension = sourceEmployee.Extension;
            destinationEmployee.Photo = new MemoryStream();
            destinationEmployee.PhotoByte = sourceEmployee.PhotoByte;

            return destinationEmployee;
        }
    }
}
