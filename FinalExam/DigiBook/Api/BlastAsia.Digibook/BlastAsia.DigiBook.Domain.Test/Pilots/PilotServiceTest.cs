using BlastAsia.DigiBook.Domain.Models.Pilots;
using BlastAsia.DigiBook.Domain.Pilots;
using BlastAsia.DigiBook.Domain.Pilots.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace BlastAsia.DigiBook.Domain.Test.Pilots
{

    [TestClass]
    public class PilotServiceTest
    {
        private Pilot pilot;
        private Mock<IPilotRepository> mockRepo;
        private PilotService sut;

        private Guid existingPilotId = Guid.NewGuid();
        private Guid noneExistingPilotId = Guid.Empty;

        private readonly string existingCode = "TRTRMAGA180123";
        private readonly string nonExistingCode = "EMPAMAGA180123";

        [TestInitialize]
        public void Initialize()
        {
            pilot = new Pilot()
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

            mockRepo = new Mock<IPilotRepository>();
            sut = new PilotService(mockRepo.Object);

            pilot.DateOfBirth = DateTime.Now.AddYears(-22);


            mockRepo
                .Setup(
                    r => r.Retrieve(existingCode)
                )
                .Returns(new Pilot());


            mockRepo
                .Setup(
                    r => r.Retrieve(nonExistingCode)
                )
                .Returns<Pilot>(null);

            mockRepo.Setup(
                    r => r.Retrieve(existingPilotId)
                )
                .Returns(new Pilot());

        }

        [TestCleanup]
        public void Cleanup()
        {

        }

        [TestMethod]
        public void Save_WithValidData_ShouldCallRepositorySave()
        {
            // arrange

            // act 
            sut.Save(pilot.PilotId, pilot);


            // assert
            mockRepo
                .Verify(
                    r => r.Create(pilot), Times.Once
                );

        }


        [TestMethod]
        public void Save_withNullFirstName_ShouldThrowInvalidNameException()
        {
            // arrange
           
            pilot.FirstName = "";
            // act 
            // assert

            Assert.ThrowsException<InvalidNameException>(
                    () => sut.Save(pilot.PilotId, pilot)
                );
        }

        [TestMethod]
        public void Save_WithMoreThan60CharacterFirstName_ShouldThrowInvalidNameException()
        {
            // arrange
            pilot.FirstName = "EmmanuelllEmmanuelllEmmanuelllEmmanuelllEmmanuelllEmmanuelllEmmanuelll";
            // act 
            // assert
            Assert.ThrowsException<InvalidNameException>(
                    () => sut.Save(pilot.PilotId, pilot)
                );
        }


        [TestMethod]
        public void Save_WithMoreThan60CharactersMiddleName_ShouldThrowInvalidNameException()
        {
            // arrange
            
            pilot.MiddleName = "EmmanuelllEmmanuelllEmmanuelllEmmanuelllEmmanuelllEmmanuelllEmmanuelll";
            // act 
            // assert
            Assert.ThrowsException<InvalidNameException>(
                    () => sut.Save(pilot.PilotId, pilot)
                );
        }


        [TestMethod]
        public void Save_WithNullLastName_ShouldThrowInvalidNameException()
        {
            // arrange
           
            pilot.LastName = null;
            // act 
            // assert
            Assert.ThrowsException<InvalidNameException>(
                    () => sut.Save(pilot.PilotId, pilot)
                );
        }


        [TestMethod]
        public void Save_WithLastNameMoreThan60Characters_ShouldThrowInvalidNameException()
        {
            // arrange
           
            pilot.LastName = "EmmanuelllEmmanuelllEmmanuelllEmmanuelllEmmanuelllEmmanuelllEmmanuelll";
            // act 
            // assert
            Assert.ThrowsException<InvalidNameException>(
                    () => sut.Save(pilot.PilotId, pilot)
                );
        }


        [TestMethod]
        public void Save_WithNullDateOfBirth_ShouldThrowInvalidDateException()
        {
            // arrange
            
            pilot.DateOfBirth = null;
            // act 
            // assert
            Assert.ThrowsException<InvalidDateException>(
                    () => sut.Save(pilot.PilotId, pilot)
                );
        }


        [TestMethod]
        public void Save_WithInvalidDateOfBirth_ShouldThrowInvalidDateException()
        {
            // arrange
          
            pilot.DateOfBirth = DateTime.Now.AddYears(-20);
            // act 
            // assert
            Assert.ThrowsException<InvalidDateException>(
                    () => sut.Save(pilot.PilotId, pilot)
                );
        }


        [TestMethod]
        public void Save_WithNUllYearsOfExperience_ShouldThrowInvalidYearsOfExperienceException()
        {
            // arrange
           
            pilot.YearsOfExperience = null;
            // act 
            // assert
            Assert.ThrowsException<InvalidYearsOfExperienceException>(
                    () => sut.Save(pilot.PilotId, pilot)
                );
        }


        [TestMethod]
        public void Save_WithInvalidYearsOfExperience_ShouldThrowInvalidYearsOfExperienceException()
        {
            // arrange
            pilot.YearsOfExperience = 9;
            // act 
            // assert
            Assert.ThrowsException<InvalidYearsOfExperienceException>(
                    () => sut.Save(pilot.PilotId, pilot)
                );
        }


        [TestMethod]
        public void Save_WithNullDateActivated_ShouldThrowInvalidDateException()
        {
            // arrange
           
            pilot.DateActivated = null;
            // act 
            // assert
            Assert.ThrowsException<InvalidDateException>(
                    () => sut.Save(pilot.PilotId, pilot)
                );
        }


        [TestMethod]
        public void Save_WithValidPilotCodeFormat_ShouldCallReposiryCreate()
        {
            // arrange

            // act 
            sut.Save(pilot.PilotId, pilot);
            // assert
            mockRepo
                .Verify(
                    r => r.Create(pilot), Times.Once
                );
        }

        [TestMethod]
        public void Save_WithInValidPilotCodeFormat_ShouldThrowInvalidPilotCodeException()
        {
            // arrange
            pilot.FirstName = "1Emmanuel";

            // act 
            // assert
            Assert.ThrowsException<InvalidPilotCodeException>(
                    () => sut.Save(pilot.PilotId, pilot)
                );

            mockRepo
                .Verify(
                    r => r.Create(pilot), Times.Never
                );

        }

        [TestMethod]
        public void Save_WithExistingPilotCode_ShouldThrowExistingPilotCodeException()
        {
            // arrange

            pilot.FirstName = "Try";
            pilot.MiddleName = "Try";

            mockRepo
                .Setup(
                    r => r.Retrieve(existingCode)
                )
                .Returns(new Pilot());

            // act 
            // assert

            Assert.ThrowsException<ExistingPilotCodeException>(
                    () => sut.Save(pilot.PilotId, pilot)
                );

            mockRepo
                .Verify(
                    r => r.Retrieve(existingCode), Times.Once
                );

            mockRepo
                .Verify(
                    r => r.Create(pilot), Times.Never
                ); 
        }


        [TestMethod]
        public void Save_WithNoneExistingPilotCode_ShouldCallRepositoryCreate()
        {
            // act 
            sut.Save(pilot.PilotId, pilot);

            // assert
            mockRepo
                .Verify(
                    r => r.Retrieve(nonExistingCode), Times.Once
                );

            mockRepo
                .Verify(
                    r => r.Create(pilot), Times.Once
                );
        }


        [TestMethod]
        public void Save_WithExistingPilotId_ShouldCallRepositoryUpdate()
        {
            // arrange
            pilot.PilotId = existingPilotId;

            // act 
            sut.Save(pilot.PilotId, pilot);

            // assert
            mockRepo
                .Verify(
                    r => r.Retrieve(existingPilotId), Times.Once
                );
            mockRepo
                .Verify(
                    r => r.Update(pilot), Times.Once
                );
        }

        [TestMethod]
        public void Save_WithNoneExistingPilotId_ShouldCallRepositoryCreate()
        {
            // arrange
            pilot.PilotId = noneExistingPilotId;

            mockRepo
                .Setup(
                    r => r.Retrieve(noneExistingPilotId)
                )
                .Returns<Pilot>(null);

            // act 
            sut.Save(pilot.PilotId, pilot);

            // assert
            mockRepo
                .Verify(
                    r => r.Retrieve(noneExistingPilotId), Times.Once
                );
            mockRepo
                .Verify(
                    r => r.Create(pilot), Times.Once
                );
        }

    }
}
