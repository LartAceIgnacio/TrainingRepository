using BlastAsia.DigiBook.Domain.Models.Departments;
using BlastAsia.DigiBook.Infrastructure.Persistence;
using BlastAsia.DigiBook.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.Digibook.Infrastructure.Persistence.Test
{
    [TestClass]
    public class DepartmentRepositoryTest
    {
        private Department department;
        private string connectionString;
        private DbContextOptions<DigiBookDbContext> dbOptions;
        private DigiBookDbContext dbContext;
        private DepartmentRepository sut;

        [TestInitialize]
        public void Initialize()
        {
            department = new Department
            {
                DepartmentName = "IT",
                DepartmentHeadId = Guid.NewGuid()
            };

            connectionString = @"Server=.;Database=DigiBookDb;Integrated Security=true";
            dbOptions = new DbContextOptionsBuilder<DigiBookDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            dbContext = new DigiBookDbContext(dbOptions);
            dbContext.Database.EnsureCreated();

            sut = new DepartmentRepository(dbContext);
        }

        [TestCleanup]
        public void CleanUp()
        {
            dbContext.Dispose();
            dbContext = null;
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Create_DepartmentWithValidData_SavesRecordInTheDatabase()
        { 
            // Act
            var newDepartment = sut.Create(department);

            // Assert
            Assert.IsNotNull(newDepartment);
            Assert.IsTrue(newDepartment.DepartmentId != null);

            //CleanUp
            sut.Delete(newDepartment.DepartmentId);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Retrieve_WithExistingDepartmentId_ReturnsRecordFromDb()
        {
            // Arrange
            var newDepartment = sut.Create(department);

            // Act
            var found =sut.Retrieve(newDepartment.DepartmentId);

            //Assert
            Assert.IsNotNull(newDepartment);

            // CleanUp
            sut.Delete(found.DepartmentId);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Update_WithExistingDepartmentData_UpdatesRecordInDb()
        {
            // Arrange
            var newDepartment = sut.Create(department);

            var expectedDepartmentName = "HR";
            var expectedDeparmentHeadId = Guid.NewGuid();

            newDepartment.DepartmentName = expectedDepartmentName;
            newDepartment.DepartmentHeadId = expectedDeparmentHeadId;

            // Act
            sut.Update(newDepartment.DepartmentId, newDepartment);

            // Assert
            var updatedDepartment = sut.Retrieve(newDepartment.DepartmentId);

            Assert.AreEqual(expectedDepartmentName, updatedDepartment.DepartmentName);
            Assert.AreEqual(expectedDeparmentHeadId, updatedDepartment.DepartmentHeadId);

            //CleanUp
            sut.Delete(updatedDepartment.DepartmentId);
        }
    }
}
