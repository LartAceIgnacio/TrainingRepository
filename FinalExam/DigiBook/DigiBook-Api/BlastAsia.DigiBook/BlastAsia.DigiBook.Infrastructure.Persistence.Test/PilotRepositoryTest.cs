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
        private PilotRepository sut = null;

        [TestInitialize]
        public void TestInitialize()
        {
            pilot = new Pilot
            {
                PilotId = Guid.NewGuid(),
                FirstName = "Angelou",
                MiddleName = "Acosta",
                LastName = "Celis",
                DateOfBirth = new DateTime(1994, 10, 24),
                YearsOfExperience = 10,
                DateActivated = DateTime.Today,
                PilotCode = "FFMMLLLLYYmmdd",
                DateCreated = DateTime.Today,
                DateModified = new DateTime()
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
        public void TestCleanUp()
        {
            dbContext.Dispose();
            dbContext = null;
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Create_WithValidData_SavesRecordsInTheDatabase()
        {

            //Act
            var newPilot = sut.Create(pilot);

            //Assert
            Assert.IsNotNull(newPilot);
            Assert.IsTrue(newPilot.PilotId != Guid.Empty);

            //CleanUp

            sut.Delete(newPilot.PilotId);

        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Delete_WithAnExistingContact_RemovesRecordFromData()
        {
            //Arrange

            var newPilot = sut.Create(pilot);

            //Act
            sut.Delete(newPilot.PilotId);

            //Assert
            pilot = sut.Retrieve(newPilot.PilotId);
            Assert.IsNull(pilot);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Retrieve_WithExistingContactId_ReturnsRecordFromDb()
        {
            //Arrange
            var newPilot = sut.Create(pilot);


            //Act
            var found = sut.Retrieve(newPilot.PilotId);

            //Assert
            Assert.IsNotNull(found);

            //CleanUp
            sut.Delete(found.PilotId);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Update_WithValidData_SavesUpdatesInDb()
        {
            //Arrange
            var newPilot = sut.Create(pilot);
            var expectedFirstName = "Angelou";
            var expectedMiddleName = "Acosta";
            var expectedLastName = "Celis";
            var expectedDateOfBirth = new DateTime(1990, 10, 24);
            var expectedYearsOfExperience = 11;
            var expectedDateActivated = DateTime.Today;
            var expectedPilotCode = "FFMMLLLLYYmmyy";


            newPilot.FirstName = expectedFirstName;
            newPilot.MiddleName = expectedMiddleName;
            newPilot.LastName = expectedLastName;
            newPilot.DateOfBirth = expectedDateOfBirth;
            newPilot.YearsOfExperience = expectedYearsOfExperience;
            newPilot.DateActivated = expectedDateActivated;
            newPilot.PilotCode = expectedPilotCode;

            //Act
            sut.Update(newPilot.PilotId, newPilot);

            //Assert
            var updatedPilot = sut.Retrieve(newPilot.PilotId);
            Assert.AreEqual(expectedFirstName, updatedPilot.FirstName);
            Assert.AreEqual(expectedMiddleName, updatedPilot.MiddleName);
            Assert.AreEqual(expectedLastName, updatedPilot.LastName);
            Assert.AreEqual(expectedDateOfBirth, updatedPilot.DateOfBirth);
            Assert.AreEqual(expectedYearsOfExperience, updatedPilot.YearsOfExperience);
            Assert.AreEqual(expectedDateActivated, updatedPilot.DateActivated);
            Assert.AreEqual(expectedPilotCode, updatedPilot.PilotCode);

            //CleanUp
            sut.Delete(updatedPilot.PilotId);
        }

    }
}
