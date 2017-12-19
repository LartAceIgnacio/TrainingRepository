using BlastAsia.DigiBook.Domain.Models.Employees;
using BlastAsia.DigiBook.Infrastracture.Persistence.Repositories;
using BlastAsia.DigiBook.Insfrastracture.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BlastAsia.DigiBook.Infrastracture.Persistence.Test
{
    [TestClass]
    public class EmployeeRepositoryTest
    {
        private Employee employee; // model 
        private DbContextOptions<DigiBookDbContext> dbOptions; //
        private DigiBookDbContext dbContext;
        private readonly string connectionString = @"Data Source=.; Database=DigiBookDb; Integrated Security=true;";
        private EmployeeRepository sut;

        [TestInitialize]
        public void Initialize() {
            employee = new Employee
            {
                EmployeeId = new Guid(),
                FirstName = "Emmanuel",
                LastName = "Magadia",
                MobilePhone = "09279528841",
                EmailAddress = "emagadia@blastasia.com",
            //    Photo = new MemoryStream(),
                OfficePhone = "123-123-123",
                Extension = "asdasd"
            };

            dbOptions = new DbContextOptionsBuilder<DigiBookDbContext>()
                                   .UseSqlServer(connectionString)
                                   .Options;

            dbContext = new DigiBookDbContext(dbOptions); // ORM
            dbContext.Database.EnsureCreated();
            sut = new EmployeeRepository(dbContext); // System under test
        }
        
        [TestCleanup]
        public void Cleanup()
        {
            dbContext.Dispose();
            dbContext = null;
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Create_WithValidData_SavesRecordToDatabase()
        {
            // act
            var newEmployee = sut.Create(employee);

            // assert 
            Assert.IsNotNull(newEmployee);
            Assert.IsTrue(newEmployee.EmployeeId != Guid.Empty);

            // Cleanup
            sut.Delete(newEmployee.EmployeeId);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Delete_WithAnExistingEmployee_RemovesRecordFromDatabase()
        {
            // arrange 
            // var sut = new ContactRepository(dbContext); // System under test
            var newContact = sut.Create(employee);

            // act 
            sut.Delete(newContact.EmployeeId);
            // assert
            employee = sut.Retrieve(newContact.EmployeeId);
            Assert.IsNull(employee);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Retrieve_WithExistingEmployeeId_ReturnsRecordFromDatabase()
        {
            // arrange
            var newContact = sut.Create(employee);
            //act
            var found = sut.Retrieve(newContact.EmployeeId);
            // assert 
            Assert.IsNotNull(found);

            sut.Delete(newContact.EmployeeId);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Update_WithExistingEmployeeId_SaveAndUpdateInDatabase()
        {
            //arrange
            var newEmployee = sut.Create(employee);

            var expectedFirstName = "Kyrie"; // from emem
            var expectedLastName = "Irving";
            var expectedEmail = "kyrie@yahoo.com";
            var expectedMobileNumber = "09279528841";
            var expectedOfficePhone = "321-3123-132";
            var expectedExtention = "09279528841";

            newEmployee.FirstName = expectedFirstName;
            newEmployee.LastName = expectedLastName;
            newEmployee.EmailAddress = expectedEmail;
            newEmployee.MobilePhone = expectedMobileNumber;
            newEmployee.OfficePhone = expectedOfficePhone;
            newEmployee.Extension = expectedExtention;

            // act
            sut.Update(newEmployee.EmployeeId, newEmployee);

            var UpdatedContact = sut.Retrieve(newEmployee.EmployeeId);
            // assert 
            Assert.AreEqual(UpdatedContact.FirstName, expectedFirstName);
            Assert.AreEqual(UpdatedContact.LastName , expectedLastName);
            Assert.AreEqual(UpdatedContact.EmailAddress , expectedEmail);
            Assert.AreEqual(UpdatedContact.MobilePhone , expectedMobileNumber);
            Assert.AreEqual(UpdatedContact.OfficePhone , expectedOfficePhone);
            Assert.AreEqual(UpdatedContact.Extension , expectedExtention);

            // cleanup
            sut.Delete(UpdatedContact.EmployeeId);
        }
    }
}
