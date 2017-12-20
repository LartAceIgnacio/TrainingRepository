using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlastAsia.Digibook.Domain.Models.Appointments;
using BlastAsia.Digibook.Infrastracture.Persistence;
using BlastAsia.Digibook.Infrastracture.Persistence.Repositories;

namespace BlastAsia.Digibook.Infrastructure.Persistence.Test
{
    [TestClass]
    public class AppointmentRepositoryTest
    {
        private Appointment appointment;
        private string connectionString;
        DbContextOptions<DigiBookDbContext> dbOptions = null;
        private DigiBookDbContext dbContext = null;
        private AppointmentRepository sut = null;
        private Guid existingId = Guid.NewGuid();
        private Guid nonExistingId = Guid.Empty;
        private DateTime appointmentDate = DateTime.Today;

        [TestInitialize]
        public void InitializeData()
        {

            appointment = new Appointment()
            {
                AppointmentDate = appointmentDate,
                GuestId = existingId,
                HostId = existingId,
                StartTime = appointmentDate.AddHours(9),
                EndTime = appointmentDate.AddHours(11),
                IsCanceled = false,
                IsDone = true,
                Notes = "initial test"
            };

            connectionString = @"Data Source=.;Database=DigiBookDb;Integrated Security=true;";
            dbOptions = new DbContextOptionsBuilder<DigiBookDbContext>()
                                    .UseSqlServer(connectionString)
                                    .Options;
            dbContext = new DigiBookDbContext(dbOptions);
            sut = new AppointmentRepository(dbContext);
            dbContext.Database.EnsureCreated();
        }

        [TestCleanup]
        public void Cleanup_Data()
        {
            dbContext.Dispose();
            dbContext = null;
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Create_WithValidData_SavesRecordInTheDatabase()
        {
            var newAppointment = sut.Create(appointment);

            Assert.IsNotNull(newAppointment);
            Assert.IsTrue(newAppointment.AppointmentId != nonExistingId);

            sut.Delete(newAppointment.AppointmentId);

        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Delete_WithExistingAppointment_RemoveRecordFromDatabase()
        {
            var newAppointment = sut.Create(appointment);

            sut.Delete(newAppointment.AppointmentId);
            var retrievedAppointment = sut.Retrieve(newAppointment.AppointmentId);

            Assert.IsNull(retrievedAppointment);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Retrieve_WithExistingAppointment_ReturnsRecordFromDatabase()
        {
            var newAppointment = sut.Create(appointment);

            var retrievedAppointment = sut.Retrieve(newAppointment.AppointmentId);

            Assert.IsNotNull(retrievedAppointment);
            sut.Delete(retrievedAppointment.AppointmentId);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Update_WithValidAppointment_SavesUpdateInDatabase()
        {
            var newAppointment = sut.Create(appointment);
            Guid updatedId = Guid.NewGuid();
            DateTime updatedAppointmentDate = DateTime.Today.AddDays(1);

            var expectedAppointmentDate = updatedAppointmentDate;
            var expectedGuestId = updatedId;
            var expectedHostId = updatedId;
            var expectedStartTime = updatedAppointmentDate.AddHours(9);
            var expectedEndTime = updatedAppointmentDate.AddHours(10);
            var expectedIsCanceled = false;
            var expectedIsDone = false;
            var expectedNotes = "Expected Notes";

            newAppointment.AppointmentDate = expectedAppointmentDate;
            newAppointment.GuestId = expectedGuestId;
            newAppointment.HostId = expectedHostId;
            newAppointment.StartTime = expectedStartTime;
            newAppointment.EndTime = expectedEndTime;
            newAppointment.IsCanceled = expectedIsCanceled;
            newAppointment.IsDone = expectedIsDone;
            newAppointment.Notes = expectedNotes;

            sut.Update(newAppointment.AppointmentId, newAppointment);

            Assert.AreEqual(expectedAppointmentDate, newAppointment.AppointmentDate);
            Assert.AreEqual(expectedGuestId, newAppointment.GuestId);
            Assert.AreEqual(expectedHostId, newAppointment.HostId);
            Assert.AreEqual(expectedStartTime, newAppointment.StartTime);
            Assert.AreEqual(expectedEndTime, newAppointment.EndTime);
            Assert.AreEqual(expectedIsCanceled, newAppointment.IsCanceled);
            Assert.AreEqual(expectedIsDone, newAppointment.IsDone);
            Assert.AreEqual(expectedNotes, newAppointment.Notes);

            sut.Delete(newAppointment.AppointmentId);
        }
    }
}
