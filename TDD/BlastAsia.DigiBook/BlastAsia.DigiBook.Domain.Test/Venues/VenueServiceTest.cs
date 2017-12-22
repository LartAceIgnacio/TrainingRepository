using BlastAsia.DigiBook.Domain.Models.Venues;
using BlastAsia.DigiBook.Domain.Venues;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Test.Venues
{
    
    [TestClass]
    public class VenueServiceTest
    {

        private Venue venue;
        private Mock<IVenueRepository> mockVenueRepository;
        private VenueService sut;
        private Guid nonExistingId;
        private Guid ExistingId;

        [TestInitialize]
        public void Initialize()
        {
            venue = new Venue
            {

                VenueName = "It Department",

            };

            mockVenueRepository = new Mock<IVenueRepository>();
            sut = new VenueService(mockVenueRepository.Object);

            nonExistingId = Guid.Empty;
            ExistingId = Guid.NewGuid();

            mockVenueRepository
                .Setup(vr => vr.Retrieve(nonExistingId))
                .Returns<Venue>(null);

            mockVenueRepository
                .Setup(vr => vr.Retrieve(ExistingId))
                .Returns(venue);
        }


        [TestMethod]
        public void Save_WithBlanVenueName_ThrowsVenueNameRequiredException()
        {
            //Arrange
            venue.VenueName = "";

            //Assert
            Assert.ThrowsException<VenueNameRequiredException>(
                ()=> sut.Save(venue.VenueId,venue)
                );

            mockVenueRepository
                .Verify(vr => vr.Create(venue), Times.Never());
        }

        [TestMethod]
        public void Save_withValidData_ShouldCallRepositoryCreateAndGenerateVenueId()
        {
            //Arrange
            venue.VenueId = nonExistingId;

            mockVenueRepository
                .Setup(vr => vr.Create(venue))
                .Callback(() => venue.VenueId = ExistingId)
                .Returns(venue);
            
            //Act
            var result = sut.Save(nonExistingId, venue);

            //Assert
            mockVenueRepository
                .Verify(vr => vr.Retrieve(nonExistingId), Times.Once());

            mockVenueRepository
                .Verify(vr => vr.Create(venue), Times.Once());

            Assert.IsTrue(result.VenueId != nonExistingId);

        }

        [TestMethod]
        public void Save_withExistingVenueId_ShouldCallRepositoryUpdate()
        {
            //Arrange
            venue.VenueId = ExistingId;

            //Act
            var result = sut.Save(ExistingId, venue);

            //Assert
            mockVenueRepository
                .Verify(vr => vr.Retrieve(ExistingId), Times.Once());

            mockVenueRepository
                .Verify(vr => vr.Update(ExistingId,venue), Times.Once());

        }
    }
}
