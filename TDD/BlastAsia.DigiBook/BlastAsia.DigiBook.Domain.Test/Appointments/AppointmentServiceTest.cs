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
        private Mock<IContactRepository> mockContactRepository;
        private Mock<IEmployeeRepository> mockEmployeeRepository;
        private AppointmentService sut;
        private Guid existingAppointmentId = Guid.NewGuid();
        private Guid existingGuesttId = Guid.NewGuid();
        private Guid existingHostId = Guid.NewGuid();
        private Guid nonExistingAppointmentId = Guid.Empty;
        private Contact contact;
        private Employee employee;
        private Appointment appointment;

        [TestInitialize]
        public void Initialize()
        {
            appointment = new Appointment
            {
                AppointmentDate = DateTime.Today,
                GuestId = existingGuesttId,
                HostId = existingHostId,
                StartTime = DateTime.Now.TimeOfDay,
                EndTime = DateTime.Now.TimeOfDay.Add(TimeSpan.Parse("01:00:00")),
                IsCancelled = false,
                IsDone = true,
                Notes = "Ongoing"
            };

            mockAppointmentRepository = new Mock<IAppointmentRepository>();
            mockEmployeeRepository = new Mock<IEmployeeRepository>();
            mockContactRepository = new Mock<IContactRepository>();

            contact = new Contact();
            employee = new Employee();

            mockAppointmentRepository
                .Setup(a => a.Retrieve(existingAppointmentId))
                .Returns(appointment);

            mockAppointmentRepository
                .Setup(a => a.Retrieve(nonExistingAppointmentId))
                .Returns<Appointment>(null);

            mockContactRepository
                .Setup(c => c.Retrieve(existingGuesttId))
                .Returns(contact);

            mockEmployeeRepository
               .Setup(e => e.Retrieve(existingHostId))
               .Returns(employee);

            sut = new AppointmentService(mockAppointmentRepository.Object, mockContactRepository.Object, mockEmployeeRepository.Object);
        }

        [TestCleanup]
        public void Cleanup()
        {

        }

        [TestMethod]
        public void Save_NewAppointmentWithValidData_ShouldCallRepositoryCreate()
        {
            //Act
            var result = sut.Save(appointment);

            //Assert
            mockAppointmentRepository
                .Verify(a => a.Retrieve(nonExistingAppointmentId), Times.Once);

            mockAppointmentRepository
                .Verify(a => a.Create(appointment), Times.Once);
        }

        [TestMethod]
        public void Save_WithExistingAppointment_ShouldCallRepositoryCreate()
        {
            //Arrange
            appointment.AppointmentId = existingAppointmentId;

            //Act
            sut.Save(appointment);

            //Assert
            mockAppointmentRepository
                .Verify(a => a.Retrieve(existingAppointmentId), Times.Once);
            mockAppointmentRepository
               .Verify(a => a.Update(existingAppointmentId, appointment), Times.Once);
        }

        [TestMethod]
        public void Save_AppointmentDateEarlierThanToday_ThrowsValidAppointmentDateRequiredException()
        {
            //Arrange
            appointment.AppointmentDate = DateTime.Today.AddDays(-1);

            //Assert
            Assert.ThrowsException<ValidAppointmentDateRequiredException>(
               () => sut.Save(appointment));
            mockAppointmentRepository
                .Verify(a => a.Create(appointment), Times.Never());
        }

        [TestMethod]
        public void Save_AppointmentStartTimeGreaterThanEndTime_ThrowsInclusiveStartAndEndTimeException()
        {
            //Arrange
            appointment.StartTime = DateTime.Now.TimeOfDay.Add(TimeSpan.Parse("10:00:00"));

            Assert.ThrowsException<InclusiveStartAndEndTimeException>(
              () => sut.Save(appointment));
            mockAppointmentRepository
                .Verify(a => a.Create(appointment), Times.Never());
        }

        [TestMethod]
        public void Save_AppointmentEndTimeAndStartTimeEqual_ThrowsInclusiveStartAndEndTimeException()
        {
            //Arrange
            appointment.EndTime = appointment.StartTime;

            //Assert
            Assert.ThrowsException<InclusiveStartAndEndTimeException>(
              () => sut.Save(appointment));
            mockAppointmentRepository
                .Verify(a => a.Create(appointment), Times.Never());
        }

        [TestMethod]
        public void Save_NotExistingGuestId_ThrowsGuestIdRequiredException()
        {
            //Arrange
            var nonExistingGuestId = Guid.Empty;
            appointment.GuestId = nonExistingGuestId;

            mockContactRepository
                .Setup(c => c.Retrieve(appointment.GuestId))
                .Returns<Contact>(null);

            //Assert
            Assert.ThrowsException<GuestIdRequiredException>(
                 () => sut.Save(appointment));

            mockAppointmentRepository
                .Verify(a => a.Create(appointment), Times.Never());
        }

        [TestMethod]
        public void Save_NotExistingHostId_ThrowsHostIdRequiredException()
        {
            //Arrange
            appointment.HostId = Guid.Empty;

            mockEmployeeRepository
                .Setup(e => e.Retrieve(appointment.HostId))
                .Returns<Employee>(null);

            //Assert
            Assert.ThrowsException<HostIdRequiredException>(
                 () => sut.Save(appointment));

            mockAppointmentRepository
                .Verify(a => a.Create(appointment), Times.Never());
        }
    }
}
