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
        public void TestInitialize()
        {
            employee = new Employee
            {
                FirstName = "Gelo",
                LastName = "Celis",
                MobilePhone = "09279553255",
                EmailAddress = "anjacelis21@gmail.com",
                Photo = "Photo",
                OfficePhone = "8569521",
                Extension = "09"

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
        public void TestCleanUp()
        {
            dbContext.Dispose();
            dbContext = null;
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Create_WithValidData_SavesRecordsInTheDatabase()
        {



            //Act
            var newEmployee = sut.Create(employee);

            //Assert
            Assert.IsNotNull(newEmployee);
            Assert.IsTrue(newEmployee.EmployeeId != Guid.Empty);

            //CleanUp

            sut.Delete(newEmployee.EmployeeId);

        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Delete_WithAnExistingContact_RemovesRecordFromData()
        {
            //Arrange

            var newEmployee = sut.Create(employee);

            //Act
            sut.Delete(newEmployee.EmployeeId);

            //Assert
            employee = sut.Retrieve(newEmployee.EmployeeId);
            Assert.IsNull(employee);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Retrieve_WithExistingContactId_ReturnsRecordFromDb()
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
            var expectedFirstName = "Angelou"; //from Gelo
            var expectedLastName = "Acosta"; //from celis




            newEmployee.FirstName = expectedFirstName;
            newEmployee.LastName = expectedLastName;


            //Act

            sut.Update(newEmployee.EmployeeId, newEmployee);

            //Assert
            var updatedContact = sut.Retrieve(newEmployee.EmployeeId);
            Assert.AreEqual(expectedFirstName, updatedContact.FirstName);
            Assert.AreEqual(expectedLastName, updatedContact.LastName);


            //CleanUp
            sut.Delete(updatedContact.EmployeeId);
        }
    }
}
