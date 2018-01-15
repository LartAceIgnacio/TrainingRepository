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
            // var sut = new employeeRepository(dbContext); // System under test
            var newEmployee = sut.Create(employee);

            // act 
            sut.Delete(newEmployee.EmployeeId);
            // assert
            employee = sut.Retrieve(newEmployee.EmployeeId);
            Assert.IsNull(employee);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Retrieve_WithExistingEmployeeId_ReturnsRecordFromDatabase()
        {
            // arrange
            var newEmployee = sut.Create(employee);
            //act
            var found = sut.Retrieve(newEmployee.EmployeeId);
            // assert 
            Assert.IsNotNull(found);

            sut.Delete(newEmployee.EmployeeId);
        }


         [TestMethod]
        public void Retrieve_WithPaginationWithValidData_ReturnsRecordFromDatabase()
        {
            // arrange
            var newEmployee = sut.Create(employee);
            var pageNumber = 1;
            var recordNumber = 5;
            var keyWord = "em";
            // act 
            var found = sut.Retrieve(pageNumber,recordNumber, keyWord);
            // assert
            Assert.IsNotNull(found);

            sut.Delete(newEmployee.EmployeeId);
        }


        [TestMethod]
        public void Retrieve_WithInvalidKeyWord_ReturnsDefaultRecordFromDataBase()
        {
            // arrange
            var newEmployee = sut.Create(employee);
            var pageNumber = 1;
            var recordNumber = 5;
            var keyWord = "";
            // act 
            var found = sut.Retrieve(pageNumber, recordNumber, keyWord);
            // assert
            Assert.IsNotNull(found);

            sut.Delete(newEmployee.EmployeeId);
        }


        [TestMethod]
        public void Retrieve_WithInvalidPageNumber_ReturnsDefaultRecordFromDataBase()
        {
            // arrange
            var newEmployee = sut.Create(employee);
            var pageNumber = -1;
            var recordNumber = 5;
            var keyWord = "em";
            // act 
            var found = sut.Retrieve(pageNumber, recordNumber, keyWord);
            // assert
            Assert.IsNotNull(found);

            sut.Delete(newEmployee.EmployeeId);
        }

        [TestMethod]
        public void Retrieve_WithInvalidRecordNumber_ReturnsDefaultRecordFromDataBase()
        {
            // arrange
            var newEmployee = sut.Create(employee);
            var pageNumber = 1;
            var recordNumber = -5;
            var keyWord = "em";
            // act 
            var found = sut.Retrieve(pageNumber, recordNumber, keyWord);
            // assert
            Assert.IsNotNull(found);

            sut.Delete(newEmployee.EmployeeId);
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

            var Updatedemployee = sut.Retrieve(newEmployee.EmployeeId);
            // assert 
            Assert.AreEqual(Updatedemployee.FirstName, expectedFirstName);
            Assert.AreEqual(Updatedemployee.LastName , expectedLastName);
            Assert.AreEqual(Updatedemployee.EmailAddress , expectedEmail);
            Assert.AreEqual(Updatedemployee.MobilePhone , expectedMobileNumber);
            Assert.AreEqual(Updatedemployee.OfficePhone , expectedOfficePhone);
            Assert.AreEqual(Updatedemployee.Extension , expectedExtention);

            // cleanup
            sut.Delete(Updatedemployee.EmployeeId);
        }
    }
}
