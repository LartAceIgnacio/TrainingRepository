
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
        private string _connectionString;
        private Appointment _appointment;
        private AppointmentRepository _sut;
        private DigiBookDbContext _dbContext = null;
        private DbContextOptions<DigiBookDbContext> _dbOptions = null;

        [TestInitialize]
        public void InitializeTest()
        {

            _appointment = new Appointment
            {
                AppointmentDate = DateTime.Today.AddDays(1),
                GuestId = Guid.NewGuid(),
                HostId = Guid.NewGuid(),
                StartTime = DateTime.Now.TimeOfDay,
                EndTime = DateTime.Now.TimeOfDay.Add(TimeSpan.Parse("01:00:00")),
                IsCancelled = false,
                IsDone = true,
                Notes = "Sucess"
            };

            _connectionString = @"Data Source=.;Database=DigiBookDb;Integrated Security=true;";
            _dbOptions = new DbContextOptionsBuilder<DigiBookDbContext>()
                .UseSqlServer(_connectionString)
                .Options;

            _dbContext = new DigiBookDbContext(_dbOptions); // ORM
            _dbContext.Database.EnsureCreated();
            _sut = new AppointmentRepository(_dbContext);
        }


        [TestCleanup]
        public void Cleanup()
        {
            _dbContext.Dispose();
            _dbContext = null;
        }

        [TestMethod]
        [TestProperty("TestType", "Appointment")]
        public void Create_WithValidData_SavesRecordToDatabase()
        {
            // arrange

            // act 
            var newAppointment = _sut.Create(_appointment);



            // assert
            Assert.IsNotNull(newAppointment);
            Assert.IsTrue(newAppointment.AppointmentId != Guid.Empty);

            // Cleanup
            _sut.Delete(newAppointment.AppointmentId);
        }



        [TestMethod]
        [TestProperty("TestType", "Appointment")]
        public void Delete_WithAnExistingAppointmentId_RemovesRecordFromDatabase()
        {
            //Arrange
            var newAppointment = _sut.Create(_appointment);

            //Act
            _sut.Delete(newAppointment.AppointmentId);

            //Assert

            _appointment = _sut.Retrieve(newAppointment.AppointmentId);
            Assert.IsNull(_appointment);
        }

        [TestMethod]
        [TestProperty("TestType", "Appointment")]
        public void Retrieve_WithAnExistingAppointmentId_ReturnsAppointmentRecordsFromDb()
        {
            //arrange
            var newAppointment = _sut.Create(_appointment);

            //act
            var retrieveAppointment = _sut.Retrieve(newAppointment.AppointmentId);

            //assert
            Assert.IsNotNull(retrieveAppointment);

            //cleanup
            _sut.Delete(newAppointment.AppointmentId);
        }

        [TestMethod]
        [TestProperty("TestType", "Appointment")]
        public void Update_WithAnExistingAppointmentId_ShouldUpdateAppointmentRecordsFromDb()
        {
            //Arrange
            var newAppointment = _sut.Create(_appointment);
            var expectedNotes = "Updated Note";

            //changing the value of properties
            newAppointment.Notes = expectedNotes;
            //Act
            _sut.Update(newAppointment.AppointmentId, newAppointment);
            var updatedAppointment = _sut.Retrieve(newAppointment.AppointmentId);

            //Assert   
            Assert.AreEqual(updatedAppointment.Notes, expectedNotes);

            //cleanup
            _sut.Delete(updatedAppointment.AppointmentId);
        }
    }
}