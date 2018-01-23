using BlastAsia.DigiBook.Domain.Models.Pilots;
using BlastAsia.DigiBook.Domain.Pilots;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Test.Pilots
{

    [TestClass]
    public class PilotServiceTest
    {

        [TestInitialize]
        public void Initialize()
        {

        }

        [TestCleanup]
        public void Cleanup()
        {

        }

        [TestMethod]
        public void Save_WithValidData_ShouldCallRepositorySave()
        {
            // arrange
            var pilot = new Pilot()
            {
                PilotId = Guid.NewGuid(),
                FirstName = "Emmanuel",
                MiddleName = "Pararuan",
                LastName = "Magadia",
                DateOfBirth = DateTime.Now,
                YearsOfExperience = DateTime.Now,
                DateActivated = DateTime.Now,
                PilotCode = "",
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now
            };

            var mockRepo = new Mock<IPilotRepository>();

            var sut = new PilotService(mockRepo.Object);

            // act 
            sut.Save(pilot.PilotId, pilot);


            // assert
            mockRepo
                .Verify(
                    r => r.Create(pilot), Times.Once    
                );

        }

    }
}
