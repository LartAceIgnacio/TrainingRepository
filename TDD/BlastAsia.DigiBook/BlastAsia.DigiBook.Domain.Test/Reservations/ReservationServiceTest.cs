using BlastAsia.DigiBook.Domain.Models.Reservations;
using BlastAsia.DigiBook.Domain.Reservations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Test.Reservations
{
    [TestClass]
    public class ReservationServiceTest
    {
        private Reservation reservation;
        private Mock<IReservationRepository> mockReservationRepository;
        private ReservationService sut;

        [TestInitialize]
        public void Initialize()
        {
            reservation = new Reservation
            {
                VenueName = "Super Luxury Hotel",
                Description = "5 Star Hotel",
                StartDate = DateTime.Now.AddDays(3),
                EndDate = DateTime.Now.AddDays(5)
            };
            mockReservationRepository = new Mock<IReservationRepository>();
            sut = new ReservationService(mockReservationRepository.Object);
        }

        [TestCleanup]
        public void CleanUp()
        {

        }

        [TestMethod]
        public void Save_ReservationWithValidDetails_ShouldCallRepositoryCreate()
        {
            // Act
            sut.Save(reservation.ReservationId, reservation);

            // Assert
            mockReservationRepository.Verify(r => r.Create(reservation), Times.Once);

        }

        [TestMethod]
        public void Save_ReservationWithExistingData_ShouldCallRepositoryUpdate()
        {
            // Arrange
            mockReservationRepository
                .Setup(r => r.Retrieve(reservation.ReservationId))
                .Returns(reservation);

            // Act
            sut.Save(reservation.ReservationId, reservation);

            // Assert
            mockReservationRepository.Verify(r => r.Retrieve(reservation.ReservationId), Times.Once);
            mockReservationRepository.Verify(r => r.Update(reservation.ReservationId, reservation), Times.Once);
        }

        [TestMethod]
        public void Save_ReservationWithValidData_ReturnReservationWithId()
        {
            // Arrange
            mockReservationRepository
                .Setup(r => r.Create(reservation))
                .Callback(() => reservation.ReservationId = Guid.NewGuid())
                .Returns(reservation);

            // Act
            sut.Save(reservation.ReservationId, reservation);

            // Assert
            mockReservationRepository.Verify(r => r.Create(reservation), Times.Once);
            Assert.IsTrue(reservation.ReservationId != Guid.Empty);
        }

        [TestMethod]
        public void Save_WithBlankVenueName_ShouldReturnVenueNameRequiredException()
        {
            // Arrange
            reservation.VenueName = null;

            // Assert
            Assert.ThrowsException<VenueNameRequiredException>(
                () => sut.Save(reservation.ReservationId, reservation));
            mockReservationRepository.Verify(r => r.Create(reservation), Times.Never);
        }

        [TestMethod]
        public void Save_WithVenueNameMoreThanMaximumCharacters_ShouldReturnMaximumCharacterExceededException()
        {
            // Arrange
            reservation.VenueName = "Venue more than 50 characters. Venue more than 50 characters. Venue more than 50 characters.";

            // Assert
            Assert.ThrowsException<MaximumCharacterExceededException>(
                () => sut.Save(reservation.ReservationId, reservation));
            mockReservationRepository.Verify(r => r.Create(reservation), Times.Never);
        }

        [TestMethod]
        public void Save_WithDescriptionMoreThanMaximumCharacters_ShouldReturnMaximumCharacterExceededException()
        {
            // Arrange
            reservation.Description = "DescriptionMorethan100Characters.DescriptionMorethan100Characters." +
                "DescriptionMorethan100Characters.DescriptionMorethan100Characters.";

            // Assert
            Assert.ThrowsException<MaximumCharacterExceededException>(
                () => sut.Save(reservation.ReservationId, reservation));
            mockReservationRepository.Verify(r => r.Create(reservation), Times.Never);

        }

        [TestMethod]
        public void Save_StartDateLessThanCurrentDate_ShouldReturnScheduleRequiredException()
        {
            // Arrange
            reservation.StartDate = DateTime.Now.AddDays(-1);

            // Assert
            Assert.ThrowsException<ScheduleRequiredException>(
                () => sut.Save(reservation.ReservationId, reservation));
            mockReservationRepository.Verify(r => r.Create(reservation), Times.Never);
        }

        [TestMethod]
        public void Save_StartDateIsEqualToEndDate_ShouldReturnScheduleRequiredException()
        {
            // Arrange
            reservation.StartDate = reservation.EndDate;

            // Assert
            Assert.ThrowsException<ScheduleRequiredException>(
                () => sut.Save(reservation.ReservationId, reservation));
            mockReservationRepository.Verify(r => r.Create(reservation), Times.Never);

        }

        [TestMethod]
        public void Save_EndDateIsEqualsToCurrentDate_ShouldReturnScheduleRequiredException()
        {
            // Arrange 
            reservation.EndDate = DateTime.Now;

            // Assert
            Assert.ThrowsException<ScheduleRequiredException>(
                () => sut.Save(reservation.ReservationId, reservation));
            mockReservationRepository.Verify(r => r.Create(reservation), Times.Never);
        }
    }
}
