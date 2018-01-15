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
        private EmployeeRepository sut;

        [TestInitialize]
        public void Initialize()
        {
            employee = new Employee
            {
                FirstName = "Eugene",
                LastName = "Ravina",
                MobilePhone = "09277109530",
                EmailAddress = "eravina@blastasia.com",
                OfficePhone = "373-30-32",
                Extension = "Extension",
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
        public void Create_EmployeeWithValidData_SavesRecordInTheDatabase()
        {

            //Act
            var newEmployee= sut.Create(employee);

            //Assert
            Assert.IsNotNull(newEmployee);
            Assert.IsTrue(newEmployee.EmployeeId != Guid.Empty);

            //Cleanup
            sut.Delete(newEmployee.EmployeeId);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Delete__EmployeeWithAnExistingEmployee_RemovesRecordFromDatabase()
        {

            sut = new EmployeeRepository(dbContext);
            var newEmployee = sut.Create(employee);

            // Act

            sut.Delete(newEmployee.EmployeeId);

            // Assert
            employee = sut.Retrieve(newEmployee.EmployeeId);
            Assert.IsNull(employee);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Retrieve_EmployeeWithExistingEmployeeId_ReturnsRecordFromDb()
        {
            //Arrange
            var newEmployee = sut.Create(employee);


            //Act
            var found = sut.Retrieve(newEmployee.EmployeeId);

            //Assert
            Assert.IsNotNull(found);

            //Cleanup
            sut.Delete(found.EmployeeId);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Update_EmployeeWithValidData_SavesUpdatesInDb()
        {
            //Arrange
            var newEmployee = sut.Create(employee);
            var expectedFirstName = "Vincent"; //from Eugene
            var expectedLastName = "Taguro"; //from Ravina
            var expectedMobilePhone = "064521";
            var expectedEmailAddress = "eugenejhonravina@yahoo.com"; // from eravina@blastasia.com
            var expectedOfficePhone = "922-12-64";
            var expectedExtension = "123";

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

            //Cleanup
            sut.Delete(updatedEmployee.EmployeeId);
        }
    }
}
