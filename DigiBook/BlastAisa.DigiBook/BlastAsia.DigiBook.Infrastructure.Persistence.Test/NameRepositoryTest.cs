using BlastAsia.DigiBook.Domain.Models.Names;
using BlastAsia.DigiBook.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Infrastructure.Persistence.Test
{
    [TestClass]
    public class NameRepositoryTest
    {
        private string connectionString;
        private DbContextOptions<DigiBookDbContext> dbOption;
        private DigiBookDbContext dbContext;
        private NameRepository sut;
        private Name name;

        [TestInitialize]
        public void TestInitialize()
        {
            connectionString = @"Data Source=.; Database=DigiBookDb; Integrated Security = true";
            dbOption = new DbContextOptionsBuilder<DigiBookDbContext>()
                .UseSqlServer(connectionString)
                .Options;
            dbContext = new DigiBookDbContext(dbOption);
            sut = new NameRepository(dbContext);
            name = new Name
            {
                NameId = Guid.NewGuid(),
                NameFirst = "Karl",
                NameLast = "Matencio"
            };
        }

        [TestCleanup]
        public void TestCleanup()
        {
            dbContext.Dispose();
            dbContext = null;
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Create_WithValidData_SavesRecordToTheDatabase()
        {
            //Arrange
            
            //Act
            var result = sut.Create(name);
            //Assert
            Assert.IsNotNull(name);
            Assert.IsTrue(name.NameId != Guid.Empty);
            //Cleanup
            sut.Delete(name.NameId);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Retrieve_WithValidData_GetsRecordFromTheDatabase()
        {
            //Arrange
            
            var newName = sut.Create(name);
            //Act
            var found = sut.Retrieve(newName.NameId);
            //Assert
            Assert.IsNotNull(found);
            //Cleanup
            sut.Delete(name.NameId);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Update_WithValidData_UpdatesRecordFromTheDatabase()
        {
            //Arrange
           
            var newName = sut.Create(name);

            var expectedFirstName = "Renz";
            var expectedLastName = "Nebran";

            newName.NameFirst = expectedFirstName;
            newName.NameLast = expectedLastName;
            //Act
            sut.Update(newName.NameId, name);
            //Assert
            var update = sut.Retrieve(newName.NameId);

            Assert.AreEqual(newName.NameFirst, update.NameFirst);
            Assert.AreEqual(newName.NameLast, update.NameLast);
            //Cleanup
            sut.Delete(name.NameId);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Delete_WithValidData_DeletesRecordFromTheDatabase()
        {
            //Arrange
            
            var newName = sut.Create(name);
            //Act
            sut.Delete(newName.NameId);
            //Assert
            name = sut.Retrieve(newName.NameId);
            Assert.IsNull(name);
        }
    }
}
