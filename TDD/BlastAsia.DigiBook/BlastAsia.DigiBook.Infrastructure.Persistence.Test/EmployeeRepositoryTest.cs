
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
        private string connectionString;
        private DbContextOptions<DigiBookDbContext> dbOptions = null;
        private EmployeeRepository sut;
        private DigiBookDbContext dbContext;
 
        [TestInitialize()]
        public void Initialize()
        {
            employee = new Employee
            {
                FirstName = "Ryan Karl",
                LastName = "Oribello",
                MobilePhone = "09264709989",
                EmailAddress = "oribelloryan@gmail.com",
                Photo = new MemoryStream(),
                OfficePhone = "432-232-244",
                Extension = "101"
            };

            connectionString =
               @"Data Source=.; Database=DigiBookDb; Integrated Security=true";

            dbOptions = new DbContextOptionsBuilder<DigiBookDbContext>()
                .UseSqlServer(connectionString)
                .Options;
            dbContext = new DigiBookDbContext(dbOptions);
            dbContext.Database.EnsureCreated();
            sut = new EmployeeRepository(dbContext);


        }
        [TestCleanup()]
        public void CleanUp()
        {

        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Create_WithValidData_SaveRecordInTheDatabase()
        {
            //Arrange

            //Act
            var newEmployee = sut.Create(employee);

            //Assert
            Assert.IsNotNull(employee);
            Assert.IsTrue(newEmployee.EmployeeId != Guid.Empty);

            //CleanUp
            sut.Delete(newEmployee.EmployeeId);

        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Delete_WithAnExistingemployee_RemovesRecordFromDatabase()
        {
            var newEmployee = sut.Create(employee);
            //Act
            sut.Delete(newEmployee.EmployeeId);

            employee = sut.Retrieve(newEmployee.EmployeeId);
            Assert.IsNull(employee);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Retrieve_WithExistingemployeeId_ReturnsRecordFromDb()
        {

            var newEmployee = sut.Create(employee);

            var found = sut.Retrieve(newEmployee.EmployeeId);

            Assert.IsNotNull(found);
            sut.Delete(newEmployee.EmployeeId);

        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Update_WithValidData_SaveUpdatesInDb()
        {
            var newEmployee = sut.Create(employee);
            var expectedFirstName = "Linus";
            var expectedLastName = "Paul";
            var expectedEmail = "linusPauling@gmail.com";
            var expectedMobilePhone = "09999999999";
            var expectedOfficePhone = "123 - 123 - 123";
            var expectedExtension = "001";
              
            newEmployee.FirstName = expectedFirstName;
            newEmployee.LastName = expectedLastName;
            newEmployee.EmailAddress = expectedEmail;
            newEmployee.MobilePhone = expectedMobilePhone;
            newEmployee.OfficePhone = expectedOfficePhone;
            newEmployee.Extension = expectedExtension;

            sut.Update(newEmployee.EmployeeId, newEmployee);

            var updatedEmployee = sut.Retrieve(newEmployee.EmployeeId);
            Assert.AreEqual(expectedFirstName, updatedEmployee.FirstName);
            Assert.AreEqual(expectedLastName, updatedEmployee.LastName);
            Assert.AreEqual(expectedEmail, updatedEmployee.EmailAddress);
            Assert.AreEqual(expectedMobilePhone, updatedEmployee.MobilePhone);
            Assert.AreEqual(expectedOfficePhone, updatedEmployee.OfficePhone);
            Assert.AreEqual(expectedExtension, updatedEmployee.Extension);

            sut.Delete(updatedEmployee.EmployeeId);
        }
    }
}
