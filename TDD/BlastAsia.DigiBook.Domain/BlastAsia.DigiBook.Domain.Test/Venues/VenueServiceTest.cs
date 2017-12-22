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
        private Mock<IVenueRepository> mockRepository;
        private VenueService sut;
        private Venue venue;
        private Guid existingId = Guid.NewGuid();
        [TestInitialize]
        public void Initialize()
        {
            mockRepository = new Mock<IVenueRepository>();
            sut = new VenueService(mockRepository.Object);

            venue = new Venue
            {
                VenueId = new Guid(),
                VenueName = "Venue name",
                Description = ""
            };

        }

        [TestMethod]
        public void Save_WithValidData_ShouldCallVenueRepositoryCreate()
        {
           
            sut.Save(venue.VenueId,venue);

            mockRepository
                .Verify(r => r.Create(venue), Times.Once);
        }

        [TestMethod]
        public void Save_WithValidId_ShouldCallVenuRepositoryUpdate()
        {
            venue.VenueId = existingId;

            mockRepository
                .Setup(r => r.Retrieve(venue.VenueId))
                .Returns(venue);

            sut.Save(venue.VenueId, venue);

            mockRepository
                .Verify(r => r.Retrieve(existingId), Times.Once);

            mockRepository
                .Verify(r => r.Update(existingId, venue), Times.Once);
        }

        [TestMethod]
        public void Save_WithInvalidVenueName_ShouldThrowVenueNameRequiredException()
        {
            venue.VenueName = "";

            Assert.ThrowsException<VenueNameRequiredException>(
                () => sut.Save(venue.VenueId, venue)
                );

            mockRepository
                .Verify(r => r.Retrieve(venue.VenueId), Times.Never);
        }

        [TestMethod]
        public void Save_VenueNameExceedsTheLimit_ShouldThrowVenueNameException()
        {
            venue.VenueName = "123456789012345678901234567890123456789012345678901234567890";

            Assert.ThrowsException<VenueNameRequiredException>(
                () => sut.Save(venue.VenueId, venue)
                );

            mockRepository
                .Verify(r => r.Retrieve(venue.VenueId), Times.Never);
        }

        [TestMethod]
        public void Save_DescriptionExceedsTheLimit_ShouldThrowDescriptionException()
        {
            venue.Description = "12345678901234567890123456789012345678901234567890" +
                "123456789012345678901234567890123456789012345678901234567890";
            Assert.ThrowsException<DescriptionException>(
                 () => sut.Save(venue.VenueId, venue)
                 );
            mockRepository
                .Verify(r => r.Retrieve(venue.VenueId), Times.Never);
        }
    }
}
