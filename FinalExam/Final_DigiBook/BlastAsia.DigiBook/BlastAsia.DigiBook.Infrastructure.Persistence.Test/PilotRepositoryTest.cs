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

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Create_WithValidData_SaveRecordInTheDatabase()
        {
            //Arrange
           var connectionString =  @"Data Source=.; Database=DigiBookDb; Integrated Security=true";

           var dbOptions = new DbContextOptionsBuilder<DigiBookDbContext>()
                .UseSqlServer(connectionString)
                .Options;
            var dbContext = new DigiBookDbContext(dbOptions);

            var  sut = new PilotRepository(dbContext);
            dbContext.Database.EnsureCreated();


           var  pilot = new Pilot
            {
                FirstName = "Christoper",
                MiddleName = "Magdaleno",
                LastName = "Manuel",
                BirthDate = DateTime.Now.AddYears(-25),
                YearsOfExperience = 10,
                DateActivated = DateTime.Today,
            };
            //Act
            var newPilot = sut.Create(pilot);

            //Assert
            Assert.IsNotNull(pilot);
            Assert.IsTrue(newPilot.PilotId != Guid.Empty);

            //CleanUp
   

        }
    }
    
}
