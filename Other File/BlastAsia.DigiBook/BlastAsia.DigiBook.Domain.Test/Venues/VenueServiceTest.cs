using BlastAsia.DigiBook.Domain.Models.Venues;
using BlastAsia.DigiBook.Domain.Venues;
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
        private Object result;
        private Guid existingVenueId = Guid.NewGuid();
        private Guid nonExisitingVenueId = Guid.Empty;

        [TestInitialize]
        public void Init()
        {
            venue = new Venue
            { 
                VenueName = "Training Room",
                Description = "Where new trainees are trained"
            };
            mockVenueRepository = new Mock<IVenueRepository>();
            sut = new VenueService(mockVenueRepository.Object);

            mockVenueRepository
                .Setup(vr => vr.Retrieve(existingVenueId))
                .Returns(venue);
        }

        [TestMethod]
        public void Save_WithNewValidData_ShouldCallSaveRepository()
        {
            //Arrange  
            mockVenueRepository
               .Setup(vr => vr.Create(venue))
               .Returns(venue);
            //Act 
            result = sut.Save(Guid.Empty, venue);
           
            //Assert
            Assert.IsNotNull(result);
            mockVenueRepository
                .Verify(vr => vr.Create(venue), Times.Once);
        }

        [TestMethod]
        public void Save_WithExisitngVenueId_ShouldCallUpdateRepository()
        {
            //Arrange
            venue.VenueId = existingVenueId;
            //Act 
            sut.Save(venue.VenueId, venue);
            //Assert
            mockVenueRepository
                .Verify(vr => vr.Update(venue.VenueId, venue), Times.Once);
            mockVenueRepository
                .Verify(vr => vr.Create(venue), Times.Never);
        }

        [TestMethod]
        public void Save_WithBlankEmptVenueName_ThrowsRequiredException()
        {
            //Arrange
            venue.VenueName = "";
            //Act 
  
            //Assert
            Assert.ThrowsException<RequiredVenueNameException>(
                ()=> sut.Save(Guid.Empty, venue));
            mockVenueRepository
                .Verify(vr => vr.Create(venue), Times.Never);
        }

        [TestMethod]
        public void Save_WithVenueNameGreaterThanMaximumLength_ThrowsMaximumVenueNameException()
        {
            //Arrange
            venue.VenueName = "Miusov, as a man man of breeding and deilcacy, could not but feel some inwrd qualms, when he reached the Father Superior's with Ivan: he felt ashamed of havin lost his temper. He felt that he ought to have disdaimed that despicable wretch, Fyodor Pavlovitch, too much to have been upset by him in Father Zossima's cell, ...";
            //Act 

            //Assert
            Assert.ThrowsException<MaximumVenueNameException>(
               ()=> sut.Save(Guid.Empty, venue));
            mockVenueRepository
               .Verify(vr => vr.Create(venue), Times.Never);
        }


        [TestMethod]
        public void Save_WithDescriptionGreaterThanMaximumLength_ThrowsMaximumDescriptionLengthException()
        {
            //Arrange
            venue.Description = "Miusov, as a man man of breeding and deilcacy, could not but feel some inwrd qualms, when he reached the Father Superior's with Ivan: he felt ashamed of havin lost his temper. He felt that he ought to have disdaimed that despicable wretch, Fyodor Pavlovitch, too much to have been upset by him in Father Zossima's cell, ...";
            //Act 

            //Assert
            Assert.ThrowsException<MaximumDescriptionLengthException>(
                () => sut.Save(Guid.Empty, venue));
            mockVenueRepository
                .Verify(vr => vr.Create(venue), Times.Never);
        }
    }
}
