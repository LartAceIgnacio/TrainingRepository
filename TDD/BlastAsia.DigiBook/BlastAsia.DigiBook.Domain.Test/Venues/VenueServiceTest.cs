using BlastAsia.DigiBook.Domain.Models.Venues;
using BlastAsia.DigiBook.Domain.Venues;
using BlastAsia.DigiBook.Domain.Venues.Exceptions;
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
        private Mock<IVenueRepository> mockVenueRepository;
        private Venue venue;
        private VenueService sut;
        private Guid existingVenueId = Guid.NewGuid();

        [TestInitialize]
        public void InitializeTest()
        {
            venue = new Venue
            {
                VenueName = "BlastAsia",
                Description = "Sample"
            };

            mockVenueRepository = new Mock<IVenueRepository>();

            sut = new VenueService(mockVenueRepository.Object);

            mockVenueRepository
                .Setup(c => c.Retrieve(existingVenueId))
                .Returns(venue);

            mockVenueRepository
                .Setup(c => c.Retrieve(venue.VenueID))
                .Returns<Venue>(null);
        }
        [TestCleanup]
        public void CleanupTest()
        {

        }
        [TestMethod]
        public void Save_NewAppointmentWithValidData_ShouldCallRepositoryCreate()
        {
            //Arrange
            
            //Act

            sut.Save(venue.VenueID, venue);

            //Assert

            mockVenueRepository
                .Verify(a => a.Retrieve(venue.VenueID), Times.Once);
            mockVenueRepository
                .Verify(a => a.Create(venue), Times.Once);
        }

        [TestMethod]
        public void Save_WithExistingAppointment_ShouldCallRepositoryUpdate()
        {
            // Arrange 

            venue.VenueID = existingVenueId;

            // Act

            sut.Save(venue.VenueID, venue);

            // Assert

            mockVenueRepository
                .Verify(a => a.Retrieve(venue.VenueID)
                , Times.Once);
            mockVenueRepository
                .Verify(a => a.Update(venue.VenueID, venue)
                , Times.Once);
        }

        [TestMethod]
        public void Create_AppointmentWithValidData_ShouldReturnNewAppointmentWithAppointmentId()
        {
            // Arrange 

            mockVenueRepository
                .Setup(a => a.Create(venue))
                .Callback(() =>
                {
                    venue.VenueID = Guid.NewGuid();
                })
                .Returns(venue);

            // Act 

            var result = sut.Save(venue.VenueID, venue);

            // Assert

            Assert.IsTrue(result.VenueID != Guid.Empty);
        }

        [TestMethod]
        public void Create_VenueWithVenueNameExceeding50Characters_ThrowsNameExceedException()
        {
            // Arrange

            venue.VenueName = "012345678901234567890123456789012345678901234567890";

            // Act

            // Assert

            mockVenueRepository
                .Verify(c => c.Create(venue)
                , Times.Never);
            Assert.ThrowsException<VenueNameException>(
                () => sut.Save(venue.VenueID, venue));
        }
        [TestMethod]
        public void Create_VenueWithDescriptionExceeding100Characters_ThrowsDescriptionExeption()
        {
            // Arrange

            venue.Description = "01234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567891";

            // Act

            // Assert

            mockVenueRepository
                .Verify(c => c.Create(venue)
                , Times.Never);
            Assert.ThrowsException<DescriptionException>(
                () => sut.Save(venue.VenueID, venue));
        }

    }
}
