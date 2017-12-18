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
    public class AppointementRepositoryTest
    {
        private Appointment appointment = null;
        private DbContextOptions<DigiBookDbContext> dbOptions = null;
        private DigiBookDbContext dbContext = null;
        private String connectionString = null;
        private AppointmentRepository sut = null;
        private Guid existingGuestId = Guid.NewGuid();
        private Guid existingHostId = Guid.NewGuid();


        [TestInitialize]
        public void TestInitialize()
        {
            appointment = new Appointment
            {
                AppointmentId = new Guid(),
                AppointmnetDate = DateTime.Now.AddDays(1),
                GuestID = existingGuestId,
                HostId = existingHostId,
                StartTime = new DateTime().TimeOfDay,
                EndTime = new DateTime().TimeOfDay,
                IsCanceleld = false,
                IsDone = true,
                Notes = "Sample Notes"

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
        public void TestCleanUp()
        {
            dbContext.Dispose();
            dbContext = null;
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Create_WithValidData_SavesRecordsInTheDatabase()
        {



            //Act
            appointment.GuestID = Guid.NewGuid();
            appointment.HostId = Guid.NewGuid();

            var newAppointment = sut.Create(appointment);

            //Assert
            Assert.IsNotNull(newAppointment);
            Assert.IsTrue(newAppointment.AppointmentId != Guid.Empty);

            //CleanUp

            sut.Delete(newAppointment.AppointmentId);

        }
        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Retrieve_WithExistingContactId_ReturnsRecordFromDb()
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

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Update_WithValidData_SavesUpdatesInDb()
        {
            //arrange
            var newAppointment = sut.Create(appointment);

            var expectedAppointmentDate = DateTime.Now.AddDays(2);
            var expectedStartTime = new DateTime().AddHours(1).TimeOfDay;
            var expectedEndTime = new DateTime().AddHours(5).TimeOfDay;
            var expectedIsCancelled = true;
            var expectedIsDone = false;
            var expectedNotes = "asdasdasd asdasd";

            newAppointment.AppointmnetDate = expectedAppointmentDate;
            newAppointment.StartTime = expectedStartTime;
            newAppointment.EndTime = expectedEndTime;
            newAppointment.IsCanceleld = expectedIsCancelled;
            newAppointment.IsDone = expectedIsDone;
            newAppointment.Notes = expectedNotes;

            // act
            sut.Update(newAppointment.AppointmentId, appointment);

            var UpdatedContact = sut.Retrieve(newAppointment.AppointmentId);
            // assert 
            Assert.AreEqual(UpdatedContact.AppointmnetDate, expectedAppointmentDate);
            Assert.AreEqual(UpdatedContact.StartTime, expectedStartTime);
            Assert.AreEqual(UpdatedContact.EndTime, expectedEndTime);
            Assert.AreEqual(UpdatedContact.IsCanceleld, expectedIsCancelled);
            Assert.AreEqual(UpdatedContact.IsDone, expectedIsDone);
            Assert.AreEqual(UpdatedContact.Notes, expectedNotes);
            // cleanup
            sut.Delete(UpdatedContact.AppointmentId);
        }
    }
}
