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
    public class AppointementRepositoryTest
    {
        private Appointment appointment = null;
        private DbContextOptions<DigiBookDbContext> dbOptions = null;
        private DigiBookDbContext dbContext = null;
        private String connectionString = null;
        private AppointmentRepository sut = null;
        private ContactRepository sutContact = null;
        private EmployeeRepository sutEmployee = null;
        private Guid existingGuestId = Guid.NewGuid();
        private Guid existingHostId = Guid.NewGuid();


        [TestInitialize]
        public void TestInitialize()
        {
            connectionString =
                @"Data Source=.;Database=DigiBookDb;Integrated Security=true;";
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
                AppointmentId = new Guid(),
                AppointmentDate = DateTime.Now.AddDays(1),
                StartTime = new DateTime().TimeOfDay,
                EndTime = new DateTime().TimeOfDay,
                IsCancelled = false,
                IsDone = true,
                Notes = "Sample Notes",
                GuestId = sutContact.Retrieve().FirstOrDefault().ContactId,
                HostId = sutEmployee.Retrieve().FirstOrDefault().EmployeeId

            };
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

            newAppointment.AppointmentDate = expectedAppointmentDate;
            newAppointment.StartTime = expectedStartTime;
            newAppointment.EndTime = expectedEndTime;
            newAppointment.IsCancelled = expectedIsCancelled;
            newAppointment.IsDone = expectedIsDone;
            newAppointment.Notes = expectedNotes;

            // act
            sut.Update(newAppointment.AppointmentId, appointment);

            var UpdatedContact = sut.Retrieve(newAppointment.AppointmentId);
            // assert 
            Assert.AreEqual(UpdatedContact.AppointmentDate, expectedAppointmentDate);
            Assert.AreEqual(UpdatedContact.StartTime, expectedStartTime);
            Assert.AreEqual(UpdatedContact.EndTime, expectedEndTime);
            Assert.AreEqual(UpdatedContact.IsCancelled, expectedIsCancelled);
            Assert.AreEqual(UpdatedContact.IsDone, expectedIsDone);
            Assert.AreEqual(UpdatedContact.Notes, expectedNotes);
            // cleanup
            sut.Delete(UpdatedContact.AppointmentId);
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
    }
}
