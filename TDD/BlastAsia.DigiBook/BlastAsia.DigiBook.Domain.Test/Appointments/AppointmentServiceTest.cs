using BlastAsia.DigiBook.Domain.Appointments;
using BlastAsia.DigiBook.Domain.Contacts;
using BlastAsia.DigiBook.Domain.Employees;
using BlastAsia.DigiBook.Domain.Models.Appointments;
using BlastAsia.DigiBook.Domain.Models.Contacts;
using BlastAsia.DigiBook.Domain.Models.Employees;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Test.Appointments
{
    [TestClass]
    public class AppointmentServiceTest
    {
        private Mock<IAppointmentRepository> mockAppointmentRepository;
        private Mock<IEmployeeRepository> mockEmployeeRepository;
        private Mock<IContactRepository> mockContactRepository;
        private Appointment appointment;
        private Employee employee = new Employee();
        private Contact contact = new Contact();
        private AppointmentService sut;
        private Guid existingEmployeeId = Guid.NewGuid();
        private Guid existingContactId = Guid.NewGuid();
        private Guid existingAppointmentId = Guid.NewGuid();
        private Guid nonExistingHostId;
        private readonly Guid nonExistingGuestId;

        [TestInitialize]
        public void InitializeTest()
        {
            appointment = new Appointment
            {
                AppointmentId = new Guid(),
                AppointmentDate = DateTime.Today,
                GuestId = existingContactId,
                HostId = existingEmployeeId,
                StartTime = new DateTime().TimeOfDay,
                EndTime = new DateTime().TimeOfDay.Add(TimeSpan.Parse("01:00:00")),
                IsCancelled = false,
                IsDone = true,
                Notes = ""

            };

            mockAppointmentRepository = new Mock<IAppointmentRepository>();
            mockEmployeeRepository = new Mock<IEmployeeRepository>();
            mockContactRepository = new Mock<IContactRepository>();
            sut = new AppointmentService(mockAppointmentRepository.Object, mockEmployeeRepository.Object, mockContactRepository.Object);

            mockEmployeeRepository
                .Setup(e => e.Retrieve(appointment.HostId))
                .Returns(employee);
            mockContactRepository
                .Setup(c => c.Retrieve(appointment.GuestId))
                .Returns(contact);
            mockAppointmentRepository
               .Setup(a => a.Retrieve(existingAppointmentId))
               .Returns(appointment);
            mockAppointmentRepository
                .Setup(a => a.Retrieve(appointment.AppointmentId))
                .Returns<Appointment>(null);
        }

        [TestCleanup]
        public void CleanupTest()
        {

        }

        [TestMethod]
        public void Save_NewAppointmentWithValidData_ShouldCallRepositoryCreate()
        {
            //Arrange
            employee.EmployeeId = existingEmployeeId;
            contact.ContactId = existingContactId;

            //Act
            sut.Save(employee.EmployeeId, contact.ContactId, appointment);

            //Assert
            mockAppointmentRepository
                .Verify(a => a.Retrieve(appointment.AppointmentId), Times.Once);
            mockAppointmentRepository
                .Verify(a => a.Create(employee.EmployeeId, contact.ContactId, appointment), Times.Once);
        }

        [TestMethod]
        public void Save_WithExistingAppointment_ShouldCallRepositoryUpdate()
        {
            // Arrange 
            appointment.AppointmentId = existingAppointmentId;
            // Act
            sut.Save(employee.EmployeeId, contact.ContactId, appointment);

            // Assert
            mockAppointmentRepository
                .Verify(a => a.Retrieve(appointment.AppointmentId), Times.Once);
            mockAppointmentRepository
                .Verify(a => a.Update(appointment.AppointmentId, appointment), Times.Once);
        }

        [TestMethod]
        public void Create_AppointmentWithValidData_ShouldReturnNewAppointmentWithAppointmentId()
        {
            // Arrange 
            appointment.HostId = existingEmployeeId;
            appointment.GuestId = existingContactId;
            mockAppointmentRepository
                .Setup(a => a.Create(employee.EmployeeId, contact.ContactId, appointment))
                .Callback(() =>
                {
                    appointment.AppointmentId = Guid.NewGuid();
                    employee.EmployeeId = Guid.NewGuid();
                    contact.ContactId = Guid.NewGuid();
                })
                .Returns(appointment);

            // Act 
            var result = sut.Save(employee.EmployeeId, contact.ContactId, appointment);

            // Assert
            Assert.IsTrue(result.AppointmentId != Guid.Empty);
        }

        [TestMethod]
        public void Save_AppointmentWithNonExistingHostId_ShouldThrowInvalidHostIdException()
        {
            appointment.HostId = nonExistingHostId;

            Assert.ThrowsException<InvalidHostIdException>(
                    () => sut.Save(employee.EmployeeId, contact.ContactId, appointment));
        }

        [TestMethod]
        public void Save_AppointmentWithNonExistingGuestId_ShouldThrowInvalidGuestIdException()
        {
            appointment.GuestId = nonExistingGuestId;

            Assert.ThrowsException<InvalidGuestIdException>(
                    () => sut.Save(employee.EmployeeId, contact.ContactId, appointment));
        }
        [TestMethod]
        public void Create_EndTimeLessThanStartTime_ThrowsInclusiveTimeRequiredException()
        {
            // Arrange
            appointment.StartTime = new DateTime().TimeOfDay.Add(TimeSpan.Parse("01:00:00"));
            appointment.EndTime = new DateTime().TimeOfDay;

            // Assert 
            mockAppointmentRepository
                .Verify(a => a.Create(employee.EmployeeId, contact.ContactId, appointment), Times.Never);
            Assert.ThrowsException<InclusiveTimeRequiredException>(
                () => sut.Save(employee.EmployeeId, contact.ContactId, appointment));
        }

        [TestMethod]
        public void Create_AppointmentWithAppointmentDateLessThanToday_ThrowsAppointmentDateInvalidException()
        {
            // Arrange
            appointment.AppointmentDate = new DateTime(2017, 09, 18);

            // Assert
            mockAppointmentRepository
                .Verify(a => a.Create(employee.EmployeeId, contact.ContactId,appointment), Times.Never);
            Assert.ThrowsException<AppointmentDateRequiredException>(
                () => sut.Save(employee.EmployeeId, contact.ContactId, appointment));
            
        }

    }
    

}
