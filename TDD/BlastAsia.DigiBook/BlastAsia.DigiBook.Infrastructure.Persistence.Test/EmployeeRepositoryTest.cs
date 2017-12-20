using BlastAsia.DigiBook.Domain.Models.Employees;
using BlastAsia.DigiBook.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Infrastructure.Persistence.Test
{
    [TestClass]
    public class EmployeeRepositoryTest
    {
        private Employee employee = null;
        private DbContextOptions<DigiBookDbContext> dbOptions = null;
        private DigiBookDbContext dbContext = null;
        private String connectionString = null;
        private EmployeeRepository sut = null;

        [TestInitialize]
        public void InitializeTest()
        {

            employee = new Employee
            {
                FirstName = "Luigi",
                LastName = "Abille",
                MobilePhone = "09568717617",
                EmailAddress = "luigiabille@gmail.com",
                Photo = "sample.jpg",
                OfficePhone = "9111111",
                Extension = "121"
            };
            connectionString
                = @"Data Source=.;Database=DigiBookDb;Integrated Security=true;";
            dbOptions = new DbContextOptionsBuilder<DigiBookDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            dbContext = new DigiBookDbContext(dbOptions); // ORM
            dbContext.Database.EnsureCreated();

            sut = new EmployeeRepository(dbContext);
        }

        [TestCleanup]
        public void CleanupTest()
        {
            dbContext.Dispose();
            dbContext = null;
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Create_WithValidData_SavesRecordInTheDatabase()
        {
            // Act
            var newEmployee = sut.Create(employee);

            // Assert
            Assert.IsNotNull(newEmployee);
            Assert.IsTrue(newEmployee.EmployeeId != Guid.Empty);

            // Cleanup
            sut.Delete(newEmployee.EmployeeId);
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
            Assert.IsNotNull(found);

            // Cleanup
            sut.Delete(found.EmployeeId);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Update_WithValidData_SavesUpdatesInDb()
        {
            // Arrange
            var newEmployee = sut.Create(employee);
            var expectedFirstName = "Antonio";
            var expectedLastName = "Tan";
            var expectedMobilePhone = "34234";
            var expectedEmailAddress = "3243@gmail.com";
            var expectedPhoto = "3423.png";
            var expectedOfficePhone = "32423";
            var expectedExtension = "14";

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

            // Cleanup
            sut.Delete(updatedEmployee.EmployeeId);

        }
    }
}