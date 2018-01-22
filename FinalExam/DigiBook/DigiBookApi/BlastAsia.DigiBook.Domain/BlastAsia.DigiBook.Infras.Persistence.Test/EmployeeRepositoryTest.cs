using BlastAsia.DigiBook.Domain.Models.Employees;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace BlastAsia.DigiBook.Infrastructure.Persistence.Test
{
    [TestClass]
    public class EmployeeRepositoryTest
    {
        private string connectionString;

        private Employee employee;
        private EmployeeRepository sut;
        private DigiBookDbContext dbContext = null;
        private DbContextOptions<DigiBookDbContext> dbOptions = null;

        [TestInitialize]
        public void InitializeTest()
        {
            employee = new Employee
            {
                FirstName = "Jhoane",
                LastName = "Garcia",
                MobilePhone = "09123456789",
                EmailAddress = "jgarcia@gmail.com",
                Photo = new MemoryStream(),
                OfficePhone = "9312062",
                Extension = "012"
            };


            connectionString = @"Data Source=.;Database=DigiBookDb;Integrated Security=true;";
            dbOptions = new DbContextOptionsBuilder<DigiBookDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            dbContext = new DigiBookDbContext(dbOptions); // ORM
            dbContext.Database.EnsureCreated();
            sut = new EmployeeRepository(dbContext);
        }

        [TestCleanup]
        public void CleanupTest()
        {
            dbContext.Dispose();
            dbContext = null;
        }

        [TestMethod]
        [TestProperty("TestType", "Employee")]
        public void Create_WithValidData_SavesRecordToDatabase()
        {

            //Act
            var newEmployee = sut.Create(employee);

            //Assert
            Assert.IsNotNull(employee);
            Assert.IsTrue(newEmployee.EmployeeId != Guid.Empty);


            // Cleanup
            sut.Delete(newEmployee.EmployeeId);

        }

        [TestMethod]
        [TestProperty("TestType", "Employee")]
        public void Delete_WithAnExistingEmployeeId_DeletesRecordToDatabase()
        {
            //Arrange
            var newEmployee = sut.Create(employee);

            //Act
            sut.Delete(employee.EmployeeId);

            //Arrange
            employee = sut.Retrieve(newEmployee.EmployeeId);
            Assert.IsNull(employee);
        }

        [TestMethod]
        [TestProperty("TestType", "Employee")]
        public void Update_WithAnExistingEmployeeId_ShouldUpdateEmployeeRecordToDatabase()
        {
            var newEmployee = sut.Create(employee);
            var expectedFirstName = "Rehehenz";

            //Chaning the values
            newEmployee.FirstName = expectedFirstName;
            //act
            sut.Update(newEmployee.EmployeeId, employee);
            var updatedEmployee = sut.Retrieve(newEmployee.EmployeeId);


            //assert

            Assert.AreEqual(updatedEmployee.FirstName, expectedFirstName);

            sut.Delete(updatedEmployee.EmployeeId);
        }

        [TestMethod]
        [TestProperty("TestType", "Employee")]
        public void Retrieve_WithAnExistingEmployeeId_ReturnsEmployeeRecordsFromDb()
        {
            var newEmployee = sut.Create(employee);

            var retrieveEmployee = sut.Retrieve(newEmployee.EmployeeId);

            Assert.IsNotNull(retrieveEmployee);

            sut.Delete(newEmployee.EmployeeId);
        }
    }
}
