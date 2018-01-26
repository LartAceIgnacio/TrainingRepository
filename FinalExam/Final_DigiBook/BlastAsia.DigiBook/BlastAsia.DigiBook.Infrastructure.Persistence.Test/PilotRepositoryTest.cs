using BlastAsia.DigiBook.Domain.Models.Pilots;
using BlastAsia.DigiBook.Domain.Pilots;
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
        private Pilot pilot, createdPilot;
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
            if(result != null)
            {
              sut.Delete(result.PilotId);
            }
           
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Create_WithValidData_SaveRecordInTheDatabase()
        {
            //Arrange

            //Act
            result = sut.Create(pilot);

            //Assert
            Assert.IsNotNull(pilot);
            Assert.IsTrue(result.PilotId != Guid.Empty);

            //CleanUp

            
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Delete_WithAnExistingPilot_RemovesRecordFromDatabase()
        {
            createdPilot = sut.Create(pilot);
            //Act
            sut.Delete(createdPilot.PilotId);

            result = sut.Retrieve(createdPilot.PilotId);
            Assert.IsNull(result);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Retrieve_WithExistingPilotId_ShouldRetrieveInDatabase()
        {
            //Arrange

            //Act 
            createdPilot = sut.Create(pilot);
            result = sut.Retrieve(createdPilot.PilotId);

            //Assert
            Assert.IsNotNull(result);

            //Cleanup

        }


        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Update_WithValidData_SaveUpdateInDb()
        {
            //Arrange
            var createdPilot = sut.Create(pilot);
            var expectedFirstName = "Ryan Karl";
            var expectedMiddleName = "Renosa";
            var expectedLastName = "Oribello";
  

            //Act 
            var newPilot = sut.Retrieve(createdPilot.PilotId);
            newPilot.FirstName = expectedFirstName;
            newPilot.MiddleName = expectedMiddleName;
            newPilot.LastName = expectedLastName;
            //Assert
            result = sut.Update(newPilot.PilotId, newPilot);
            Assert.AreEqual(expectedFirstName, result.FirstName);
            Assert.AreEqual(expectedMiddleName, result.MiddleName);
            Assert.AreEqual(expectedLastName, result.LastName);
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

            //Act 
            var record = sut.Fetch(pageNo, numRec, filterValue);
            //Assert

            Assert.IsNotNull(record);
        }


        [TestMethod]
        public void Retrive_WithValidPilotCode_ShouldRetrivePilotIntheDatabase()
        {
            //Arrange  
            var generator = new PilotCodeGenerator();
            pilot.PilotCode = generator.PilotCodeGenerate(pilot);
            var createdPilot = sut.Create(pilot);
            //Act 
            result = sut.Retrieve(createdPilot.PilotCode);
            //Assert
            Assert.IsNotNull(result);
        }
    }
    
}
