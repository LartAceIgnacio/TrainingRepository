using BlastAsia.DigiBook.Domain.Models.Employees;
using BlastAsia.DigiBook.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BlastAsia.DigiBook.Infrastructure.Persistence.Test
{
    [TestClass]
    public class EmployeeRepositoryTest
    {
        private Employee employee = null;
        private string connectionString;
        private DbContextOptions<DigiBookDbContext> dbOptions;
        private DigiBookDbContext dbContext;
        private EmployeeRepository sut;

        [TestInitialize]
        public void Initialize()
        {
            employee = new Employee
            {
                 Firstname = "Mattronilo",
                 Lastname = "Mendez",
                 MobilePhone = "09293235700",
                 EmailAddress = "mmendez@blastasia.com",
                 OfficePhone = "1234567",
                 Extension = "02",
                 //Photo = new MemoryStream()
            };

            connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=DigiBookDb;Trusted_Connection=True;";
            dbOptions = new DbContextOptionsBuilder<DigiBookDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            dbContext = new DigiBookDbContext(dbOptions);

            sut = new EmployeeRepository(dbContext);
            dbContext.Database.EnsureCreated();
        }

        [TestCleanup]
        public void CleanUp()
        {
            dbContext.Dispose();
            dbContext = null;
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Create_WithValidEmployeeData_SaveRecordInTheDatabase()
        {
            // Arrange

            // Act
            var newEmployee = sut.Create(employee);

            //Assert
            Assert.IsNotNull(newEmployee);
            Assert.IsTrue(newEmployee.Id != Guid.Empty);


            // Cleanup
            sut.Delete(newEmployee.Id);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Delete_WithExistingEmployeeId_RemovesDataFromDatabase()
        {
            // Arrange
            var newEmployee = sut.Create(employee);
            // Act
            sut.Delete(newEmployee.Id);

            // Assert
            employee = sut.Retrieve(newEmployee.Id);
            Assert.IsNull(employee);

        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Retrieve_WithExistingEmployeeId_ReturnsRecordFromDb()
        {
            // Arrange
            var newEmployee = sut.Create(employee);

            // Act
            var found = sut.Retrieve(newEmployee.Id);

            // Assert
            Assert.IsNotNull(found);

            //Cleanup
            sut.Delete(found.Id);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Update_WithExistingEmployeeId_ShouldReturnRecordFromDb()
        {
            // Arrange
            var newEmployee = sut.Create(employee);
            var expectedFirstname = "Robert";
            var expectedLastname = "Martin";
            var expectedEmail = "rmartin@blastasia.com";
            var expectedPhoto = new MemoryStream();

            newEmployee.Lastname = expectedLastname;
            newEmployee.Firstname = expectedFirstname;
            newEmployee.EmailAddress = expectedEmail;
            //newEmployee.Photo = expectedPhoto;

            // Act
            sut.Update(newEmployee.Id, employee);

            // Assert
            var updatedEmployee = sut.Retrieve(newEmployee.Id);
            Assert.AreEqual(expectedFirstname, updatedEmployee.Firstname);
            Assert.AreEqual(expectedLastname, updatedEmployee.Lastname);
            Assert.AreEqual(expectedEmail, updatedEmployee.EmailAddress);
            //Assert.AreEqual(expectedPhoto, updatedEmployee.Photo);

            // Cleanup
            sut.Delete(updatedEmployee.Id);
        }


    }
}
