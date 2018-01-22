using BlastAsia.DigiBook.Domain.Models.Employees;
using BlastAsia.DigiBook.Infrastructure.Persistence;
using BlastAsia.DigiBook.Infrastructure.Persistence.Repositories;
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

        private Employee employee = null;
        private DbContextOptions<DigiBookDbContext> dbOptions = null;
        private DigiBookDbContext dbContext = null;
        private String connectionString = null;
        EmployeeRepository sut;

        [TestInitialize]
        public void Initialize()
        {
            employee = new Employee
            {
                FirstName = "Angela Blanche",
                LastName = "Olarte",
                MobilePhone = "09981642039",
                EmailAddress = "abbieolarte@gmail.com",
                Photo = new MemoryStream(),
                OfficePhone = "555222",
                Extension = "105"
            };

            connectionString =
                @"Server=.;Database=DigiBookDb;Integrated Security=true;";
            dbOptions = new DbContextOptionsBuilder<DigiBookDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            dbContext = new DigiBookDbContext(dbOptions);
            dbContext.Database.EnsureCreated();

            sut = new EmployeeRepository(dbContext);
        }

        [TestCleanup]
        public void Cleanup()
        {
            dbContext.Dispose();
            dbContext = null;
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Create_EmployeeWithValidData_SavesRecordInTheDatabase()
        {
            // Act
            var newEmployee = sut.Create(employee);

            // Assert
            Assert.IsNotNull(newEmployee);
            Assert.IsTrue(newEmployee.EmployeeId != Guid.Empty);

            // Cleanup
            sut.Delete(employee.EmployeeId);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Delete_WithAnExistingEmployee_RemovesRecordFromDatabase()
        {
            // Arrange            
            var newEmployee = sut.Create(employee);

            // Act
            sut.Delete(newEmployee.EmployeeId);

            // Assert
            employee = sut.Retrieve(newEmployee.EmployeeId);
            Assert.IsNull(employee);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Retrieve_WithExistingEmployeeId_ReturnsRecordFromDb()
        {
            // Arrange
            var newEmployee = sut.Create(employee);

            // Act
            var found = sut.Retrieve(newEmployee.EmployeeId);

            // Assert
            sut.Delete(found.EmployeeId);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Update_WithValidData_SavesUpdatesInDb()
        {
            // Arrange
            var newEmployee = sut.Create(employee);
            var expectedFirstName = "Angela";
            var expectedLastName = "Veluz";
            var expectedMobilePhone = "09981742039";
            var expectedEmailAddress = "abbieolarte@yahoo.com";
            var expectedPhoto = new MemoryStream();
            var expectedOfficePhone = "555222";
            var expectedExtension = "115";

            newEmployee.FirstName = expectedFirstName;
            newEmployee.LastName = expectedLastName;
            newEmployee.MobilePhone = expectedMobilePhone;
            newEmployee.EmailAddress = expectedEmailAddress;
            newEmployee.Photo = expectedPhoto;
            newEmployee.OfficePhone = expectedOfficePhone;
            newEmployee.Extension = expectedExtension;

            // Act
            sut.Update(newEmployee.EmployeeId, newEmployee);

            // Assert
            var updatedEmployee = sut.Retrieve(newEmployee.EmployeeId);
            Assert.AreEqual(expectedFirstName, updatedEmployee.FirstName);
            Assert.AreEqual(expectedLastName, updatedEmployee.LastName);
            Assert.AreEqual(expectedMobilePhone, updatedEmployee.MobilePhone);
            Assert.AreEqual(expectedEmailAddress, updatedEmployee.EmailAddress);
            Assert.AreEqual(expectedPhoto, updatedEmployee.Photo);
            Assert.AreEqual(expectedOfficePhone, updatedEmployee.OfficePhone);
            Assert.AreEqual(expectedExtension, updatedEmployee.Extension);

            //Cleanup
            sut.Delete(updatedEmployee.EmployeeId);
        }

    }
}
