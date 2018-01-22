using BlastAsia.DigiBook.Domain.Models.Appointments;
using BlastAsia.DigiBook.Infrastructure.Persistence;
using BlastAsia.DigiBook.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlastAsia.Digibook.Infrastructure.Persistence.Test
{
    [TestClass]
    public class AppointmentRepositoryTest
    {
        private Appointment appointment = null;
        private DbContextOptions<DigiBookDbContext> dbOptions = null;
        private DigiBookDbContext dbContext = null;
        private String connectionString = null;
        private AppointmentRepository sut = null;
        private ContactRepository sutContact = null;
        private EmployeeRepository sutEmployee = null;

        private Guid existingGuesttId = Guid.NewGuid();
        private Guid existingHostId = Guid.NewGuid();

        [TestInitialize]
        public void Initialize()
        {
            connectionString =
                @"Server=.;Database=DigiBookDb;Integrated Security=true;";
            dbOptions = new DbContextOptionsBuilder<DigiBookDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            dbContext = new DigiBookDbContext(dbOptions);
            dbContext.Database.EnsureCreated();

            sut = new AppointmentRepository(dbContext);
            sutContact = new ContactRepository(dbContext);
            sutEmployee = new EmployeeRepository(dbContext);

            appointment = new Appointment
            {
                AppointmentDate = DateTime.Today,
                StartTime = DateTime.Now.TimeOfDay,
                EndTime = DateTime.Now.TimeOfDay.Add(TimeSpan.Parse("01:00:00")),
                IsCancelled = false,
                IsDone = true,
                Notes = "Ongoing",
                GuestId = sutContact.Retrieve().FirstOrDefault().ContactId,
                HostId = sutEmployee.Retrieve().FirstOrDefault().EmployeeId
            };
        }

        [TestCleanup]
        public void Cleanup()
        {
            dbContext.Dispose();
            dbContext = null;
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Create_AppointmentWithValidData_SavesRecordInTheDatabase()
        {
            // Act
            var newAppointment = sut.Create(appointment);

            // Assert
            Assert.IsNotNull(newAppointment);
            Assert.IsTrue(newAppointment.AppointmentId != Guid.Empty);

            // Cleanup
            sut.Delete(appointment.AppointmentId);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Delete_WithAnExistingAppointment_RemovesRecordFromDatabase()
        {
            // Arrange            
            var newAppointment = sut.Create(appointment);

            // Act
            sut.Delete(newAppointment.AppointmentId);

            // Assert
            appointment = sut.Retrieve(newAppointment.AppointmentId);
            Assert.IsNull(appointment);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Retrieve_WithExistingAppointmentId_ReturnsRecordFromDb()
        {
            // Arrange
            var newAppointment = sut.Create(appointment);

            // Act
            var found = sut.Retrieve(newAppointment.AppointmentId);

            // Assert
            sut.Delete(found.AppointmentId);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Update_WithValidData_SavesUpdatesInDb()
        {
            // Arrange
            var newAppointment = sut.Create(appointment);
            var expectedAppointmentDate = DateTime.Today;
            var expectedGuestId = appointment.GuestId;
            var expectedHostId = appointment.HostId;
            var expectedStartTime = DateTime.Now.TimeOfDay;
            var expectedEndTime = DateTime.Now.TimeOfDay.Add(TimeSpan.Parse("01:00:00"));
            var expectedIsCancelled = false;
            var expectedIsDone = true;
            var expectedNotes = "Ongoing";

            newAppointment.AppointmentDate = expectedAppointmentDate;
            newAppointment.GuestId = expectedGuestId;
            newAppointment.HostId = expectedHostId;
            newAppointment.StartTime = expectedStartTime;
            newAppointment.EndTime = expectedEndTime;
            newAppointment.IsCancelled = expectedIsCancelled;
            newAppointment.IsDone = expectedIsDone;
            newAppointment.Notes = expectedNotes;

            // Act
            sut.Update(newAppointment.AppointmentId, newAppointment);

            // Assert
            var updatedAppointment = sut.Retrieve(newAppointment.AppointmentId);
            Assert.AreEqual(expectedAppointmentDate, updatedAppointment.AppointmentDate);
            Assert.AreEqual(expectedGuestId, updatedAppointment.GuestId);
            Assert.AreEqual(expectedHostId, updatedAppointment.HostId);
            Assert.AreEqual(expectedStartTime, updatedAppointment.StartTime);
            Assert.AreEqual(expectedEndTime, updatedAppointment.EndTime);
            Assert.AreEqual(expectedIsCancelled, updatedAppointment.IsCancelled);
            Assert.AreEqual(expectedIsDone, updatedAppointment.IsDone);
            Assert.AreEqual(expectedNotes, updatedAppointment.Notes);

            //Cleanup
            sut.Delete(updatedAppointment.AppointmentId);
        }
    }
}
