using BlastAsia.DigiBook.Domain.Models.Pilots;
using BlastAsia.DigiBook.Infrastracture.Persistence.Repositories;
using BlastAsia.DigiBook.Insfrastracture.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Infrastracture.Persistence.Test
{

    [TestClass]
    public class PilotRepositoryTest
    {
        private Pilot pilot;
        private DbContextOptions<DigiBookDbContext> dbOptions;
        private DigiBookDbContext dbContext;
        private readonly string connectionString = @"Data Source=.; Database=DigiBookDb; Integrated Security=true;";
        private PilotRepository sut;

        [TestInitialize]
        public void Initialize()
        {
            pilot = new Pilot
            {
                PilotId = Guid.NewGuid(),
                FirstName = "Emmanuel",
                MiddleName = "Pararuan",
                LastName = "Magadia",
                DateOfBirth = DateTime.Now,
                YearsOfExperience = 12,
                DateActivated = DateTime.Now,
                PilotCode = "",
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now
            };

            dbOptions = new DbContextOptionsBuilder<DigiBookDbContext>()
                                   .UseSqlServer(connectionString)
                                   .Options;

            dbContext = new DigiBookDbContext(dbOptions); // ORM
            dbContext.Database.EnsureCreated();
            sut = new PilotRepository(dbContext); // System under test
        }

        [TestCleanup]
        public void Cleanup()
        {
            dbContext.Dispose();
            dbContext = null;
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Create_WithValidData_SavesRecordToDatabase()
        {
            // act
            var result = sut.Create(pilot);

            // assert 
            Assert.IsNotNull(result);
            Assert.IsTrue(result.PilotId != Guid.Empty);

            // Cleanup
            sut.Delete(result.PilotId);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Delete_WithAnExistingPilot_RemovesRecordFromDatabase()
        {
            // arrange 
            var newPilot = sut.Create(pilot);
            // act 
            sut.Delete(newPilot.PilotId);
            // assert
            pilot = sut.Retrieve(newPilot.PilotId);
            Assert.IsNull(pilot);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Retrieve_WithExistingPilotId_ReturnsRecordFromDatabase()
        {
            // arrange
            var newPilot = sut.Create(pilot);
            //act
            var found = sut.Retrieve(newPilot.PilotId);
            // assert 
            Assert.IsNotNull(found);

            sut.Delete(newPilot.PilotId);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Update_WithExistingPilotId_SaveAndUpdateInDatabase()
        {
            //arrange
            var newPilot = sut.Create(pilot);

            var expectedFirstName = "Kyrie"; 
            var expectedLastName = "Irving";

            newPilot.FirstName = expectedFirstName;
            newPilot.LastName = expectedLastName;

            // act
            sut.Update(newPilot.PilotId, newPilot);

            var Updatedpilot = sut.Retrieve(newPilot.PilotId);
            // assert 
            Assert.AreEqual(Updatedpilot.FirstName, expectedFirstName);
            Assert.AreEqual(Updatedpilot.LastName, expectedLastName);

            // cleanup
            sut.Delete(Updatedpilot.PilotId);
        }
    }
}
