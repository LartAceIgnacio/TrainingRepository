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
        private Appointment appointment = null;
        private DbContextOptions<DigiBookDbContext> dbOptions = null;
        private DigiBookDbContext dbContext = null;
        private String connectionString = null;
        private AppointmentRepository sut;

        [TestInitialize]
        public void Initialize()
        {
            appointment = new Appointment
            {
                AppointmentDate = DateTime.Today,
                Notes = "Notes"
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
        [TestProperty("TestType", "Integration")]
        public void Create_AppointmentWithValidData_SavesRecordInTheDatabase()
        {

            //Act
            var newAppointment = sut.Create(appointment);

            //Assert
            Assert.IsNotNull(newAppointment);
            Assert.IsTrue(newAppointment.AppointmentId != Guid.Empty);

            //Cleanup
            sut.Delete(newAppointment.AppointmentId);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Delete__WithAnExistingAppointment_RemovesRecordFromDatabase()
        {

            sut = new AppointmentRepository(dbContext);
            var newAppointment = sut.Create(appointment);

            // Act
            sut.Delete(newAppointment.AppointmentId);

            // Assert
            appointment = sut.Retrieve(newAppointment.AppointmentId);
            Assert.IsNull(appointment);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Retrieve_AppointmentWithExistingApointmentId_ReturnsRecordFromDb()
        {
            //Arrange
            var newAppointment = sut.Create(appointment);


            //Act
            var found = sut.Retrieve(newAppointment.AppointmentId);

            //Assert
            Assert.IsNotNull(found);

            //Cleanup
            sut.Delete(found.AppointmentId);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Update_AppointmentWithValidData_SavesUpdatesInDb()
        {
            //Arrange
            var newAppointment = sut.Create(appointment);
            var expectedNotes = "Success"; //from Eugene
        

            newAppointment.Notes = expectedNotes;
         

            //Act
            sut.Update(newAppointment.AppointmentId, newAppointment);


            //Assert
            var updatedAppointment = sut.Retrieve(newAppointment.AppointmentId);
            Assert.AreEqual(expectedNotes, updatedAppointment.Notes);
        
            //Cleanup
            sut.Delete(updatedAppointment.AppointmentId);
        }
    }
}
