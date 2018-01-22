using BlastAsia.DigiBook.API.Controllers;
using BlastAsia.DigiBook.Domain.Models.Reservations;
using BlastAsia.DigiBook.Domain.Reservations;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.API.Test
{
    [TestClass]
    public class ReservationControllerTest
    {
        private Mock<IReservationService> mockReservationService;
        private Mock<IReservationRepository> mockReservationRepository;
        private ReservationController sut;
        private Reservation reservation;
        private JsonPatchDocument patchedReservation;

        [TestInitialize]
        public void Initialize()
        {
            mockReservationService = new Mock<IReservationService>();
            mockReservationRepository = new Mock<IReservationRepository>();
            sut = new ReservationController(mockReservationService.Object, mockReservationRepository.Object);
            reservation = new Reservation
            {
                VenueName = "Super Luxury Hotel",
                Description = "5 star hotel",
                StartDate = DateTime.Now.AddDays(7),
                EndDate = DateTime.Now.AddDays(10)
            };

            patchedReservation = new JsonPatchDocument();
            patchedReservation.Replace("VenueName", "Super Hotel");

            mockReservationRepository
                .Setup(r => r.Retrieve(reservation.ReservationId))
                .Returns(reservation);
        }

        [TestCleanup]
        public void Cleanup()
        {

        }

        [TestMethod]
        public void GetReservations_WithEmptyReservationId_ReturnsOkObjectResult()
        {
            // Arrange
            mockReservationRepository
                .Setup(r => r.Retrieve())
                .Returns(new List<Reservation>());

            // Act
            var result = sut.GetReservations(null);

            // Assert
            mockReservationRepository.Verify(r => r.Retrieve(), Times.Once);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void GetReservations_WithReservationId_ReturnsOkObjectResult()
        {
            // Act
            var result = sut.GetReservations(reservation.ReservationId);

            // Assert
            mockReservationRepository.Verify(r => r.Retrieve(reservation.ReservationId), Times.Once);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void CreateReservation_ReservationWithValidData_ReturnCreatedAtActionResult()
        {
            // Act
            var result = sut.CreateReservation(reservation);

            // Assert
            mockReservationService.Verify(r => r.Save(reservation.ReservationId, reservation), Times.Once);
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
        }

        [TestMethod]
        public void CreateReservation_ReservationWithEmptyData_ReturnBadRequestResult()
        {
            // Arrange
            reservation = null;

            // Act
            var result = sut.CreateReservation(reservation);

            // Assert
            mockReservationService.Verify(r => r.Save(Guid.Empty, reservation), Times.Never);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void DeleteReservation_ReservationDeleted_ReturnNoContentResult()
        {
            // Act
            var result = sut.DeleteReservation(reservation.ReservationId);

            // Assert
            mockReservationRepository.Verify(r => r.Delete(reservation.ReservationId), Times.Once);
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        [TestMethod]
        public void DeleteReservation_WithoutReservationId_ReturnNotFoundResult()
        {
            // Arrange
            mockReservationRepository
                .Setup(r => r.Retrieve(reservation.ReservationId))
                .Returns<Reservation>(null);

            // Act
            var result = sut.DeleteReservation(reservation.ReservationId);

            // Assert
            mockReservationRepository.Verify(r => r.Delete(reservation.ReservationId), Times.Never);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void UpdateReservation_WithExistingReservationDataAndId_ReturnOkObjectResult()
        {
            // Act
            var result = sut.UpdateReservation(reservation, reservation.ReservationId);

            // Assert
            mockReservationRepository.Verify(r => r.Retrieve(reservation.ReservationId), Times.Once);
            mockReservationService.Verify(r => r.Save(reservation.ReservationId, reservation), Times.Once);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void UpdateReservation_ReservationWithoutValue_ReturnBadRequestResult()
        {
            // Arrange
            reservation = null;

            // Act
            var result = sut.UpdateReservation(reservation, Guid.Empty);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            mockReservationRepository.Verify(r => r.Retrieve(Guid.Empty), Times.Never);
            mockReservationService.Verify(r => r.Save(Guid.Empty, reservation), Times.Never);
        }

        [TestMethod]
        public void UpdateReservation_WithNoExistingReservation_ReturnNotFoundResult()
        {
            // Arrange
            reservation.ReservationId = Guid.Empty;
            mockReservationRepository
                .Setup(r => r.Retrieve(reservation.ReservationId))
                .Returns<Reservation>(null);

            // Act
            var result = sut.UpdateReservation(reservation, reservation.ReservationId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            mockReservationRepository.Verify(r => r.Retrieve(reservation.ReservationId), Times.Once);
            mockReservationService.Verify(r => r.Save(reservation.ReservationId, reservation), Times.Never);
        }

        [TestMethod]
        public void PatchReservation_WithExistingReservationDataAndId_ReturnOkObjectResult()
        {
            // Act
            var result = sut.PatchReservation(patchedReservation, reservation.ReservationId);

            // Assert
            mockReservationService.Verify(r => r.Save(reservation.ReservationId, reservation), Times.Once);
            mockReservationRepository.Verify(r => r.Retrieve(reservation.ReservationId), Times.Once);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void PatchReservation_WithoutExistingDataAndId_ReturnNotFoundResult()
        {
            // Arrange
            mockReservationRepository
                .Setup(r => r.Retrieve(reservation.ReservationId))
                .Returns<Reservation>(null);

            // Act
            var result = sut.PatchReservation(patchedReservation, reservation.ReservationId);

            // Assert
            mockReservationRepository.Verify(r => r.Retrieve(reservation.ReservationId), Times.Once);
            mockReservationService.Verify(r => r.Save(reservation.ReservationId, reservation), Times.Never);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));

        }

        [TestMethod]
        public void PatchReservation_WithEmptyPatchDocument_ReturnBadRequestResult()
        {
            // Arrange
            patchedReservation = null;

            // Act
            var result = sut.PatchReservation(patchedReservation, reservation.ReservationId);

            // Assert
            mockReservationRepository.Verify(r => r.Retrieve(reservation.ReservationId), Times.Never);
            mockReservationService.Verify(r => r.Save(reservation.ReservationId, reservation), Times.Never);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }
    }
}
