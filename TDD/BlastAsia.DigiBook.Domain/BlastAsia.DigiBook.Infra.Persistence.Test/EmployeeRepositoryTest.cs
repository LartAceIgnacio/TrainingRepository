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
        public Employee employee = null;
        public string connectionString = null;
        public DbContextOptions<DigiBookDbContext> dbOptions = null;
        public DigiBookDbContext dbContext = null;
        public EmployeeRepository sut = null;

        [TestInitialize]
        public void Initialize()
        {
            employee = new Employee {
                FirstName = "Chris",
                LastName = "Manuel",
                MobilePhone = "09156879240",
                EmailAddress = "cmanuel@blastasia.com",
                Photo = new MemoryStream(new byte[16]),
                OfficePhone = "758-5224",
                Extension = "02",
                PhotoByte = new byte[16]
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
        public void Cleanup()
        {
            dbContext.Dispose();
            dbContext = null;
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Create_WithValidData_SavesRecordInTheDatabase()
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
        [TestProperty("TestType", "Integration")]
        public void Delete_WithExistingEmployee_RemovesRecordFromDatabase()
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
        public void Retrieve_WithExistingEmployeeId_ReturnsRecordFromDatabase()
        {
            //Arrange
            var newEmployee = sut.Create(employee);

            //Act
            var found = sut.Retrieve(newEmployee.EmployeeId);

            //Assert
            Assert.IsNotNull(found);

            //Cleanup
            sut.Delete(newEmployee.EmployeeId);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Update_WithValidData_SavesUpdatesInDatabase()
        {
            //Arrange
            var newEmployee = sut.Create(employee);
            var expectedFirstName = "Topeng";
            var expectedLastName = "Leunam";
            var expectedMobileNo = "09263135367";
            var expectedEmailAddress = "topengleunam@blastasia.com";
            var expectedOfficePhone = "741-5541";
            var expectedExtension = "13";
            var expectedPhotoByte = new byte[15];

            newEmployee.FirstName = expectedFirstName;
            newEmployee.LastName = expectedLastName;
            newEmployee.MobilePhone = expectedMobileNo;
            newEmployee.EmailAddress = expectedEmailAddress;
            newEmployee.OfficePhone = expectedOfficePhone;
            newEmployee.Extension = expectedExtension;
            newEmployee.PhotoByte = expectedPhotoByte;

            //Act
            sut.Update(newEmployee.EmployeeId, newEmployee);

            //Assert
            var updatedContact = sut.Retrieve(newEmployee.EmployeeId);
            Assert.AreEqual(updatedContact.FirstName, newEmployee.FirstName);
            Assert.AreEqual(updatedContact.LastName, newEmployee.LastName);
            Assert.AreEqual(updatedContact.MobilePhone, newEmployee.MobilePhone);
            Assert.AreEqual(updatedContact.EmailAddress, newEmployee.EmailAddress);
            Assert.AreEqual(updatedContact.OfficePhone, newEmployee.OfficePhone);
            Assert.AreEqual(updatedContact.Extension, newEmployee.Extension);
            Assert.AreEqual(updatedContact.PhotoByte, newEmployee.PhotoByte);

            //Cleanup
            sut.Delete(newEmployee.EmployeeId);
        }
    }
}
