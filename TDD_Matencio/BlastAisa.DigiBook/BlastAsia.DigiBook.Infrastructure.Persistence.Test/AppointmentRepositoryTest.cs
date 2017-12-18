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

        private Guid existingContactId = Guid.NewGuid();
        private Guid existingEmployeeId = Guid.NewGuid();

        [TestInitialize]
        public void TestInitialize()
        {
            appointment = new Appointment
            {
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
        public void TestCleanup()
        {
            dbContext.Dispose();
            dbContext = null;
        }

        [TestMethod]
        [TestProperty("TestType","Integration")]
        public void Create_WithValidData_SavesRecordToTheDataBase()
        {
            //Arrange
            var newAppointment = sut.Create(appointment);
            //Act

            //Assert
            Assert.IsNotNull(newAppointment);
            Assert.IsTrue(newAppointment.appointmentId != Guid.Empty);
            //Cleanup
            sut.Delete(appointment.appointmentId);
        }

        [TestMethod]
        [TestProperty("TestType","Integration")]
        public void Retrieve_WithValidData_ReturnsRecordFromDatabase()
        {
            //Arrange
            var newAppointment = sut.Create(appointment);
            //Act
            var found = sut.Retrieve(appointment.appointmentId);
            //Assert
            Assert.IsNotNull(found);
            //Cleanup
            sut.Delete(appointment.appointmentId);
        }

        [TestMethod]
        [TestProperty("TestType","Integration")]
        public void Update_WithValidData_UpdatesRecordFromDatabase()
        {
            //Arrange
            var newAppointment = sut.Create(appointment);

            var expectedAppointmentDate = DateTime.Today.AddDays(5);
            var expectedStartTime = DateTime.Now.TimeOfDay.Add(TimeSpan.Parse("01:00:00"));
            var expectedEndTime = DateTime.Now.TimeOfDay.Add(TimeSpan.Parse("03:00:00"));

            newAppointment.AppointmentDate = expectedAppointmentDate;
            newAppointment.StartTime = expectedStartTime;
            newAppointment.EndTime = expectedEndTime;
            //Act
            sut.Update(newAppointment.appointmentId, appointment);
            //Assert
            var update = sut.Retrieve(newAppointment.appointmentId);

            Assert.AreEqual(newAppointment.AppointmentDate, update.AppointmentDate);
            Assert.AreEqual(newAppointment.StartTime, update.StartTime);
            Assert.AreEqual(newAppointment.EndTime, update.EndTime);
            //Cleanup
            sut.Delete(appointment.appointmentId);
        }
    }
}
