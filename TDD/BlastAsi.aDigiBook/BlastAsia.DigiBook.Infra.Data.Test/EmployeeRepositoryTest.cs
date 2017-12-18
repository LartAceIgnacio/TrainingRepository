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
        private Employee employee;
        private DbContextOptions<DigiBookDbContext> dbOptions = null;
        private DigiBookDbContext dbContext = null;
        private String connectionString = null;
        private EmployeeRepository sut;

        [TestInitialize]
        public void Initialize()
        {
            employee = new Employee()
            {
                FirstName = "Jasmin",
                LastName = "Magdaleno",
                MobilePhone = "09057002880",
                EmailAddress = "jasminmagdaleno@blastasia.com",
                Photo = new MemoryStream(),
                OfficePhone = "5551212",
                Extension = "1"
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
        public void CleanUp()
        {
            dbContext.Dispose();
            dbContext = null;
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Create_WithValidData_SavesRecordInDatabase()
        {
            //Arrange
            
            //Act
            var newEmployee = sut.Create(employee);

            //Assert
            Assert.IsNotNull(newEmployee);
            Assert.IsTrue(newEmployee.EmployeeId != Guid.Empty);

            //CleanUp
            sut.Delete(newEmployee.EmployeeId);
        }

        [TestMethod]
        [TestProperty("TestType","Integration")]
        public void Delete_WithAnExistingEmployee_RemovesRecordFromDatabase()
        {
            //Arrange
            var newEmployee = sut.Create(employee);

            //Act
            sut.Delete(employee.EmployeeId);

            //Assert
            employee = sut.Retrieve(newEmployee.EmployeeId);
            Assert.IsNull(employee);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Retrieve_WithExistingEmployeeId_ReturnsRecordFromDb()
        {
            //Arrange
            var newEmployee = sut.Create(employee);

            //Act
            var found = sut.Retrieve(newEmployee.EmployeeId);

            //Assert
            Assert.IsNotNull(found);

            //CleanUp
            sut.Delete(found.EmployeeId);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Update_WithValidData_SavesUpdatesInDb()
        {
            //Arrange
            var newEmployee = sut.Create(employee);
            var expectedFirstName = "Sherlock"; //from Jasmin
            var expectedLastName = "Holmes"; //from Magdaleno
            var expectedMobilePhone = "09057002000"; //from 09057002991
            var expectedEmailAddress = "sHolmes@blastasia.com"; //from jasminmagdaleno@blastasia.com
            var expectedOfficePhone = "1234567"; //from 5551212
            var expectedExtension = "2"; //from 1

            newEmployee.FirstName = expectedFirstName;
            newEmployee.LastName = expectedLastName;
            newEmployee.MobilePhone = expectedMobilePhone;
            newEmployee.EmailAddress = expectedEmailAddress;
            newEmployee.OfficePhone = expectedOfficePhone;
            newEmployee.Extension = expectedExtension;

            //Act
            sut.Update(newEmployee.EmployeeId, newEmployee);

            //Assert
            var updatedEmployee = sut.Retrieve(newEmployee.EmployeeId);
            Assert.AreEqual(expectedFirstName, updatedEmployee.FirstName);
            Assert.AreEqual(expectedLastName, updatedEmployee.LastName);
            Assert.AreEqual(expectedMobilePhone, updatedEmployee.MobilePhone);
            Assert.AreEqual(expectedEmailAddress, updatedEmployee.EmailAddress);
            Assert.AreEqual(expectedOfficePhone, updatedEmployee.OfficePhone);
            Assert.AreEqual(expectedExtension, updatedEmployee.Extension);

            //CleanUp
            sut.Delete(updatedEmployee.EmployeeId);
        }
    }
}
