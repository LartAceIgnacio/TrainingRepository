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
        private Guid existingId;
        private Guid nonExistingId;


        [TestInitialize]
        public void Initialize()
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

            mockPilotRepository = new Mock<IPilotRepository>();
            sut = new PilotService(mockPilotRepository.Object);
            existingId = Guid.NewGuid();
            nonExistingId = Guid.Empty;

            
        }

        [TestMethod]
        public void Save_WithBlankFirstName_ThrowsFirstNameRequiredException()
        {
            //Arrange
            pilot.FirstName = "";

            //Assert

            Assert.ThrowsException<FirstNameRequiredException>(
                () => sut.Save(pilot.PilotId, pilot)
                );
        }

        [TestMethod]
        public void Save_WithFirstNameIsGreaterThanSixty_ThrowsFirstNameRequiredException()
        {
            //Arrange
            pilot.FirstName = "1234567890123456789012345678901234567890123456789012345678901234567890";

            //Assert

            Assert.ThrowsException<FirstNameRequiredException>(
                () => sut.Save(pilot.PilotId, pilot)
                );
        }

        [TestMethod]
        public void Save_WithBlankLastName_ThrowsLastNameRequiredException()
        {
            //Arrange
            pilot.LastName = "";

            //Assert

            Assert.ThrowsException<LastNameRequiredException>(
                () => sut.Save(pilot.PilotId, pilot)
                );
        }

        [TestMethod]
        public void Save_WithLastNameIsGreaterThanSixty_ThrowsLastNameRequiredException()
        {
            //Arrange
            pilot.LastName = "1234567890123456789012345678901234567890123456789012345678901234567890";

            //Assert

            Assert.ThrowsException<LastNameRequiredException>(
                () => sut.Save(pilot.PilotId, pilot)
                );
        }

        [TestMethod]
        public void Save_WithBlankDateOfBirth_ThrowsDateOfBirthRequiredException()
        {
            //Arrange
            pilot.DateOfBirth = null;

            //Act

            //Assert
            Assert.ThrowsException<DateOfBirthRequiredException>(
                () => sut.Save(pilot.PilotId, pilot)
                );
        }

        [TestMethod]
        public void Save_WithDateOfBirthAgeLessThan21_ThrowsDateOfBirthRequiredException()
        {
            //Arrange
            pilot.DateOfBirth = new DateTime(2000, 10, 24);

            //Act

            //Assert
            Assert.ThrowsException<DateOfBirthRequiredException>(
                () => sut.Save(pilot.PilotId, pilot)
                );
        }
        [TestMethod]
        public void Save_WithBlankYearsOfExperience_ThrowsYearsOfExperienceRequiredException()
        {
            //Arrange
            pilot.YearsOfExperience = null;

            //Act

            //Assert
            Assert.ThrowsException<YearsOfExperienceRequiredException>(
                () => sut.Save(pilot.PilotId, pilot)
                );
        }
        [TestMethod]
        public void Save_WithYearsOfExperienceLessThan10_ThrowsYearsOfExperienceRequiredException()
        {
            //Arrange
            pilot.YearsOfExperience = 9;

            //Act

            //Assert
            Assert.ThrowsException<YearsOfExperienceRequiredException>(
                () => sut.Save(pilot.PilotId, pilot)
                );
        }

        [TestMethod]
        public void Save_WithBlankDateActivated_ThrowsDateActivatedRequiredException()
        {
            //Arrange
            pilot.DateActivated = null;

            //Act

            //Assert
            Assert.ThrowsException<DateActivatedRequiredException>(
                () => sut.Save(pilot.PilotId, pilot)
                );
        }

        [TestMethod]
        public void Save_WithPilotCodeNotEqualTo12_ThrowsPilotCodeMustBe12CharactersRequiredException()
        {
            //Arrange
            pilot.PilotCode = "1123";

            //Act

            //Assert
            Assert.ThrowsException<PilotCodeMustBe12CharactersRequiredException>(
                () => sut.Save(pilot.PilotId, pilot)
                );
        }
        [TestMethod]
        public void Save_NewPilotWithValidDataAndNoExistingId_ShouldCallRepositoryCreateAndGeneratePilotId()
        {

            //Arrange
            pilot.PilotId = nonExistingId;

            mockPilotRepository
                .Setup(c => c.Retrieve(nonExistingId))
                .Returns<Pilot>(null);

            mockPilotRepository
                .Setup(c => c.Create(pilot))
                .Callback(() => pilot.PilotId = existingId)
                .Returns(pilot);

            //Act

            var result = sut.Save(nonExistingId, pilot);

            //Assert
            mockPilotRepository
                .Verify(c => c.Retrieve(nonExistingId), Times.Once());


            mockPilotRepository
                .Verify(c => c.Create(pilot), Times.Once());

            Assert.IsTrue(result.PilotId != Guid.Empty);

        }
        [TestMethod]
        public void Save_NewPilotWithExistingIdAndValidData_ShouldCallRepositoryUpdate()
        {

            //Arrange
            pilot.PilotId = existingId;

            mockPilotRepository
                .Setup(c => c.Retrieve(existingId))
                .Returns(pilot);

            //Act

            var result = sut.Save(existingId, pilot);

            //Assert
            mockPilotRepository
                .Verify(c => c.Retrieve(existingId), Times.Once());


            mockPilotRepository
                .Verify(c => c.Update(pilot.PilotId, pilot), Times.Once());


        }

    }
}
