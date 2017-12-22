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
        private Mock<IVenueRepository> mockVenueRepository;
        private VenueService sut;
        private Venue venue;


        [TestInitialize]
        public void Initialize()
        {
            venue = new Venue {
                VenueId = Guid.NewGuid(),
                VenueName = "House",
                Description = "Party"
            };

            mockVenueRepository = new Mock<IVenueRepository>();
            sut = new VenueService(mockVenueRepository.Object);

            mockVenueRepository
                .Setup(x => x.Retrieve(venue.VenueId))
                .Returns(venue);
            
            mockVenueRepository
                .Setup(x => x.Retrieve(Guid.Empty))
                .Returns<Venue>(null);
        }

        [TestMethod]
        public void Save_WithValidData_ShouldCallRepositoryCreate()
        {
            //Arrange

            //Act
            sut.Save(Guid.Empty, venue);

            //Assert
            mockVenueRepository.Verify(x => x.Retrieve(Guid.Empty), Times.Once);
            mockVenueRepository.Verify(x => x.Create(venue), Times.Once);
        }
        
        [TestMethod]
        public void Save_WithValidData_ShouldCallRepositoryUpdate()
        {
            //Arrange

            //Act
            sut.Save(venue.VenueId, venue);

            //Assert
            mockVenueRepository.Verify(x => x.Retrieve(venue.VenueId), Times.Once);
            mockVenueRepository.Verify(x => x.Update(venue.VenueId, venue), Times.Once);
        }

        [TestMethod]
        public void Save_WithBlankVenueName_ThrowsVenueNameRequiredException()
        {
            //Arrange
            venue.VenueName = "";

            //Act


            //Assert
            Assert.ThrowsException<VenueNameRequiredException>(() => sut.Save(venue.VenueId, venue));
            mockVenueRepository.Verify(x => x.Retrieve(venue.VenueId), Times.Never);

        }

        [TestMethod]
        public void Save_WithMoreThanFiftyCharsInVenueName_ThrowsVenueNameMaxLengthException()
        {
            //Arrange
            venue.VenueName = "      Automatic Raise Credit Limit in Bank....      ";

            //Act


            //Assert
            Assert.ThrowsException<VenueNameMaxLengthException>(() => sut.Save(venue.VenueId, venue));
            mockVenueRepository.Verify(x => x.Retrieve(venue.VenueId), Times.Never);
        }

        [TestMethod]
        public void Save_WithMoreThanOneHundreadCharsInVenueDescription_ThrowsVenueDescriptionMaxLengthException()
        {
            //Arrange
            venue.Description = "      Automatic Raise Credit Limit in Bank....            Automatic Raise Credit Limit in Bank....      ";

            //Act


            //Assert
            Assert.ThrowsException<VenueDescriptionMaxLengthException>(() => sut.Save(venue.VenueId, venue));
            mockVenueRepository.Verify(x => x.Retrieve(venue.VenueId), Times.Never);
        }
    }
}
