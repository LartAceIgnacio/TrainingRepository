using BlastAsia.DigiBook.Domain.Models.Departments;
using BlastAsia.DigiBook.Infrastructure.Persistence.Departments;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Infrastructure.Persistence.Test
{
    [TestClass]
    public class DepartmentRepositoryTest
    {
        private String connectionString = null;
        private DbContextOptions<DigiBookDbContext> dbOptions = null;
        private DigiBookDbContext dbContext = null;
        private DepartmentRepository sut = null;
        private Department department = null;

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Create_WithValidData_SavesRecordOnDatabase()
        {
            // Act
            var connectionString
                = @"Data Source=.;Database=DigiBookDb;Integrated Security=true;";

            var dbOptions = new DbContextOptionsBuilder<DigiBookDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            var dbContext = new DigiBookDbContext(dbOptions);

            dbContext.Database.EnsureCreated();

            var sut = new DepartmentRepository(dbContext);

            var department = new Department
            {
                DepartmentId = Guid.NewGuid(),
                Name = "sample"
            };

            var newDepartment = sut.Create(department);

            // Assert

            Assert.IsNotNull(department);
            Assert.IsTrue(newDepartment.DepartmentId != Guid.Empty);

            // Cleanup

            sut.Delete(newDepartment.DepartmentId);
        }
        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Retrieve_WithExistingDepartmentId_ReturnsRecordFromDatabase()
        {
            // Arrange
            var connectionString
                = @"Data Source=.;Database=DigiBookDb;Integrated Security=true;";

            var dbOptions = new DbContextOptionsBuilder<DigiBookDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            var dbContext = new DigiBookDbContext(dbOptions);

            dbContext.Database.EnsureCreated();
                
            var sut = new DepartmentRepository(dbContext);

            var department = new Department
            {
                DepartmentId = Guid.NewGuid(),
                Name = "Sample"
            };

            var newDepartment = sut.Create(department);

            // Act

            var found = sut.Retrieve(newDepartment.DepartmentId);

            // Assert

            Assert.IsNotNull(found);

            // Cleanup

            sut.Delete(found.DepartmentId);
        }
        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void MyTestMethod()
        {

        }
    }
}
