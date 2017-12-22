using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using BlastAsia.DigiBook.Domain.Models.Venues;
using BlastAsia.DigiBook.Domain.Venues;
using BlastAsia.DigiBook.Domain.Venues.Exceptions;
using BlastAsia.DigiBook.Domain.Models.Venues.Exceptions;

namespace BlastAsia.DigiBook.Domain.Test.Venues
{

    [TestClass]
    public class VenueServiceTest
    {
        private Mock<IVenueRepository> mockRepo;
        private VenueService sut;
        private Venue venue;

        private readonly Guid nonExistingId = Guid.Empty;
        private readonly Guid existingId = Guid.NewGuid();


        [TestInitialize]
        public void Initialize()
        {
            mockRepo = new Mock<IVenueRepository>();

            sut = new VenueService(mockRepo.Object);

            venue = new Venue
            {
                VenueId = new Guid(),
                VenueName = "Venue Sample",
                Description = "Description"
            };

        }

        [TestCleanup]
        public void Cleanup()
        {

        }

        [TestMethod]
        public void Save_WithValidData_ShouldCallRepositoryCreateAndReturnVenueId()
        {
            // arrange

            mockRepo
                .Setup(
                    r => r.Create(venue)
                )
                .Callback(
                    () =>
                    {
                        venue.VenueId = Guid.NewGuid();
                    }
                )
                .Returns(venue);

            // act 
            var result = sut.Save(venue.VenueId, venue);

            // assert
            mockRepo
                .Verify(
                    r => r.Create(venue), Times.Once
                );

            Assert.IsTrue(result.VenueId != Guid.Empty);
        }


        [TestMethod]
        public void Save_WithBlankVenueName_ShouldThrowInvalidVenueNameException()
        {
            // arrange
            venue.VenueName = "";
            // act 

            // assert
            Assert.ThrowsException<InvalidVenueNameException>(
                () => sut.Save(venue.VenueId, venue)
                );

            mockRepo
                .Verify(
                    r => r.Create(venue), Times.Never
                );
        }


        [TestMethod]
        public void Save_WithInvalidVenueNameLength_ShouldThrowInvalidVenueNameException()
        {
            // arrange
            venue.VenueName = "asdasdasddasd asdasddasdasdasddasdas dasddasdasdasddasdasdasdd";
            // act 

            // assert
            Assert.ThrowsException<InvalidVenueNameException>(
                () => sut.Save(venue.VenueId, venue)
                );

            mockRepo
                .Verify(
                    r => r.Create(venue), Times.Never
                );
        }

        [TestMethod]
        public void Save_WithInvalidDescriptionLength_ShouldThrowInvalidDescriptionException()
        {
            // arrange
            venue.Description = "asdasdasddasd asdasddasdasdasddasdas dasddasdasdasddasdasdasdd asdasdasddasd asdasddasdasdasddasdas dasddasdasdasddasdasdasdd asdasdasddasd asdasddasdasdasddasdas dasddasdasdasddasdasdasdd";
            // act 

            // assert
            Assert.ThrowsException<InvalidDescriptionLengthException>(
                () => sut.Save(venue.VenueId, venue)
                );

            mockRepo
                .Verify(
                    r => r.Create(venue), Times.Never
                );
        }

        [TestMethod]
        public void Save_WithNonExistingVenueId_ShoulCallRepositoryRetrieveAndCreate()
        {
            // arrange
            mockRepo
                .Setup(
                    r => r.Retrieve(nonExistingId)
                )
                .Returns<Venue>(null);

            // act 
            sut.Save(nonExistingId, venue);

            // assert

            mockRepo
                .Verify(
                    r => r.Retrieve(nonExistingId), Times.Once
                );

            mockRepo
                .Verify(
                    r => r.Create(venue), Times.Once
                );
        }

        [TestMethod]
        public void Save_WithExistingVenueId_ShoulCallRepositoryRetrieveAndUpdate()
        {
            // arrange
            mockRepo
                .Setup(
                    r => r.Retrieve(existingId)
                )
                .Returns(venue);

            // act 
            sut.Save(existingId, venue);

            // assert

            mockRepo
                .Verify(
                    r => r.Retrieve(existingId), Times.Once
                );

            mockRepo
                .Verify(
                    r => r.Create(venue), Times.Never
                );

            mockRepo
              .Verify(
                  r => r.Update(existingId, venue), Times.Once
              );
        }
    }
}
