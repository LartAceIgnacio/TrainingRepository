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
                employeeId = new Guid(),
                firstName = "John Karl",
                lastName = "Matencio",
                mobilePhone = "09957206817",
                emailAddress = "jhnkrl15@gmail.com",
                photo = new MemoryStream(),
                officePhone = "4848766",
                extension = "1001"
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
            Assert.IsTrue(newEmployee.employeeId != Guid.Empty);
            //Cleanup
            sut.Delete(newEmployee.employeeId);
        }

        [TestMethod]
        [TestProperty("TestType","Integration")]
        public void Delete_WithExistingEmployee_RemovesRecordToTheDatabase()
        {
            //Arrange
            var newEmployee = sut.Create(employee);
            //Act
            sut.Delete(employee.employeeId);
            //Assert
            employee = sut.Retrieve(employee.employeeId);
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
            var found = sut.Retrieve(employee.employeeId);
            //Assert
            Assert.IsNotNull(found);
            //Cleanup
            sut.Delete(found.employeeId);
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

            newEmployee.firstName = expectedFirstname;
            newEmployee.lastName = expectedLastname;
            newEmployee.mobilePhone = expectedMobilePhone;
            newEmployee.emailAddress = expectedEmailAddress;
            //Act
            sut.Update(newEmployee.employeeId, employee);
            //Assert
            var updated = sut.Retrieve(newEmployee.employeeId);

            Assert.AreEqual(newEmployee.firstName, updated.firstName);
            Assert.AreEqual(newEmployee.lastName, updated.lastName);
            Assert.AreEqual(newEmployee.mobilePhone, updated.mobilePhone);
            Assert.AreEqual(newEmployee.emailAddress, updated.emailAddress);
            //Cleanup
            sut.Delete(updated.employeeId);
        }
    }
}
