using BlastAsia.DigiBook.Domain.Models.Appointments;
using BlastAsia.DigiBook.Infrastracture.Persistence.Repositories;
using BlastAsia.DigiBook.Insfrastracture.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Infrastracture.Persistence.Test
{
    [TestClass]
    public class AppointmentRepositoryTest
    {
        private Appointment appointment;
        private DbContextOptions<DigiBookDbContext> dbOptions;
        private DigiBookDbContext dbContext;
        private readonly string connectionString = @"Data Source=.; Database=DigiBookDb; Integrated Security=true;";
        private AppointmentRepository sut;

        private Guid existingGuestId = Guid.NewGuid();
        private Guid existingHostId = Guid.NewGuid();

        [TestInitialize]
        public void Initialize()
        {
            appointment = new Appointment
            {
                AppointmentId = new Guid(),
                AppointmentDate = DateTime.Now.AddDays(1),
                GuestId = existingGuestId,
                HostId = existingHostId,
                StartTime = new DateTime().TimeOfDay,
                EndTime = new DateTime().TimeOfDay,
                IsCancelled = false,
                IsDone = true,
                Notes = "Sample Notes"
            };

            dbOptions = new DbContextOptionsBuilder<DigiBookDbContext>()
                                   .UseSqlServer(connectionString)
                                   .Options;

            dbContext = new DigiBookDbContext(dbOptions); // ORM
            dbContext.Database.EnsureCreated();
            sut = new AppointmentRepository(dbContext); // System under test

        }

        [TestCleanup]
        public void Cleanup()
        {
            dbContext.Dispose();
            dbContext = null;
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Create_WithValidData_SavesRecordToDatabase()
        {
            // act
            var newEmployee = sut.Create(appointment);

            // assert 
            Assert.IsNotNull(newEmployee);
            Assert.IsTrue(newEmployee.AppointmentId != Guid.Empty);

            // Cleanup
            sut.Delete(newEmployee.AppointmentId);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Delete_WithAnExistingAppointment_RemovesRecordFromDatabase()
        {
            // arrange 
            // var sut = new appointmentRepository(dbContext); // System under test
            var newAppointment = sut.Create(appointment);

            // act 
            sut.Delete(newAppointment.AppointmentId);
            // assert
            appointment = sut.Retrieve(newAppointment.AppointmentId);
            Assert.IsNull(appointment);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Retrieve_WithExistingAppointmentId_ReturnsRecordFromDatabase()
        {
            // arrange
            var newAppointment = sut.Create(appointment);
            //act
            var found = sut.Retrieve(newAppointment.AppointmentId);
            // assert 
            Assert.IsNotNull(found);

            sut.Delete(newAppointment.AppointmentId);
        }

        [TestMethod]
        public void Retrieve_WithPaginationWithValidData_ReturnsRecordFromDatabase()
        {
            // arrange
            var newAppointment = sut.Create(appointment);
            var pageNumber = 1;
            var recordNumber = 5;
            var date = newAppointment.AppointmentDate;
            // act 
            var found = sut.Retrieve(pageNumber, recordNumber, date);
            // assert
            Assert.IsNotNull(found);

            sut.Delete(newAppointment.AppointmentId);
        }


        [TestMethod]
        public void Retrieve_WithInvalidDate_ReturnsDefaultRecordFromDataBase()
        {
            // arrange
            var newAppointment = sut.Create(appointment);
            var pageNumber = 1;
            var recordNumber = 5;
            DateTime? date = null;
            // act 
            var found = sut.Retrieve(pageNumber, recordNumber, date);
            // assert
            Assert.IsNotNull(found);

            sut.Delete(newAppointment.AppointmentId);
        }


        [TestMethod]
        public void Retrieve_WithInvalidPageNumber_ReturnsDefaultRecordFromDataBase()
        {
            // arrange
            var newAppointment = sut.Create(appointment);
            var pageNumber = -1;
            var recordNumber = 5;
            DateTime? date = newAppointment.AppointmentDate;

            // act 
            var found = sut.Retrieve(pageNumber, recordNumber, date);
            // assert
            Assert.IsNotNull(found);

            sut.Delete(newAppointment.AppointmentId);
        }

        [TestMethod]
        public void Retrieve_WithInvalidRecordNumber_ReturnsDefaultRecordFromDataBase()
        {
            // arrange
            var newAppointment = sut.Create(appointment);
            var pageNumber = 1;
            var recordNumber = -5;
            DateTime? date = newAppointment.AppointmentDate;
            // act 
            var found = sut.Retrieve(pageNumber, recordNumber, date);
            // assert
            Assert.IsNotNull(found);

            sut.Delete(newAppointment.AppointmentId);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Update_WithExistingAppointmentId_SaveAndUpdateInDatabase()
        {
            //arrange
            var newAppointment = sut.Create(appointment);

            var expectedAppointmentDate = DateTime.Now.AddDays(2);
            var expectedStartTime = new DateTime().AddHours(1).TimeOfDay;
            var expectedEndTime = new DateTime().AddHours(5).TimeOfDay;
            var expectedIsCancelled = true;
            var expectedIsDone = false;
            var expectedNotes = "asdasdasd asdasd";

            newAppointment.AppointmentDate = expectedAppointmentDate;
            newAppointment.StartTime = expectedStartTime;
            newAppointment.EndTime = expectedEndTime;
            newAppointment.IsCancelled = expectedIsCancelled;
            newAppointment.IsDone = expectedIsDone;
            newAppointment.Notes = expectedNotes;

            // act
            sut.Update(newAppointment.AppointmentId, appointment);

            var Updatedappointment = sut.Retrieve(newAppointment.AppointmentId);
            // assert 
            Assert.AreEqual(Updatedappointment.AppointmentDate, expectedAppointmentDate);
            Assert.AreEqual(Updatedappointment.StartTime, expectedStartTime);
            Assert.AreEqual(Updatedappointment.EndTime, expectedEndTime);
            Assert.AreEqual(Updatedappointment.IsCancelled, expectedIsCancelled);
            Assert.AreEqual(Updatedappointment.IsDone, expectedIsDone);
            Assert.AreEqual(Updatedappointment.Notes, expectedNotes);
            // cleanup
            sut.Delete(Updatedappointment.AppointmentId);
        }
    }
}
