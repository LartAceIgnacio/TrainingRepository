﻿using BlastAsia.DigiBook.Domain.Models.Pilots;
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

        [TestInitialize]
        public void Initialize()
        {
            pilot = new Pilot
            {
                FirstName = "Christoper",
                MiddleName = "Magdaleno",
                LastName = "Manuel",
                BirthDate = DateTime.Parse("Feb-01-1995"),
                YearsOfExperience = 10,
                DateActivated = DateTime.Today,
                PilotCode = "FFMMLLLLYYmmdd",
                DateCreated = DateTime.Now,
                DateModified = new Nullable<DateTime>()
            };

            mockPilotRepository = new Mock<IPilotRepository>();

            sut = new PilotService(mockPilotRepository.Object);

            mockPilotRepository
                .Setup(pr => pr.Retrieve(existingPilotId))
                .Returns(pilot);
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
    }

}
