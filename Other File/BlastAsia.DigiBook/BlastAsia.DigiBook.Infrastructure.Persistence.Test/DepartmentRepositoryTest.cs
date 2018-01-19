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
        private string connectionString;
        private DbContextOptions<DigiBookDbContext> dbOptions;
        private DigiBookDbContext dbContext;
        private Department department;
        private DepartmentRepository sut;
        private Department result;
        private Guid existingDepartmentHeadId = Guid.NewGuid();

        [TestInitialize()]
        public void TestInit()
        {
            department = new Department
            {
                DepartmentName = "Human Resource",
                DepartmentHeadId = existingDepartmentHeadId
            };

             connectionString =
             @"Data Source=.; Database=DigiBookDb; Integrated Security=true";

            dbOptions = new DbContextOptionsBuilder<DigiBookDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            dbContext = new DigiBookDbContext(dbOptions);

            dbContext.Database.EnsureCreated();

            sut = new DepartmentRepository(dbContext);
        }
        [TestCleanup]
        public void TestCleanUp()
        {   
            if(result != null)
            {
                if (!(Guid.Empty.Equals(result.DepartmentId)))
                {
                    sut.Delete(result.DepartmentId);
                }
            }
           
            
        }

        [TestMethod]
        public void Create_WithValidData_ShouldSaveToDatabase()
        {
           //Arrange

           //Act 
            result = sut.Create(department);
            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.DepartmentId != Guid.Empty);

            //CleanUp
    
        }


        [TestMethod]
        public void Delete_WithExisitingDepartment_ShouldDeleteInDatabase()
        {
            //Arrange

            //Act 
            var shouldBeDeleted = sut.Create(department);
            sut.Delete(shouldBeDeleted.DepartmentId);
            result = sut.Retrieve(shouldBeDeleted.DepartmentId);
            //Assert
            Assert.IsNull(result);
            
        }


        [TestMethod]
        public void Retrieve_WithExistingDepartmentId_ShouldRetrieveInDatabase()
        {
            //Arrange

            //Act 
            result = sut.Create(department);
            var retrievedDepartment = sut.Retrieve(result.DepartmentId);

            //Assert
            Assert.IsNotNull(retrievedDepartment);

            //Cleanup
           
        }


        [TestMethod]
        public void Update_WithValidData_SaveUpdatesInDb()
        {
            //Arrange
            result = sut.Create(department);
            var expectedDepartmentName = "Accounting";
            var expectedDepartmentHeadId = Guid.NewGuid();

            //Act 
            var newDepartment = sut.Retrieve(result.DepartmentId);
            newDepartment.DepartmentName = expectedDepartmentName;
            newDepartment.DepartmentHeadId = expectedDepartmentHeadId;

            //Assert
            var updatedDepartment = sut.Update(newDepartment.DepartmentHeadId, newDepartment);
            Assert.AreEqual(expectedDepartmentName, updatedDepartment.DepartmentName);
            Assert.AreEqual(expectedDepartmentHeadId, updatedDepartment.DepartmentHeadId);

            //CleanUp
            
        }
     }
}
