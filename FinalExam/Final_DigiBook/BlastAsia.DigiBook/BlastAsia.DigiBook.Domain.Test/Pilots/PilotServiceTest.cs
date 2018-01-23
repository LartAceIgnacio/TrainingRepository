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
        private Pilot pilot;
        private Mock<IPilotRepository> mockPilotRepository;
        private PilotService sut;
        private Guid existingPilotId = Guid.NewGuid();
        private string longName;
        private string existingPilotCode;

        [TestInitialize]
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

            existingPilotCode = "ChMaManu180123";
            
            mockPilotRepository = new Mock<IPilotRepository>();

            sut = new PilotService(mockPilotRepository.Object);

            mockPilotRepository
                .Setup(pr => pr.Retrieve(existingPilotId))
                .Returns(pilot);

            longName = "Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old. Richard McClintock, a Latin professor at Hampden-Sydney College in Virginia, looked up one of the more obscure Latin words, consectetur, from a Lorem Ipsum passage, and going through the cites of the wo ";

            mockPilotRepository
                .Setup(pr => pr.Retrieve())
                //.Returns();
        }

        [TestMethod]
        public void Save_WithValidData_ShoulPilotRepositoryCreate()
        {
            //Arrange
             
            //Act 
            sut.Save(pilot.PilotId, pilot);

            //Assert
            mockPilotRepository
                .Verify(c => c.Create(pilot), Times.Once());
        }

        [TestMethod]
        public void Save_WithExistingPilot_CallsPilotRepositoryUpdate()
        {
            //Arrange
            pilot.PilotId = existingPilotId;
            //Act 
            sut.Save(pilot.PilotId, pilot);

            //Assert
            mockPilotRepository
                .Verify(c => c.Create(pilot), Times.Never());
            mockPilotRepository
               .Verify(c => c.Update(pilot.PilotId, pilot), Times.Once());
        }


        [TestMethod]
        public void Save_WithEmptyFirstName_ThrowsFirstNameRequiredException()
        {
            //Arrange
            pilot.FirstName = "";
            //Act 

            //Assert
            Assert.ThrowsException<FirstNameRequiredException>(
                ()=> sut.Save(pilot.PilotId, pilot));

            mockPilotRepository
                .Verify(c => c.Create(pilot), Times.Never());

        }


        [TestMethod]
        public void Save_WithFirstNameLengthGreaterThanMaximumLength_ThrowsFirstNameMaximumLenghtException()
        {
            //Arrange
            pilot.FirstName = longName;

            //Assert
            Assert.ThrowsException<FirstNameMaximumLenghtException>(
              () => sut.Save(pilot.PilotId, pilot));

            mockPilotRepository
                .Verify(c => c.Create(pilot), Times.Never());
        }


        [TestMethod]
        public void Save_WithMiddleNameLengthGreaterThanMaximumLength_ThrowsMiddleNameLengthException()
        {
            //Arrange
            pilot.MiddleName = longName;
            //Act 

            //Assert
            Assert.ThrowsException<MiddleNameLengthException>(
              () => sut.Save(pilot.PilotId, pilot));

            mockPilotRepository
                .Verify(c => c.Create(pilot), Times.Never());
        }

        [TestMethod]
        public void Save_WithEmptyLastName_ThrowsLastNameRequiredException()
        {
            //Arrange
            pilot.LastName = "";
            //Act 

            //Assert
            Assert.ThrowsException<LastNameRequiredException>(
                () => sut.Save(pilot.PilotId, pilot));

            mockPilotRepository
                .Verify(c => c.Create(pilot), Times.Never());

        }

        [TestMethod]
        public void Save_WithLastNameLengthGreaterThanMaximumLength_ThrowsLastNameMaximumLenghtException()
        {
            //Arrange
            pilot.LastName = longName;

            //Assert
            Assert.ThrowsException<LastNameMaximumLenghtException>(
              () => sut.Save(pilot.PilotId, pilot));

            mockPilotRepository
                .Verify(c => c.Create(pilot), Times.Never());
        }


        [TestMethod]
        public void Save_WithAgeLessThan21yrsOld_ThrowsAgeRequirementException()
        {
            //Arrange
            pilot.BirthDate = DateTime.Today;

            //Assert
            Assert.ThrowsException<MinimumAgeRequirement>(
              () => sut.Save(pilot.PilotId, pilot));

            mockPilotRepository
                .Verify(c => c.Create(pilot), Times.Never());
        }


        [TestMethod]
        public void Save_WithYearsOfExperienceLessThan10YearsOfExperience_ThrowsYearsOfExperienceMinimumRequiredException()
        {
            //Arrange
            pilot.YearsOfExperience = 5;
            //Act 

            //Assert
            Assert.ThrowsException<YearsOfExperienceMinimumRequiredException>(
                () => sut.Save(pilot.PilotId, pilot));

            mockPilotRepository
              .Verify(c => c.Create(pilot), Times.Never());
        }


        [TestMethod]
        public void Save_WithInvalidPilotCode_ThrowsInvalidPilotCodeException()
        {
            //Arrange
            pilot.FirstName = "!R5";
            //Act 

            //Assert
            Assert.ThrowsException<InvalidPilotCodeException>(
                () => sut.Save(pilot.PilotId, pilot));

            mockPilotRepository
              .Verify(c => c.Create(pilot), Times.Never());
        }

        [TestMethod]
        public void Save_WithExistingPilotCode_ThrowsNonUniquePilotCodeException()
        {
            //Arrange

            mockPilotRepository
                .Setup(pr => pr.RetrievePilotCode(existingPilotCode))
                .Returns(pilot);
            //Act 

            //Assert
            Assert.ThrowsException<NonUniquePilotCodeException>(
                () => sut.Save(pilot.PilotId, pilot));

            mockPilotRepository
              .Verify(c => c.Create(pilot), Times.Never());
        }
    }
}
