using BlastAsia.Digibook.Domain.Models.Employees;
using BlastAsia.Digibook.Infrastracture.Persistence;
using BlastAsia.Digibook.Infrastracture.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BlastAsia.Digibook.Infrastructure.Persistence.Test
{
    [TestClass]
    public class EmployeeRepositoryTest
    {
        private Employee employee;
        private DbContextOptions<DigiBookDbContext> dbOptions = null;
        private DigiBookDbContext dbContext = null;
        private string connectionString = null;
        private EmployeeRepository sut = null;

        [TestInitialize]
        public void Initialize()
        {
            employee = new Employee
            {
                FirstName = "Glenn Alexander",
                LastName = "Cano",
                MobilePhone = "09173723594",
                EmailAddress = "gcano@blastasia.com",
                //Photo = new MemoryStream(),
                OfficePhone = "9144456",
                Extension = "421",
                PhotoByte = new byte[4]
            };

            connectionString = @"Data Source=.;Database=DigiBookDb;Integrated Security=true;";
            dbOptions = new DbContextOptionsBuilder<DigiBookDbContext>()
                                    .UseSqlServer(connectionString)
                                    .Options;
            dbContext = new DigiBookDbContext(dbOptions);
            dbContext.Database.EnsureCreated();
            sut = new EmployeeRepository(dbContext);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            dbContext.Dispose();
            dbContext = null;
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Create_WithValidData_SavesRecordInTheDatabase()
        {
            var newEmployee = sut.Create(employee);

            Assert.IsNotNull(newEmployee);
            Assert.IsTrue(newEmployee.EmployeeId != Guid.Empty);

            sut.Delete(newEmployee.EmployeeId);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Delete_WithAnExistingContact_RemoveRecordFromDatabase()
        {
            var newEmployee = sut.Create(employee);

            sut.Delete(newEmployee.EmployeeId);
            employee = sut.Retrieve(newEmployee.EmployeeId);
            Assert.IsNull(employee);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Retrieve_WithExistingContactId_ReturnsRecordFromDb()
        {
            var newEmployee = sut.Create(employee);

            var found = sut.Retrieve(newEmployee.EmployeeId);

            Assert.IsNotNull(found);

            sut.Delete(found.EmployeeId);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Update_WithValidData_SavesUpdatesInDb()
        {
            var newEmployee = sut.Create(employee);

            var expectedFirstName = "Alex";
            var expectedLastName = "Onac";
            var expectedMobilePhone = "987654321";
            var expectedEmailAddress = "gcano@a.com";
            var expectedOfficePhone = "9111111";
            var expectedExtension = "420";
            var expectedPhotoByte = new byte[2];

            newEmployee.FirstName = expectedFirstName;
            newEmployee.LastName = expectedLastName;
            newEmployee.MobilePhone = expectedMobilePhone;
            newEmployee.EmailAddress = expectedEmailAddress;
            newEmployee.OfficePhone = expectedOfficePhone;
            newEmployee.Extension = expectedExtension;
            newEmployee.PhotoByte = expectedPhotoByte;

            sut.Update(newEmployee.EmployeeId, newEmployee);
            var updatedEmployee = sut.Retrieve(newEmployee.EmployeeId);

            Assert.AreEqual(expectedFirstName, newEmployee.FirstName);
            Assert.AreEqual(expectedLastName, newEmployee.LastName);
            Assert.AreEqual(expectedMobilePhone, newEmployee.MobilePhone);
            Assert.AreEqual(expectedEmailAddress, newEmployee.EmailAddress);
            Assert.AreEqual(expectedOfficePhone, newEmployee.OfficePhone);
            Assert.AreEqual(expectedExtension, newEmployee.Extension);
            Assert.AreEqual(expectedPhotoByte, newEmployee.PhotoByte);

            sut.Delete(updatedEmployee.EmployeeId);
        }
    }
}
