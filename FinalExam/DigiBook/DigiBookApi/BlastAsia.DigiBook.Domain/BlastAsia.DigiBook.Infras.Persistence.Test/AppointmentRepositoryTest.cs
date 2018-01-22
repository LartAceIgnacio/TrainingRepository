using BlastAsia.DigiBook.Domain.Models.Appointments;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace BlastAsia.DigiBook.Infrastructure.Persistence.Test
{
    [TestClass]
    public class AppointmentRepositoryTest
    {
        private string connectionString;
        private Appointment appointment;
        private AppointmentRepository sut;
        private DigiBookDbContext dbContext = null;
        private DbContextOptions<DigiBookDbContext> dbOptions = null;
        private ContactRepository sutContact;
        private EmployeeRepository sutEmployee;
        private Guid existingContactId;
        private Guid existingEmployeeId;

        [TestInitialize]
        public void InitializeTest()
        {
          

            connectionString = @"Data Source=.;Database=DigiBookDb;Integrated Security=true;";
            dbOptions = new DbContextOptionsBuilder<DigiBookDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            dbContext = new DigiBookDbContext(dbOptions); // ORM
            dbContext.Database.EnsureCreated();
            sut = new AppointmentRepository(dbContext);
            sutEmployee = new EmployeeRepository(dbContext);
            sutContact = new ContactRepository(dbContext);

            existingContactId = sutContact.Retrieve().FirstOrDefault().ContactId;
            existingEmployeeId = sutEmployee.Retrieve().FirstOrDefault().EmployeeId;
            appointment = new Appointment
            {
                AppointmentDate = DateTime.Today.AddDays(1),
                GuestId = existingContactId,
                HostId = existingEmployeeId,
                StartTime = DateTime.Now.TimeOfDay,
                EndTime = DateTime.Now.TimeOfDay.Add(TimeSpan.Parse("01:00:00")),
                IsCancelled = false,
                IsDone = true,
                Notes = "Sucess"
            };
        }


        [TestCleanup]
        public void Cleanup()
        {
            dbContext.Dispose();
            dbContext = null;
        }

        [TestMethod]
        [TestProperty("TestType", "Appointment")]
        public void Create_WithValidData_SavesRecordToDatabase()
        {
            // arrange

            // act 
            var newAppointment = sut.Create(appointment);



            // assert
            Assert.IsNotNull(newAppointment);
            Assert.IsTrue(newAppointment.AppointmentId != Guid.Empty);

            // Cleanup
            sut.Delete(newAppointment.AppointmentId);
        }



        [TestMethod]
        [TestProperty("TestType", "Appointment")]
        public void Delete_WithAnExistingAppointmentId_RemovesRecordFromDatabase()
        {
            //Arrange
            var newAppointment = sut.Create(appointment);

            //Act
            sut.Delete(newAppointment.AppointmentId);

            //Assert

            appointment = sut.Retrieve(newAppointment.AppointmentId);
            Assert.IsNull(appointment);
        }

        [TestMethod]
        [TestProperty("TestType", "Appointment")]
        public void Retrieve_WithAnExistingAppointmentId_ReturnsAppointmentRecordsFromDb()
        {
            //arrange
            var newAppointment = sut.Create(appointment);

            //act
            var retrieveAppointment = sut.Retrieve(newAppointment.AppointmentId);

            //assert
            Assert.IsNotNull(retrieveAppointment);

            //cleanup
            sut.Delete(newAppointment.AppointmentId);
        }

        [TestMethod]
        [TestProperty("TestType", "Appointment")]
        public void Update_WithAnExistingAppointmentId_ShouldUpdateAppointmentRecordsFromDb()
        {
            //Arrange
            var newAppointment = sut.Create(appointment);
            var expectedNotes = "Updated Note";

            //changing the value of properties
            newAppointment.Notes = expectedNotes;
            //Act
            sut.Update(newAppointment.AppointmentId, newAppointment);
            var updatedAppointment = sut.Retrieve(newAppointment.AppointmentId);

            //Assert   
            Assert.AreEqual(updatedAppointment.Notes, expectedNotes);

            //cleanup
            sut.Delete(updatedAppointment.AppointmentId);
        }
    }
}
