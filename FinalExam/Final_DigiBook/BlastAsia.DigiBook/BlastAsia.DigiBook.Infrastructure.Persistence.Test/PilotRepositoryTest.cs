using BlastAsia.DigiBook.Domain.Models.Pilots;
using BlastAsia.DigiBook.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BlastAsia.DigiBook.Infrastructure.Persistence.Test
{
    [TestClass]
    public class PilotRepositoryTest
    {
        private string connectionString;
        private DbContextOptions<DigiBookDbContext> dbOptions;
        private DigiBookDbContext dbContext;
        private Pilot pilot;
        private PilotRepository sut;
        private Pilot result;
 

        [TestInitialize()]
        public void Initialize()
        {
            pilot = new Pilot
            {
                FirstName = "Christoper",
                MiddleName = "Magdaleno",
                LastName = "Manuel",
                BirthDate = DateTime.Now.AddYears(-25),
                YearsOfExperience = 10,
                DateActivated = DateTime.Today,
            };

            connectionString =
               @"Data Source=.; Database=DigiBookDb; Integrated Security=true";

            dbOptions = new DbContextOptionsBuilder<DigiBookDbContext>()
                .UseSqlServer(connectionString)
                .Options;
            dbContext = new DigiBookDbContext(dbOptions);
            dbContext.Database.EnsureCreated();

            sut = new PilotRepository(dbContext);
        }

        [TestCleanup()]
        public void CleanUp()
        {

        }
        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Create_WithValidData_SaveRecordInTheDatabase()
        {
            //Arrange

            //Act
            var newPilot = sut.Create(pilot);

            //Assert
            Assert.IsNotNull(pilot);
            Assert.IsTrue(newPilot.PilotId != Guid.Empty);

            //CleanUp

            sut.Delete(newPilot.PilotId);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Delete_WithAnExistingPilot_RemovesRecordFromDatabase()
        {
            var newPilot = sut.Create(pilot);
            //Act
            sut.Delete(newPilot.PilotId);

            result = sut.Retrieve(newPilot.PilotId);
            Assert.IsNull(result);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Retrieve_WithExistingPilotId_ShouldRetrieveInDatabase()
        {
            //Arrange

            //Act 
            result = sut.Create(pilot);
            var retrievedPilot = sut.Retrieve(result.PilotId);

            //Assert
            Assert.IsNotNull(retrievedPilot);

            //Cleanup

        }


        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Update_WithValidData_SaveUpdateInDb()
        {
            //Arrange
            result = sut.Create(pilot);
            var expectedFirstName = "Ryan Karl";
            var expectedMiddleName = "Renosa";
            var expectedLastName = "Oribello";
  

            //Act 
            var newPilot = sut.Retrieve(result.PilotId);
            newPilot.FirstName = expectedFirstName;
            newPilot.MiddleName = expectedMiddleName;
            newPilot.LastName = expectedLastName;
            //Assert
            var updatedPilot = sut.Update(newPilot.PilotId, newPilot);
            Assert.AreEqual(expectedFirstName, updatedPilot.FirstName);
            Assert.AreEqual(expectedMiddleName, updatedPilot.MiddleName);
            Assert.AreEqual(expectedLastName, updatedPilot.LastName);
            //CleanUp

        }


        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Fetch_WithValidPageAndRecord_ShouldRetrieveInDatabase()
        {
            int pageNo = 1;
            int numRec = 10;
            string filterValue = "";
            //Arrange
            sut.Fetch(pageNo, numRec, filterValue);
            //Act 

            //Assert
        }
    }
    
}
