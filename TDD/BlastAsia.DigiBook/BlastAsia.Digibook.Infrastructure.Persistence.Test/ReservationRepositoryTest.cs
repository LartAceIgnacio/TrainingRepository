using BlastAsia.DigiBook.Domain.Models.Reservations;
using BlastAsia.DigiBook.Infrastructure.Persistence;
using BlastAsia.DigiBook.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.Digibook.Infrastructure.Persistence.Test
{
    [TestClass]
    public class ReservationRepositoryTest
    {
        private Reservation reservation;
        private String connectionString;
        private DbContextOptions<DigiBookDbContext> dbOptions;
        private DigiBookDbContext dbContext;
        private ReservationRepository sut;

        [TestInitialize]
        public void Initialize()
        {
            reservation = new Reservation
            {
                VenueName = "Super Luxury Hotel",
                Description = "5 star hotel",
                StartDate = DateTime.Now.AddDays(5),
                EndDate = DateTime.Now.AddDays(7)
            };

            connectionString =
                @"Server=.;Database=DigiBookDb;Integrated Security=true;";
            dbOptions = new DbContextOptionsBuilder<DigiBookDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            dbContext = new DigiBookDbContext(dbOptions);
            dbContext.Database.EnsureCreated();

            sut = new ReservationRepository(dbContext);
        }

        [TestCleanup]
        public void Cleanup()
        {
            dbContext.Dispose();
            dbContext = null;
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Create_WithValidData_SaveRecordInDatabase()
        {
            // Act
            var newReservation = sut.Create(reservation);

            // Assert
            Assert.IsNotNull(newReservation);
            Assert.IsTrue(newReservation.ReservationId != Guid.Empty);

            // Cleanup
            sut.Delete(reservation.ReservationId);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Delete_WithAnExistingReservation_RemovesRecordFromDatabase()
        {
            // Arrange
            var newReservation = sut.Create(reservation);

            // Act
            sut.Delete(newReservation.ReservationId);

            // Assert
            reservation = sut.Retrieve(newReservation.ReservationId);
            Assert.IsNull(reservation);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Retrieve_WithExistingReservationId_ReturnsRecordFromDb()
        {
            // Arrange
            var newReservation = sut.Create(reservation);

            // Act
            var found = sut.Retrieve(newReservation.ReservationId);

            // Assert
            Assert.IsNotNull(found);
            sut.Delete(found.ReservationId);
        }

        [TestMethod]
        [TestProperty ("TestType", "Integration")]
        public void Update_WithValidData_SavesUpdateInDb()
        {
            // Arrange
            var newReservation = sut.Create(reservation);

            var expectedVenueName = "Super Cafe Hotel";
            var expectedDescription = "Cafe";
            var expectedStartDate = DateTime.Now.AddDays(2);
            var expectedEndDate = DateTime.Now.AddDays(5);

            newReservation.VenueName = expectedVenueName;
            newReservation.Description = expectedDescription;
            newReservation.StartDate = expectedStartDate;
            newReservation.EndDate = expectedEndDate;

            // Act
            sut.Update(newReservation.ReservationId, reservation);

            // Assert
            var updatedReservation = sut.Retrieve(newReservation.ReservationId);

            Assert.AreEqual(expectedVenueName, updatedReservation.VenueName);
            Assert.AreEqual(expectedDescription, updatedReservation.Description);
            Assert.AreEqual(expectedStartDate, updatedReservation.StartDate);
            Assert.AreEqual(expectedEndDate, updatedReservation.EndDate);

            // Cleanup
            sut.Delete(updatedReservation.ReservationId);
        }
    }
}
