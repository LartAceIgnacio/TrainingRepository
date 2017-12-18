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
        public Appointment Appointment = null;
        public string connectionString = null;
        public DbContextOptions<DigiBookDbContext> dbOptions = null;
        public DigiBookDbContext dbContext = null;
        public AppointmentRepository sut = null;
        private Guid existingContactId = Guid.NewGuid();
        private Guid existingEmployeeId = Guid.NewGuid();

        [TestInitialize]
        public void Initialize()
        {
            Appointment = new Appointment {
                AppointmentDate = DateTime.Today,
                GuestId = existingContactId,
                HostId = existingEmployeeId,
                StartTime = DateTime.Now.TimeOfDay,
                EndTime = DateTime.Now.TimeOfDay.Add(TimeSpan.Parse("01:00:00")),
                IsCancelled = false,
                IsDone = true,
                Notes = "Sucess"
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
        public void Cleanup()
        {
            dbContext.Dispose();
            dbContext = null;
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Create_WithValidData_SavesRecordInTheDatabase()
        {
            //Arrange

            //Act
            var newAppointment = sut.Create(Appointment);

            //Assert
            Assert.IsNotNull(newAppointment);
            Assert.IsTrue(newAppointment.AppointmentId != Guid.Empty);

            //CleanUp
            sut.Delete(newAppointment.AppointmentId);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Delete_WithExistingContact_RemovesRecordFromDatabase()
        {
            //Arrange
            var newAppointment = sut.Create(Appointment);

            //Act
            sut.Delete(newAppointment.AppointmentId);

            //Assert
            Appointment = sut.Retrieve(newAppointment.AppointmentId);
            Assert.IsNull(Appointment);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Retrieve_WithExistingContactId_ReturnsRecordFromDatabase()
        {
            //Arrange
            var newAppointment = sut.Create(Appointment);

            //Act
            var found = sut.Retrieve(newAppointment.AppointmentId);

            //Assert
            Assert.IsNotNull(found);

            //Cleanup
            sut.Delete(newAppointment.AppointmentId);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Update_WithValidData_SavesUpdatesInDatabase()
        {
            //Arrange
            var newAppointment = sut.Create(Appointment);

            var newAppointmentDate = DateTime.Now;
            var newGuestId = Guid.NewGuid();
            var newHostId = Guid.NewGuid();
            var newStartTime = DateTime.Now.TimeOfDay.Add(TimeSpan.Parse("01:00:00"));
            var newEndTime = DateTime.Now.TimeOfDay.Add(TimeSpan.Parse("02:00:00"));
            var newIsCancelled = true;
            var newIsDone = false;
            var newNotes = "";

            newAppointment.AppointmentDate = newAppointmentDate;
            newAppointment.GuestId = newGuestId;
            newAppointment.HostId = newHostId;
            newAppointment.StartTime = newStartTime;
            newAppointment.EndTime = newEndTime;
            newAppointment.IsCancelled = newIsCancelled;
            newAppointment.IsDone = newIsDone;
            newAppointment.Notes = newNotes;

            //Act
            sut.Update(newAppointment.AppointmentId, newAppointment);

            //Assert
            var updatedContact = sut.Retrieve(newAppointment.AppointmentId);
            Assert.AreEqual(updatedContact.AppointmentDate, newAppointment.AppointmentDate);
            Assert.AreEqual(updatedContact.GuestId, newAppointment.GuestId);
            Assert.AreEqual(updatedContact.HostId, newAppointment.HostId);
            Assert.AreEqual(updatedContact.StartTime, newAppointment.StartTime);
            Assert.AreEqual(updatedContact.EndTime, newAppointment.EndTime);
            Assert.AreEqual(updatedContact.IsCancelled, newAppointment.IsCancelled);
            Assert.AreEqual(updatedContact.IsDone, newAppointment.IsDone);
            Assert.AreEqual(updatedContact.Notes, newAppointment.Notes);

            //Cleanup
            sut.Delete(newAppointment.AppointmentId);
        }
    }
}
