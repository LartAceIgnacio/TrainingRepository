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
        private Appointment appointment;
        private Mock<IAppointmentRepository> mockAppointmentRepository;
        private Contact contact;
        private Mock<IContactRepository> mockContactRepository;
        private Employee employee;
        private Mock<IEmployeeRepository> mockEmployeeRepository;

        private Guid existingEmployeeId = Guid.NewGuid();
        private Guid existingContactId = Guid.NewGuid();
        private Guid existingAppointmentId = Guid.NewGuid();
        private Guid nonExistingAppointmentId = Guid.Empty;
        private Guid nonExistingEmployeeId = Guid.Empty;
        private Guid nonExistingGuestId = Guid.Empty;

        AppointmentService sut;

        [TestInitialize]
        public void InitializeTest()
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

            employee = new Employee();
            contact = new Contact();

            mockAppointmentRepository = new Mock<IAppointmentRepository>();
            mockEmployeeRepository = new Mock<IEmployeeRepository>();
            mockContactRepository = new Mock<IContactRepository>();

            sut = new AppointmentService(
                mockAppointmentRepository.Object,
                mockEmployeeRepository.Object,
                mockContactRepository.Object);

            //appointment setup
            mockAppointmentRepository
                .Setup(c => c.Create(appointment))
                .Returns(appointment);
            //existingappointment setup
            mockAppointmentRepository
                .Setup(a => a.Retrieve(existingAppointmentId))
                .Returns(appointment);
            //nonexistingappointment setup
            mockAppointmentRepository
               .Setup(a => a.Retrieve(nonExistingAppointmentId))
                   .Returns<Appointment>(null);
            //employee setup
            mockEmployeeRepository
                .Setup(e => e.Retrieve(existingEmployeeId))
                .Returns(employee);
            //contact setup
            mockContactRepository
                .Setup(c => c.Retrieve(existingContactId))
                .Returns(contact);

        }

        [TestCleanup]
        public void CleanupTest()
        {

        }

        [TestMethod]
        public void Save_NewAppointmentWithValidData_ShouldCallRepositoryCreate()
        {
            //Arrange
            //Act
            sut.Save(appointment.appointmentId, appointment);

            //Assert
            mockAppointmentRepository.Verify(x => x.Retrieve(appointment.appointmentId), Times.Once);
            mockAppointmentRepository.Verify(x => x.Create(appointment), Times.Once);
        }

        [TestMethod]
        public void Save_WithValidData_ReturnsNewAppointmentWithNewAppointmentId()
        {
            //Arrange
            mockAppointmentRepository
                .Setup(c => c.Create(appointment))
                .Callback(() =>
                {
                    appointment.appointmentId = Guid.NewGuid();
                })
                .Returns(appointment);


            mockAppointmentRepository
                .Setup(c => c.Retrieve(existingAppointmentId))
                .Returns(appointment);

            //Act
            var newAppointment = sut.Save(appointment.appointmentId, appointment);
            //Assert
            Assert.IsTrue(newAppointment.appointmentId != Guid.Empty);
        }

        [TestMethod]
        public void Save_WithInvalidDate_ThrowsInvalidDateException()
        {
            //Assert
            appointment.AppointmentDate = DateTime.Today.AddMonths(-5);
            //Act

            //Arragnge
            Assert.ThrowsException<InvalidAppointmentDateException>(
                () => sut.Save(appointment.appointmentId, appointment));
            mockAppointmentRepository
                .Verify(a => a.Create(appointment), Times.Never);
        }

        [TestMethod]
        public void Save_WithInvalidStartTime_ThrowsInvalidStartTimeException()
        {
            //Arrange
            appointment.StartTime = DateTime.Now.TimeOfDay.Add(TimeSpan.Parse("2:00:00"));
            //Act

            //Assert
            Assert.ThrowsException<InvalidTimeException>(
                () => sut.Save(appointment.appointmentId, appointment));
            mockAppointmentRepository
                .Verify(a => a.Create(appointment), Times.Never);
        }

        [TestMethod]
        public void Save_WithInvalidEndTime_ThrowsInvalidEndTimeException()
        {
            //Arrange
            appointment.EndTime = DateTime.Now.TimeOfDay;
            appointment.StartTime = DateTime.Now.TimeOfDay.Add(TimeSpan.Parse("2:00:00"));
            //Act

            //Assert
            Assert.ThrowsException<InvalidTimeException>(
                () => sut.Save(appointment.appointmentId, appointment));
            mockAppointmentRepository
                .Verify(a => a.Create(appointment), Times.Never);
        }

        [TestMethod]
        public void Save_WithEqualStartTimeandEndTime_ThrowsInvalidEndTimeException()
        {
            //Arrange
            appointment.EndTime = DateTime.Now.TimeOfDay;
            appointment.StartTime = DateTime.Now.TimeOfDay;
            //Act

            //Assert
            Assert.ThrowsException<InvalidTimeException>(
                () => sut.Save(appointment.appointmentId, appointment));
            mockAppointmentRepository
                .Verify(a => a.Create(appointment), Times.Never);
        }

        [TestMethod]
        public void Save_WithInvalGuestId_ThrowsInvalidGuestIdException()
        {
            //Arrange
            appointment.GuestId = nonExistingGuestId;
            //Act

            //Assert
            Assert.ThrowsException<InvalidGuestIdException>(
                () => sut.Save(appointment.appointmentId, appointment));
            mockAppointmentRepository
                .Verify(a => a.Retrieve(It.IsAny<Guid>()), Times.Never);
        }

        [TestMethod]
        public void Save_WithInvalEmployeeId_ThrowsInvalidEmployeeException()
        {
            //Arrange
            appointment.HostId = nonExistingEmployeeId;
            //Act

            //Assert
            Assert.ThrowsException<InvalidEmployeeIdException>(
                () => sut.Save(appointment.appointmentId, appointment));
            mockAppointmentRepository
                .Verify(c => c.Retrieve(It.IsAny<Guid>()), Times.Never);
        }
    }
}
