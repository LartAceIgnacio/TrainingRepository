using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using BlastAsia.Digibook.Domain.Venues;
using BlastAsia.Digibook.Domain.Models.Venues;

namespace BlastAsia.Digibook.Domain.Test.Venues
{
    [TestClass]
    public class VenueServiceTest
    {
        private Mock<IVenueRepository> venueRepository;
        private Mock<IVenueService> venueService;
        VenueService sut;
        Venue venue;
        Guid existingId = Guid.NewGuid();
        Guid nonExistingId = Guid.Empty;

        [TestInitialize]
        public void InitializeData()
        {
            venueRepository = new Mock<IVenueRepository>();
            venueService = new Mock<IVenueService>();
            sut = new VenueService(venueRepository.Object, venueService.Object);
            venue = new Venue();

            venue.VenueName = "My Venue";
            venue.Description = "Description";
        }

        [TestMethod]
        public void SaveVenue_WithValidData_ShouldCallRepositoryCreate()
        {
            var result = sut.Save(nonExistingId,venue);

            venueRepository
                .Verify(vr => vr.Create(venue), Times.Once);
        }

        [TestMethod]
        public void SaveVenue_WithValidData_ShouldCallRepositoryUpdate()
        {
            venue.VenueId = existingId;
            var result = sut.Save(existingId,venue);

            venueRepository
                .Verify(vr => vr.Update(venue.VenueId,venue), Times.Once);
        }

        [TestMethod]
        public void SaveVenue_WithVenueNameIsMoreThan50Characters_ThrowsInvalidStringLenghtException()
        {
            venue.VenueName = "less 50 less 50 less 50 less 50 less 50 less 50 less 50";

            Assert.ThrowsException<InvalidStringLenghtException>(
                ()=> sut.Save(nonExistingId,venue));

            venueRepository
                .Verify(vr => vr.Create(venue), Times.Never);
            
        }

        [TestMethod]
        public void SaveVenue_WithVenueNameIsBlank_ThrowsInvalidStringLengthException()
        {
            venue.VenueName = "";

            Assert.ThrowsException<InvalidStringLenghtException>(
                () => sut.Save(nonExistingId,venue));

            venueRepository
                .Verify(vr => vr.Create(venue), Times.Never);
        }

        [TestMethod]
        public void SaveVenue_WithDescriptionIsGreaterThan100Characters_ThrowsInvalidStringLengthException()
        {
            venue.Description = "more than 100 asldjfaklsdfjoqwiehfaksldkhfowaefjasldhgfaowlefhaopfhajoslfjaoslkhfjaoslefjao;lsdifhaosdkfjaosdlfkjaosdl";

            Assert.ThrowsException<InvalidStringLenghtException>(
                () => sut.Save(nonExistingId,venue));

            venueRepository
                .Verify(vr => vr.Create(venue), Times.Never);
        }
    }
}
