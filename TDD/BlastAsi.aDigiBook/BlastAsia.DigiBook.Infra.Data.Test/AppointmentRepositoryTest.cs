using BlastAsia.DigiBook.Domain.Models.Appointments;
using BlastAsia.DigiBook.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Infrastructure.Persistence.Test
{
    [TestClass]
    public class AppointmentRepositoryTest
    {
        private Appointment appointment;
        private DbContextOptions<DigiBookDbContext> dbOptions = null;
        private DigiBookDbContext dbContext = null;
        private String connectionString = null;
        private AppointmentRepository sut;

        [TestInitialize]
        public void Initialize()
        {
            appointment = new Appointment
            {
                GuestId = Guid.NewGuid(),
                HostId = Guid.NewGuid(),
                AppointmentDate = DateTime.Now,
                StartTime = DateTime.Now.TimeOfDay,
                EndTime = DateTime.Now.TimeOfDay.Add(TimeSpan.Parse("02:00:00")),
                IsCancelled = false,
                IsDone = false,
                Notes = "Bring coffee."
            };

            connectionString =
                @"Data Source=.;Database=DigiBookDb;Integrated Security=true;";

            dbOptions = new DbContextOptionsBuilder<DigiBookDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            dbContext = new DigiBookDbContext(dbOptions);
            dbContext.Database.EnsureCreated();

            sut = new AppointmentRepository(dbContext);

        }

        [TestCleanup]
        public void CleanUp()
        {
            dbContext.Dispose();
            dbContext = null;
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Create_WithValidData_SavesRecordInDatabase()
        {
            //Arrange

            //Act
            var newAppointment = sut.Create(appointment);

            //Assert
            Assert.IsNotNull(newAppointment);
            Assert.IsTrue(newAppointment.AppointmentId != Guid.Empty);

            //CleanUp
            sut.Delete(newAppointment.AppointmentId);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Delete_WithAnExistingAppointment_RemovesRecordFromDatabase()
        {
            //Arrange
            var newAppointment = sut.Create(appointment);

            //Act
            sut.Delete(appointment.AppointmentId);

            //Assert
            appointment = sut.Retrieve(newAppointment.AppointmentId);
            Assert.IsNull(appointment);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Retrieve_WithExistingAppointmentId_ReturnsRecordFromDb()
        {
            //Arrange
            var newAppointment = sut.Create(appointment);

            //Act
            var found = sut.Retrieve(newAppointment.AppointmentId);

            //Assert
            Assert.IsNotNull(found);

            //CleanUp
            sut.Delete(found.AppointmentId);
        }

        //[TestMethod]
        //[TestProperty("TestType", "Integration")]
        public void Update_WithValidData_SavesUpdatesInDb()
        {
            //Arrange
            var newAppointment = sut.Create(appointment);
            var expectedGuestId = Guid.NewGuid();
            var expectedHostId = Guid.NewGuid();
            var expectedAppointmentDate = DateTime.Today.AddDays(+5);
            var expectedStartTime = DateTime.Now.TimeOfDay.Add(TimeSpan.Parse("05:00:00"));
            var expectedEndTime = expectedStartTime.Add(TimeSpan.Parse("05:00:00"));
            var expectedIsCancelled = true;
            var expectedIsDone = true;
            var expectedNotes = "Cancelled due to bad weather.";

            newAppointment.GuestId = expectedGuestId;
            newAppointment.HostId = expectedHostId;
            newAppointment.AppointmentDate = expectedAppointmentDate;
            newAppointment.StartTime = expectedStartTime;
            newAppointment.EndTime = expectedEndTime;
            newAppointment.IsCancelled = expectedIsCancelled;
            newAppointment.IsDone = expectedIsDone;
            newAppointment.Notes = expectedNotes;

            //Act
            sut.Update(newAppointment.AppointmentId, newAppointment);

            //Assert
            var updatedAppointment = sut.Retrieve(newAppointment.AppointmentId);
            Assert.AreEqual(expectedGuestId, updatedAppointment.GuestId);
            Assert.AreEqual(expectedHostId, updatedAppointment.HostId);
            Assert.AreEqual(expectedAppointmentDate, updatedAppointment.AppointmentDate);
            Assert.AreEqual(expectedStartTime, updatedAppointment.StartTime);
            Assert.AreEqual(expectedEndTime, updatedAppointment.EndTime);
            Assert.AreEqual(expectedIsCancelled, updatedAppointment.IsCancelled);
            Assert.AreEqual(expectedIsDone, updatedAppointment.IsDone);
            Assert.AreEqual(expectedNotes, updatedAppointment.Notes);

            //CleanUp
            sut.Delete(updatedAppointment.AppointmentId);
        }
    }
}
