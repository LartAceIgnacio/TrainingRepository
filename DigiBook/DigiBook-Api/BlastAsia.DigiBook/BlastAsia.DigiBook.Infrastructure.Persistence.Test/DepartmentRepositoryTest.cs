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

        private Guid existingDepartmentId = Guid.NewGuid();

        [TestInitialize]
        public void Initialize()
        {
            department = new Department
            {
                DepartmentName = "",

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
        [TestProperty("TestType", "Integration")]
        public void Create_WithValidData_SavesRecordInTheDatabase()
        {

            //Act
            var newDepartment = sut.Create(department);

            //Assert
            Assert.IsNotNull(newDepartment);
            Assert.IsTrue(newDepartment.DepartmentId != Guid.Empty);

            //Cleanup
            sut.Delete(newDepartment.DepartmentId);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Delete__WithAnExistingDepartment_RemovesRecordFromDatabase()
        {

            sut = new DepartmentRepository(dbContext);
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
            //Arrange
            var newDepartment = sut.Create(department);


            //Act
            var found = sut.Retrieve(newDepartment.DepartmentId);

            //Assert
            Assert.IsNotNull(found);

            //Cleanup
            sut.Delete(found.DepartmentId);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Update_WithValidData_SavesUpdatesInDb()
        {
            //Arrange
            var newDepartment = sut.Create(department);
            var expectedDepartmentName= "IT Department"; //from HR Department
          
            newDepartment.DepartmentName = expectedDepartmentName;
           

            //Act
            sut.Update(newDepartment.DepartmentId, newDepartment);


            //Assert
            var updatedDepartment = sut.Retrieve(newDepartment.DepartmentId);
            Assert.AreEqual(expectedDepartmentName, updatedDepartment.DepartmentName);
          

            //Cleanup
            sut.Delete(updatedDepartment.DepartmentId);
        }

    }
}
