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
        private Guid nonExistingPilotId = Guid.Empty;

        private string existingPilotCode = "EuMaRavi180125"; 

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
            mockPilotRepository = new Mock<IPilotRepository>();
            sut = new PilotService(mockPilotRepository.Object);

            mockPilotRepository
                .Setup(p => p.Create(pilot))
                .Callback(() => pilot.PilotId = Guid.NewGuid())
                .Returns(pilot);

            mockPilotRepository
                .Setup(p => p.Retrieve(existingPilotId))
                .Returns(pilot);
        }
        [TestMethod]
        public void Save_NewPilotWithValidData_ShouldCallRepositoryCreate()
        {
            //Arrange
            
            //Act
            var result = sut.Save(pilot.PilotId,pilot);

            //Assert
            mockPilotRepository
                .Verify(p => p.Create(pilot), Times.Once());

            mockPilotRepository
                .Verify(p => p.Retrieve(pilot.PilotId), Times.Never());
        }
        [TestMethod]
        public void Save_WithExistingPilot_ShoudCallRepositoryUpdate()
        {
            //Arrange 
            pilot.PilotId = existingPilotId;

            //Act
            sut.Save(pilot.PilotId, pilot);

            //Assert
            mockPilotRepository
                .Verify(p => p.Retrieve(pilot.PilotId), Times.Once());

            mockPilotRepository
                .Verify(p => p.Update(existingPilotId, pilot), Times.Once);
        }
        [TestMethod]
        public void Save_WithBlankFirstName_ThrowsPilotNameRequiredException()
        {
            //Arrange
            pilot.FirstName = "";

            //Assert
            Assert.ThrowsException<PilotNameRequiredException>(
                () => sut.Save(pilot.PilotId, pilot));
        }
        [TestMethod]
        public void Save_WithFirstNameMaximumLength_ThrowsLessThanMaximumLengthRequiredException()
        {
            // Arrange
            pilot.FirstName = "asdfghjklzasdfghjklzasdfghjklzasdfghjklzasdfghjklzasdfghjklzaa";

            // Assert
            Assert.ThrowsException<MaximumLengthRequiredException>(
                () => sut.Save(pilot.PilotId, pilot));

        }
        [TestMethod]
        public void Save_WithMiddleNameMaximumLength_ThrowsLessThanMaximumLengthRequiredException()
        {
            // Arrange
            pilot.MiddleName = "asdfghjklzasdfghjklzasdfghjklzasdfghjklzasdfghjklzasdfghjklzaa";

            // Assert
            Assert.ThrowsException<MaximumLengthRequiredException>(
                () => sut.Save(pilot.PilotId, pilot));
        }
        [TestMethod]
        public void Save_WithLastNameMaximumLength_ThrowsLessThanMaximumLengthRequiredException()
        {
            // Arrange
            pilot.LastName = "asdfghjklzasdfghjklzasdfghjklzasdfghjklzasdfghjklzasdfghjklzaa";

            // Assert
            Assert.ThrowsException<MaximumLengthRequiredException>(
                () => sut.Save(pilot.PilotId, pilot));
        }

        [TestMethod]
        public void Save_BlankDateOfBirth_ThrowsDateOfBirthRequiredException()
        {
            //Arrange
            pilot.DateOfBirth = null;

            //Assert
            Assert.ThrowsException<DateOfBirthRequiredException>(
               () => sut.Save(pilot.PilotId, pilot));
        }

        [TestMethod]
        public void Save_WithDateOfBirth_ThrowsGreatherThanTwentyOneYearsRequiredException()
        {
            //Arrange
            pilot.DateOfBirth = DateTime.Now;

            // Assert
            Assert.ThrowsException<GreatherThanTwentyOneYearsRequiredException>(
               () => sut.Save(pilot.PilotId, pilot));
        }

        [TestMethod]
        public void Save_WithBlankYearsOfExperience_ThrowsYearsOfExperienceRequiredException()
        {
            //Arrange
            pilot.YearsOfExperience = null ;

            //Assert
            Assert.ThrowsException<YearsOfExperienceRequiredException>(
               () => sut.Save(pilot.PilotId, pilot));
        }

        [TestMethod]
        public void Save_WithYearsOfExperience_ThrowsYearOfExperienceRequiredException()
        {
            //Arrange
            pilot.YearsOfExperience = 5;

            //Assert
            Assert.ThrowsException<YearsOfExperienceRequiredException>(
               () => sut.Save(pilot.PilotId, pilot));
        }
        [TestMethod]
        public void Save_WithBlankDateActivated_ThrowsDateRequiredException()
        {
            // Arrange
            pilot.DateActivated = null;

            // Assert
            Assert.ThrowsException<DateRequiredException>(
               () => sut.Save(pilot.PilotId, pilot));
        }
        //[TestMethod]
        //public void Save_WithBlankPilotCode_ThrowsPilotCodeRequiredException()
        //{
        //    // Arrange
        //    pilot.PilotCode = "";

        //    // Assert
        //    Assert.ThrowsException<PilotCodeRequiredException>(
        //       () => sut.Save(pilot.PilotId, pilot));
        //}
        [TestMethod]
        public void Save_WithExistingPilotCode_ThrowsPilotCodeUniqueRequiredException()
        {

            // Assert

            mockPilotRepository
                .Setup(p => p.Retrieve(existingPilotCode))
                .Returns(pilot);

            Assert.ThrowsException<PilotCodeRequiredException>(
                () => sut.Save(pilot.PilotId, pilot));

            mockPilotRepository
                .Verify(p => p.Retrieve(existingPilotCode), Times.Once());

            mockPilotRepository
                .Verify(p => p.Create(pilot), Times.Never);


        }
        [TestMethod]
        public void Save_WithInvalidFormat_ThrowsInvalidFormatException()
        {
            //Arrange
            pilot.FirstName = "1T";
            pilot.MiddleName = "2A";
            pilot.LastName = "3R34";

            // Assert

            Assert.ThrowsException<InvalidFormatException>(
                () => sut.Save(pilot.PilotId, pilot));
        }
        




    }
}
