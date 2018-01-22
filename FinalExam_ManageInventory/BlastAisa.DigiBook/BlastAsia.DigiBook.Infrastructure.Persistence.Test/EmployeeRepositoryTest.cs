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
        private DbContextOptions<DigiBookDbContext> dbOptions = null;
        private DigiBookDbContext dbContext = null;
        private String connectionString = null;
        private EmployeeRepository sut;

        [TestInitialize]
        public void TestInitialize()
        {
            employee = new Employee
            {
                EmployeeId = new Guid(),
                FirstName = "John Karl",
                LastName = "Matencio",
                MobilePhone = "09957206817",
                EmailAddress = "jhnkrl15@gmail.com",
                Photo = new MemoryStream(),
                OfficePhone = "4848766",
                Extension = "1001"
            };

            connectionString =
                @"Data Source=.;Database=DigiBookDb;Integrated Security=true;";

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
        public void Create_WithValidData_SavesRecordToTheDatabase()
        {
            //Arrange
            //Act
            var newEmployee = sut.Create(employee);
            //Asset
            Assert.IsNotNull(newEmployee);
            Assert.IsTrue(newEmployee.EmployeeId != Guid.Empty);
            //Cleanup
            sut.Delete(newEmployee.EmployeeId);
        }

        [TestMethod]
        [TestProperty("TestType","Integration")]
        public void Delete_WithExistingEmployee_RemovesRecordToTheDatabase()
        {
            //Arrange
            var newEmployee = sut.Create(employee);
            //Act
            sut.Delete(employee.EmployeeId);
            //Assert
            employee = sut.Retrieve(employee.EmployeeId);
            Assert.IsNull(employee);
            //Cleanup
        }

        [TestMethod]
        [TestProperty("TestType","Integration")]
        public void Retrieve_WithExistingData_ReturnsRecordFromDatabase()
        {
            //Arrange
            var newEmployee = sut.Create(employee);
            //Act
            var found = sut.Retrieve(employee.EmployeeId);
            //Assert
            Assert.IsNotNull(found);
            //Cleanup
            sut.Delete(found.EmployeeId);
        }

        [TestMethod]
        [TestProperty("TestType","Integration")]
        public void Update_WithExistingData_UpdatesRecordFromDatabaset()
        {
            //Arrange
            var newEmployee = sut.Create(employee);

            var expectedFirstname = "Maureen"; //John Karl
            var expectedLastname = "Sebastian"; //Matencio
            var expectedMobilePhone = "09161468582"; //09957206817
            var expectedEmailAddress = "jmatencio@gmail.com"; //jhnkrl15@gmail.com

            newEmployee.FirstName = expectedFirstname;
            newEmployee.LastName = expectedLastname;
            newEmployee.MobilePhone = expectedMobilePhone;
            newEmployee.EmailAddress = expectedEmailAddress;
            //Act
            sut.Update(newEmployee.EmployeeId, employee);
            //Assert
            var updated = sut.Retrieve(newEmployee.EmployeeId);

            Assert.AreEqual(newEmployee.FirstName, updated.FirstName);
            Assert.AreEqual(newEmployee.LastName, updated.LastName);
            Assert.AreEqual(newEmployee.MobilePhone, updated.MobilePhone);
            Assert.AreEqual(newEmployee.EmailAddress, updated.EmailAddress);
            //Cleanup
            sut.Delete(updated.EmployeeId);
        }
    }
}
