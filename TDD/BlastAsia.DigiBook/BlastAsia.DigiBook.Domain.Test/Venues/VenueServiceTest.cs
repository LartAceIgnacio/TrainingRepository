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

        private Guid existingVenueId = Guid.NewGuid();
        private Guid nonExistingVenueId = Guid.Empty;

        [TestInitialize]
        public void Initialize()
        {
            venue = new Venue
            {
                VenueName = "Room C",
                Description = "room for meeting"
            };

            mockVenueRepository = new Mock<IVenueRepository>();

            sut = new VenueService(mockVenueRepository.Object);

            mockVenueRepository
                .Setup(v => v.Create(venue))
                .Callback(() => venue.VenueId = Guid.NewGuid())
                .Returns(venue);

            mockVenueRepository
               .Setup(c => c.Retrieve(existingVenueId))
               .Returns(venue);

            mockVenueRepository
              .Setup(c => c.Retrieve(nonExistingVenueId))
              .Returns<Venue>(null);
        }   

        [TestMethod]
        public void Save_NewVenueWithValidData_ShouldCallRepositoryCreate()
        {
            //Arrange 
            var result = sut.Save(venue);

            //Act 
            mockVenueRepository
               .Verify(c => c.Retrieve(nonExistingVenueId), Times.Once);

            mockVenueRepository
                .Verify(c => c.Create(venue), Times.Once());
        }

        [TestMethod]
        public void Save_WithExistingVenueId_ReturnNewVenueId()
        {
            var newVenue = sut.Save(venue);

            //Assert
            mockVenueRepository
             .Verify(d => d.Create(venue), Times.Once);

            Assert.IsTrue(newVenue.VenueId != Guid.Empty);
        }

        [TestMethod]
        public void Save_WithBlankVenueName_ThrowsVenueNameRequired()
        {
            venue.VenueName = "";

            Assert.ThrowsException<VenueNameRequiredException>(
                () => sut.Save(venue));
        }
        [TestMethod]
        public void Save_WithValidData_ShouldCallRepositoryUpdate()
        {
            // Arrange
            venue.VenueId = existingVenueId;
            //Act

            sut.Save(venue);

            //Assert
            mockVenueRepository
            .Verify(c => c.Retrieve(venue.VenueId), Times.Once);

            mockVenueRepository
            .Verify(c => c.Update(venue.VenueId,venue), Times.Once());
        }
     
    }
   
    
}
