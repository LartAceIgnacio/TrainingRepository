using BlastAsia.DigiBook.Domain.Models.Appointments;
using BlastAsia.DigiBook.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
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

        private AppointmentRepository sut= null;
        public ContactRepositories sutContact = null;
        public EmployeeRepository sutEmployee = null;

        public Guid existingContactId;
        public Guid existingEmployeeId;

        [TestInitialize]
        public void Initialize()
        {
         

            connectionString =
               @"Data Source=.;Database=DigiBookDb;Integrated Security=true;";
            dbOptions = new DbContextOptionsBuilder<DigiBookDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            dbContext = new DigiBookDbContext(dbOptions);
            dbContext.Database.EnsureCreated();

            sut = new AppointmentRepository(dbContext);
            sutEmployee = new EmployeeRepository(dbContext);
            sutContact = new ContactRepositories(dbContext);

            existingContactId = sutContact.Retrieve().FirstOrDefault().ContactId;
            existingEmployeeId = sutEmployee.Retrieve().FirstOrDefault().EmployeeId;

            appointment = new Appointment
            {
                AppointmentDate = DateTime.Today,
                GuestId = existingContactId,
                HostId = existingEmployeeId,
                Notes = "Notes"
            };
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
