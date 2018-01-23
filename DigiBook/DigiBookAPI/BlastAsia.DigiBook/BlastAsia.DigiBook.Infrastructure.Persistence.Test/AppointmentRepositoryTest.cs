using BlastAsia.DigiBook.Domain.Models.Appointments;
using BlastAsia.DigiBook.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace BlastAsia.DigiBook.Infrastructure.Persistence.Test
{
    [TestClass]
    public class AppointmentRepositoryTest
    {
        private Appointment appointment = null;
        private DbContextOptions<DigiBookDbContext> dbOptions = null;
        private DigiBookDbContext dbContext = null;
        private String connectionString = null;
        private AppointmentRepository sut = null;
        public ContactRepository sutContact = null;
        public EmployeeRepository sutEmployee = null;
        private Guid exisitingHostId;
        private Guid existingGuestId;

        [TestInitialize]
        public void InitializeTest()
        {
            connectionString
               = @"Data Source=.;Database=DigiBookDb;Integrated Security=true;";

            dbOptions = new DbContextOptionsBuilder<DigiBookDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            dbContext = new DigiBookDbContext(dbOptions);
            dbContext.Database.EnsureCreated();

            sut = new AppointmentRepository(dbContext);
            sutEmployee = new EmployeeRepository(dbContext);
            sutContact = new ContactRepository(dbContext);

            //exisitingHostId = Guid.Parse("42874024-bd5b-4243-e7a2-08d55c87f4de");
            //existingGuestId = Guid.Parse("c02f60f3-ddd9-4fc3-3c5e-08d55c87d4c1");

            existingGuestId = sutContact.Retrieve().FirstOrDefault().ContactId;
            exisitingHostId = sutEmployee.Retrieve().FirstOrDefault().EmployeeId;

            appointment = new Appointment
            {
                GuestId = existingGuestId,
                HostId = exisitingHostId,
                AppointmentDate = DateTime.Today,
                StartTime = new DateTime().TimeOfDay,
                EndTime = new DateTime().TimeOfDay.Add(TimeSpan.Parse("01:00:00")),
                IsCancelled = false,
                IsDone = true,
                Notes = "6456"
            };

           
        }

        [TestCleanup]
        public void CleanupTest()
        {
            dbContext.Dispose();
            dbContext = null;
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Create_WithValidData_SavesRecordInTheDatabase()
        {
            // Act

            var newAppointment = sut.Create(appointment);

            // Assert

            Assert.IsNotNull(newAppointment);
            Assert.IsTrue(newAppointment.AppointmentId != Guid.Empty);

            // Cleanup

            sut.Delete(newAppointment.AppointmentId);
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

            Assert.IsNotNull(found);

            // Cleanup

            sut.Delete(found.AppointmentId);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Update_WithValidData_SavesUpdatesInDb()
        {
            // Arrange

            var expectedNotes = "Samp";
            appointment.Notes = expectedNotes;
            var newAppointment = sut.Create(appointment);

            // Act

            sut.Update(newAppointment.AppointmentId, newAppointment);

            // Assert

            var updatedAppointment = sut.Retrieve(newAppointment.AppointmentId);
            Assert.AreEqual(expectedNotes, updatedAppointment.Notes);

            // Cleanup

            sut.Delete(updatedAppointment.AppointmentId);

        }
    }
}