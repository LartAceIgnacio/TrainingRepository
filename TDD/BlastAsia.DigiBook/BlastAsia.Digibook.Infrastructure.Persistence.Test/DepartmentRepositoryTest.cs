using BlastAsia.DigiBook.Domain.Models.Departments;
using BlastAsia.DigiBook.Infrastructure.Persistence;
using BlastAsia.DigiBook.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlastAsia.Digibook.Infrastructure.Persistence.Test
{
    [TestClass]
    public class DepartmentRepositoryTest
    {
        private Department department;
        private DbContextOptions<DigiBookDbContext> dbOptions;
        private DigiBookDbContext dbContext;
        private String connectionString;
        private DepartmentRepository sut;
        private EmployeeRepository sutEmployee;

        [TestInitialize]
        public void Initialize()
        {
            connectionString =
                @"Server=.;Database=DigiBookDb;Integrated Security=true;";
            dbOptions = new DbContextOptionsBuilder<DigiBookDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            dbContext = new DigiBookDbContext(dbOptions);
            dbContext.Database.EnsureCreated();

            sut = new DepartmentRepository(dbContext);
            sutEmployee = new EmployeeRepository(dbContext);

            department = new Department
            {
                DepartmentName = "IT",
                DepartmentHeadId = sutEmployee.Retrieve().FirstOrDefault().EmployeeId
            };
        }

        [TestCleanup]
        public void Cleanup()
        {
            dbContext.Dispose();
            dbContext = null;
        }

        [TestMethod]
        [TestProperty("TestType","Integration")]
        public void Create_DepartmentWithValidData_SavesRecordInTheDatabase()
        {
            // Act
            var newDepartment = sut.Create(department);

            // Assert
            Assert.IsNotNull(newDepartment);
            Assert.IsTrue(newDepartment.DepartmentId != Guid.Empty);

            // Cleanup
            sut.Delete(department.DepartmentId);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Delete_WithAnExistingDepartment_RemovesRecordFromDatabase()
        {
            // Arrange
            var newDepartment = sut.Create(department);

            // Act
            sut.Delete(newDepartment.DepartmentId);

            // Assert
            department = sut.Retrieve(newDepartment.DepartmentId);
            Assert.IsNull(department);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Retrieve_WithExistingDepartmentId_ReturnsRecordFromDb()
        {
            // Act
            var newDepartment = sut.Create(department);

            // Assert
            Assert.IsNotNull(newDepartment);
            Assert.IsTrue(newDepartment.DepartmentId != Guid.Empty);

            // Cleanup
            sut.Delete(department.DepartmentId);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Update_WithValidData_SavesUpdatesInDb()
        {
            // Arrange
            var newDepartment = sut.Create(department);
            var expectedDepartmentName = "HR";
            var expectedDepartmentHeadId = department.DepartmentHeadId;

            newDepartment.DepartmentName = expectedDepartmentName;
            newDepartment.DepartmentHeadId = expectedDepartmentHeadId;

            // Act
            sut.Update(newDepartment.DepartmentId, newDepartment);

            // Assert
            var updatedDepartment = sut.Retrieve(newDepartment.DepartmentId);
            Assert.AreEqual(expectedDepartmentName, updatedDepartment.DepartmentName);
            Assert.AreEqual(expectedDepartmentHeadId, updatedDepartment.DepartmentHeadId);

            //Cleanup
            sut.Delete(updatedDepartment.DepartmentId);
        }
    }
}
