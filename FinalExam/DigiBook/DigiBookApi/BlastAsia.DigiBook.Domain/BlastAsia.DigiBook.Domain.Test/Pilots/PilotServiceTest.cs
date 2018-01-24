using BlastAsia.DigiBook.Domain.Models.Pilots;
using BlastAsia.DigiBook.Domain.Pilots;
using BlastAsia.DigiBook.Domain.Pilots.Exceptions;
using BlastAsia.DigiBook.Domain.Pilots.Pilots.Exceptions;
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
        private Mock<IPilotRepository> mockRepo;
        private PilotService sut;
        private Pilot pilot;

        [TestInitialize]
        public void Initialize()
        {
            mockRepo = new Mock<IPilotRepository>();
            sut = new PilotService(mockRepo.Object);
            pilot = new Pilot
            {
                PilotId = Guid.NewGuid(),
                FirstName = "Renz",
                MiddleName = "Tulab",
                LastName = "Nebran",
                DateOfBirth = DateTime.Parse("07/20/1997"),
                YearsOfExperience = 12,
                DateActivated = DateTime.Now,
                PilotCode = "RETUNEBR970720",
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now
            };

        }
        [TestMethod]
        public void Save_WithValidData_ShouldCallRepositoryCreate()
        {
            //act
            sut.Save(pilot.PilotId, pilot);

            //assert
            mockRepo
                .Verify(r => r.Create(pilot), Times.Once);
        }

        [TestMethod]
        public void Save_WithBlankFirstName_ShouldThrowRequiredException()
        {
            //arrange
            pilot.FirstName = "";

            //assert
            Assert.ThrowsException<RequiredException>(
                () => sut.Save(pilot.PilotId, pilot));

            mockRepo
                .Verify(r => r.Create(pilot), Times.Never);
        }
        [TestMethod]
        public void Save_WithInvalidPilotCode_ShouldThrowInvalidFormatException()
        {
            //arrange
            pilot.PilotCode = "RETUNEBR9707201";
            var x = pilot.DateOfBirth.ToString("yyMMdd");

            //assert
            Assert.ThrowsException<InvalidFormatException>(
                () => sut.Save(pilot.PilotId, pilot));

            mockRepo
                .Verify(r => r.Create(pilot), Times.Never);
        }
    }
}
