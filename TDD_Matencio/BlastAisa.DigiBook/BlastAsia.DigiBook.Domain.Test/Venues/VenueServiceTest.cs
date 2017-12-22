﻿using BlastAsia.DigiBook.Domain.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Test
{
    [TestClass]
    public class VenueServiceTest
    {
        private Venue venue;
        private Mock<IVenueRepository> mockVenueRepository;
        private VenueService sut;
        private Guid nonExistingId;

        [TestInitialize]
        public void TestInitialize()
        {
            venue = new Venue
            {
                VenueId = Guid.NewGuid(),
                VenueName = "Training Room",
                VenueDescription = "Hello"
            };
            nonExistingId = Guid.Empty;
            mockVenueRepository = new Mock<IVenueRepository>();
            sut = new VenueService(mockVenueRepository.Object);


            mockVenueRepository
                .Setup(v => v.Retrieve(venue.VenueId))
                .Returns(venue);


            mockVenueRepository
                .Setup(v => v.Retrieve(nonExistingId))
                .Returns<Venue>(null);
        }

        [TestCleanup]
        public void TestCleanup()
        {

        }

        [TestMethod]
        public void Save_WithValidData_ShouldCallRepositoryCreate()
        {
            //Arrange
            //Act
            var result = sut.Save(nonExistingId, venue);
            //Assert
            mockVenueRepository
                .Verify(v => v.Create(venue), Times.Once);
        }

        [TestMethod]
        public void Save_WithEmptyVenueName_ThrowsVenueNameRequired()
        {
            //Arrange
            venue.VenueName = "";
            //Act
            //Asert
            Assert.ThrowsException<VenueNameRequiredException>(
                () => sut.Save(venue.VenueId, venue));
        }
        [TestMethod]
        public void Save_WithVenueNameInvalidLength_ThrowsInvalidLengthException()
        {
            //Arrange
            venue.VenueName = "12345678901234567890123456789012345678901234567890q";
            //Act
            //Assert
            Assert.ThrowsException<VenueNameInvalidLengthException>(
                () => sut.Save(venue.VenueId, venue));
        }

        [TestMethod]
        public void Save_WithVenueDescriptionInvalidLength_ThrowsInvalidLengthException()
        {
            //Arrange
            venue.VenueDescription = "1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890q";
            //Act
            //Assert
            Assert.ThrowsException<VenueDescriptionInvalidLengthException>(
                () => sut.Save(venue.VenueId, venue));
        }

        [TestMethod]
        public void Save_WithExistingVenueId_ShouldCallRepositoryUpdate()
        {
            //Arrange
            //Act
            var result = sut.Save(venue.VenueId, venue);
            //Asert
            mockVenueRepository
                .Verify(m => m.Retrieve(venue.VenueId), Times.Once);
            mockVenueRepository
                .Verify(m => m.Update(venue.VenueId, venue), Times.Once);
        }

        [TestMethod]
        public void Save_WithNonExistingVenueId_ShouldCallRepositoryCreate()
        {
            //Arrange
            //Act
            var result = sut.Save(nonExistingId, venue);
            //Assert
            mockVenueRepository
                .Verify(m => m.Retrieve(nonExistingId), Times.Once);

            mockVenueRepository
                .Verify(m => m.Create(venue), Times.Once);
        }
    }
}
