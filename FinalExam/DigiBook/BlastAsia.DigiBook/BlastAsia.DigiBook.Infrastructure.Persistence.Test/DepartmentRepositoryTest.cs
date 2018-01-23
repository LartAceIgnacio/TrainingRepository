using BlastAsia.DigiBook.Domain.Models.Departments;
using BlastAsia.DigiBook.Infrastructure.Persistence.Repositories;
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
        private Department department = null;
        private DbContextOptions<DigiBookDbContext> dbOptions = null;
        private DigiBookDbContext dbContext = null;
        private String connectionString = null;
        private DepartmentRepository sut;

        [TestInitialize]
        public void Initialize()
        {
            department = new Department
            {
                DepartmentName = "IT Department",
                DeparmentMemberId = Guid.NewGuid()

            };

            connectionString =
               @"Data Source=.;Database=DigiBookDb;Integrated Security=true;";
            dbOptions = new DbContextOptionsBuilder<DigiBookDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            dbContext = new DigiBookDbContext(dbOptions);
            dbContext.Database.EnsureCreated();

            sut = new DepartmentRepository(dbContext);
        }
        [TestCleanup]
        public void TestCleanup()
        {
            dbContext.Dispose();
            dbContext = null;
        }

        [TestMethod]
        [TestProperty("TestType","Integration")]
        public void Create_WithValidData_SavesRecordInTheDatabase()
        {
            //Arrange        
            //Act
            var newDepartment = sut.Create(department);


            //Assert
            Assert.IsNotNull(department);
            Assert.IsTrue(newDepartment.DepartmentId != Guid.Empty);

            //Clean Up
            sut.Delete(newDepartment.DepartmentId);
        }
        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Delete_WithAnExistingDepartment_RemovesRecordFromTheDatabase()
        {

            var newDepartment = sut.Create(department);

            // Act

            sut.Delete(newDepartment.DepartmentId);

            //Assert

            department = sut.Retrieve(newDepartment.DepartmentId);
            Assert.IsNull(department);
        }
        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Retrieve_WithAnExistingDepartmentId_ReturnsRecordFromDb()
        {
            //Arrange 
            var newDepartment = sut.Create(department);
            //Act
            var found =  sut.Retrieve(newDepartment.DepartmentId);

            // Assert
            Assert.IsNotNull(found);

            //Clean Up
            sut.Delete(found.DepartmentId);
        }
        [TestMethod]
        public void Update_WithValidData_SavesUpdatesInDb()
        {
            //Arrange
            var newDepartment = sut.Create(department);

            var expectedDepartmentName = "HR Department";

            newDepartment.DepartmentName = expectedDepartmentName;

            //Act
            sut.Update(newDepartment.DepartmentId,department);

            //Assert
            var updatedDepartment = sut.Retrieve(department.DepartmentId);
            Assert.AreEqual(expectedDepartmentName, updatedDepartment.DepartmentName);

            //Cleanup
            sut.Delete(updatedDepartment.DepartmentId);

        }

    }
}
