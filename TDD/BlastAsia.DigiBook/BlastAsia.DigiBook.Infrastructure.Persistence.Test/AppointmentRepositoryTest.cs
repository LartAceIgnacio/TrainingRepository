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
        private string connectionString;
        private DbContextOptions<DigiBookDbContext> dbOptions = null;
        private AppointmentRepository sut;
        private DigiBookDbContext dbContext;
        private Guid existingAppointmentId = Guid.NewGuid();
        private Guid nonExistingAppointmentId = Guid.Empty;
        private Guid existingGuestId = Guid.NewGuid();
        private Guid existingHostId = Guid.NewGuid();

        // private Appointment newAppointment;
        [TestInitialize()]
        public void Initialize()
        {
            appointment = new Appointment
            {
                AppointmentDate = DateTime.Today,
                GuestId = existingGuestId,
                HostId = existingHostId,
                StartTime = DateTime.Now.TimeOfDay,
                EndTime = DateTime.Now.TimeOfDay.Add(TimeSpan.Parse("1:00:00")),
                IsCancelled = false,
                IsDone = false,
                Notes = "Interview"
            };


            connectionString =
               @"Data Source=.; Database=DigiBookDb; Integrated Security=true";

            dbOptions = new DbContextOptionsBuilder<DigiBookDbContext>()
                .UseSqlServer(connectionString)
                .Options;
            dbContext = new DigiBookDbContext(dbOptions);
            dbContext.Database.EnsureCreated();
            sut = new AppointmentRepository(dbContext);


        }
        [TestCleanup()]
        public void CleanUp()
        {

        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Create_WithValidData_SaveRecordInTheDatabase()
        {
            //Arrange

            //Act
            var newAppointment = sut.Create(appointment);

            //Assert
            Assert.IsNotNull(appointment);
            Assert.IsTrue(newAppointment.AppointmentId != Guid.Empty);

            //CleanUp
            sut.Delete(newAppointment.AppointmentId);

        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Delete_WithAnExistingAppointment_RemovesRecordFromDatabase()
        {
            var newAppointment = sut.Create(appointment);
            //Act
            sut.Delete(newAppointment.AppointmentId);

            appointment = sut.Retrieve(newAppointment.AppointmentId);
            Assert.IsNull(appointment);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Retrieve_WithExistingAppointmentId_ReturnsRecordFromDb()
        {

            var newAppointment = sut.Create(appointment);

            var found = sut.Retrieve(newAppointment.AppointmentId);

            Assert.IsNotNull(found);
            sut.Delete(newAppointment.AppointmentId);

        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Update_WithValidData_SaveUpdatesInDb()
        {
            //ARRANGE
            var newAppointment = sut.Create(appointment);
            var expectedAppointmentDate = DateTime.Today;
            var expectedGuestId = existingGuestId;
            var expectedHostId = existingHostId;
            var expectedStartTime = DateTime.Now.TimeOfDay;
            var expectedEndTime = DateTime.Now.TimeOfDay.Add(TimeSpan.Parse("1:00:00"));
            var expectedIsCancelled = true;
            var expectedIsDone = true;
            var expectedNotes = "Data";
       
            //ACT
            newAppointment.AppointmentDate = expectedAppointmentDate;
            newAppointment.GuestId = expectedGuestId;
            newAppointment.HostId = existingHostId;
            newAppointment.StartTime = expectedStartTime;
            newAppointment.EndTime = expectedEndTime;
            newAppointment.IsCancelled = expectedIsCancelled;
            newAppointment.IsDone = expectedIsDone;
            newAppointment.Notes = expectedNotes; 

            sut.Update(newAppointment.AppointmentId, newAppointment);
            
            //ASSERT
            var updatedAppointment = sut.Retrieve(newAppointment.AppointmentId);
            Assert.AreEqual(expectedAppointmentDate, updatedAppointment.AppointmentDate);
            Assert.AreEqual(expectedGuestId, updatedAppointment.GuestId);
            Assert.AreEqual(expectedHostId, updatedAppointment.HostId);
            Assert.AreEqual(expectedStartTime, updatedAppointment.StartTime);
            Assert.AreEqual(expectedEndTime, updatedAppointment.EndTime);
            Assert.AreEqual(expectedIsDone, updatedAppointment.IsDone);
            Assert.AreEqual(expectedNotes, updatedAppointment.Notes);

            //CleanUp
            sut.Delete(updatedAppointment.AppointmentId);
        }
    }
}
