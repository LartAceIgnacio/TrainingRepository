using BlastAsia.DigiBook.Domain.Models.Pilots;
using BlastAsia.DigiBook.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Infrastructure.Persistence.Test
{
    [TestClass]
    public class PilotRepositoryTest
    {

        private Pilot pilot = null;  
        private DbContextOptions<DigiBookDbContext> dbOptions = null;
        private DigiBookDbContext dbContext = null;
        private String connectionString = null;
        private PilotRepository sut;

        private Guid existingPilotId = Guid.NewGuid();

        [TestInitialize]
        public void Initialize()
        {
            pilot = new Pilot
            {
                FirstName = "Eugene",
                MiddleName = "Mauro",
                LastName = "Ravina",
                DateOfBirth = DateTime.Now.AddYears(-25),
                YearsOfExperience = 15,
                DateActivated = DateTime.Now,
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now

            };

            connectionString =
               @"Data Source=.;Database=DigiBookDb;Integrated Security=true;";
            dbOptions = new DbContextOptionsBuilder<DigiBookDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            dbContext = new DigiBookDbContext(dbOptions);
            dbContext.Database.EnsureCreated();

            sut = new PilotRepository(dbContext);
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
            var newPilot = sut.Create(pilot);

            //Assert
            Assert.IsNotNull(newPilot);
            Assert.IsTrue(newPilot.PilotId != Guid.Empty);

            //Cleanup
            sut.Delete(newPilot.PilotId);
        }
        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Delete__WithAnExistingPilot_RemovesRecordFromDatabase()
        {


            sut = new PilotRepository(dbContext);
            var newPilot = sut.Create(pilot);

            // Act

            sut.Delete(newPilot.PilotId);

            // Assert
            pilot = sut.Retrieve(newPilot.PilotId);
            Assert.IsNull(pilot);
        }
        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Retrieve_WithExistingPilotId_ReturnsRecordFromDb()
        {
            //Arrange
            var newPilot = sut.Create(pilot);


            //Act
            var found = sut.Retrieve(newPilot.PilotId);

            //Assert
            Assert.IsNotNull(found);

            //Cleanup
            sut.Delete(found.PilotId);
        }
        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Update_WithValidData_SavesUpdatesInDb()
        {
            //Arrange
            var newPilot = sut.Create(pilot);
            var expectedFirstName = "Vincent"; //from Eugene
            var expectedMiddleName = "Jose"; //from Mauro
            var expectedLastName = "Taguro"; //from Ravina

           
            newPilot.FirstName = expectedFirstName;
            newPilot.MiddleName = expectedMiddleName;
            newPilot.LastName = expectedLastName;
            

            //Act
            sut.Update(newPilot.PilotId, newPilot);


            //Assert
            var updatedPilot = sut.Retrieve(newPilot.PilotId);
            Assert.AreEqual(expectedFirstName, updatedPilot.FirstName);
            Assert.AreEqual(expectedMiddleName, updatedPilot.MiddleName);
            Assert.AreEqual(expectedLastName, updatedPilot.LastName);
            

            //Cleanup
            sut.Delete(updatedPilot.PilotId);
        }
    }
}
