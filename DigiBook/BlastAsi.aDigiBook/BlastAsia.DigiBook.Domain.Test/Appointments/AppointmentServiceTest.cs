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
        private Contact contact;
        private Employee employee;
        private Mock<IAppointmentRepository> mockAppointmentRepository;
        private Mock<IContactRepository> mockContactRepository;
        private Mock<IEmployeeRepository> mockEmployeeRepository;
        private AppointmentService sut;
        private Guid nonExistingAppointmentId = Guid.Empty,
                     existingAppointmentId = Guid.NewGuid(),
                     existingGuestId = Guid.NewGuid(),
                     existingHostId = Guid.NewGuid();

        [TestInitialize]
        public void Initialize()
        {
            appointment = new Appointment
            {
                GuestId = existingGuestId,
                HostId = existingHostId,
                AppointmentDate = DateTime.Now,
                StartTime = DateTime.Now.TimeOfDay,
                EndTime = DateTime.Now.TimeOfDay.Add(TimeSpan.Parse("02:00:00")),
                IsCancelled = false,
                IsDone = false,
                Notes = "Bring coffee."
            };

            contact = new Contact();
            employee = new Employee();

            mockAppointmentRepository = new Mock<IAppointmentRepository>();
            mockContactRepository = new Mock<IContactRepository>();
            mockEmployeeRepository = new Mock<IEmployeeRepository>();

            sut = new AppointmentService(mockAppointmentRepository.Object, mockContactRepository.Object, mockEmployeeRepository.Object);

            mockContactRepository
                .Setup(c => c.Retrieve(existingGuestId))
                .Returns(contact);

            mockEmployeeRepository
                .Setup(e => e.Retrieve(existingHostId))
                .Returns(employee);
        }

        [TestCleanup]
        public void CleanUp()
        {

        }

        [TestMethod]
        public void Save_NewAppointmentWithValidAppointmentData_ShouldCallRepositoryCreate()
        {
            //Arrange
            mockAppointmentRepository
                .Setup(a => a.Retrieve(nonExistingAppointmentId))
                .Returns<Appointment>(null);

            appointment.AppointmentId = nonExistingAppointmentId;

            //Act
            sut.Save(appointment.AppointmentId,appointment);

            //Assert
            mockAppointmentRepository
                .Verify(a => a.Retrieve(nonExistingAppointmentId), Times.Once());

            mockAppointmentRepository
                .Verify(a => a.Create(appointment), Times.Once());
        }

        [TestMethod]
        public void Save_AppointmentWithExistingData_ShouldCallRepositoryUpdate()
        {
            //Arrange
            mockAppointmentRepository
                .Setup(a => a.Retrieve(existingAppointmentId))
                .Returns(appointment);

            appointment.AppointmentId = existingAppointmentId;

            //Act
            sut.Save(appointment.AppointmentId, appointment);

            //Assert
            mockAppointmentRepository
                .Verify(a => a.Retrieve(existingAppointmentId), Times.Once());

            mockAppointmentRepository
                .Verify(a => a.Update(existingAppointmentId, appointment), Times.Once());
        }

        [TestMethod]
        public void Save_GuestIdDoesNotExist_ThrowsGuestIdDoesNotExistException()
        {
            //Arrange
            var nonExistingGuestId = Guid.Empty;

            mockContactRepository
                .Setup(c => c.Retrieve(nonExistingGuestId))
                .Returns<Contact>(null);

            appointment.GuestId = nonExistingGuestId;

            //Act

            //Assert
            Assert.ThrowsException<GuestIdDoesNotExistException>(
                 () => sut.Save(appointment.AppointmentId, appointment));

            mockAppointmentRepository
                .Verify(a => a.Create(appointment), Times.Never());
        }

        [TestMethod]
        public void Save_HostIdDoesNotExist_ThrowsHostIdDoesNotExistException()
        {
            //Arrange
            var nonExistingHostId = Guid.Empty;

            mockEmployeeRepository
                .Setup(c => c.Retrieve(nonExistingHostId))
                .Returns<Employee>(null);

            appointment.HostId = nonExistingHostId;

            //Act

            //Assert
            Assert.ThrowsException<HostIdDoesNotExistException>(
                () => sut.Save(appointment.AppointmentId, appointment));

            mockAppointmentRepository
                .Verify(a => a.Create(appointment), Times.Never());
        }

        [TestMethod]
        public void Save_AppointmentDateLessThanCurrentDate_ThrowsValidAppointmentDateRequiredException()
        {
            //Arrange
            appointment.AppointmentDate = DateTime.Today.AddDays(-1);

            //Act

            //Assert
            Assert.ThrowsException<ValidAppointmentDateRequiredException>(
                () => sut.Save(appointment.AppointmentId, appointment));

            mockAppointmentRepository
                .Verify(a => a.Create(appointment), Times.Never());
        }

        [TestMethod]
        public void Save_AppointmentStartTimeGreaterThanEndTime_ThrowsInclusiveStartTimeEndTimeRequiredException()
        {
            //Arrange
            appointment.StartTime = DateTime.Now.TimeOfDay.Add(TimeSpan.Parse("05:00:00"));

            //Act

            //Assert
            Assert.ThrowsException<InclusiveStartTimeEndTimeRequiredException>(
                () => sut.Save(appointment.AppointmentId, appointment));

            mockAppointmentRepository
                .Verify(a => a.Create(appointment), Times.Never());
        }

        [TestMethod]
        public void Save_AppointmentEndTimeLessThanStartTime_ThrowsInclusiveStartTimeEndTimeRequiredException()
        {
            //Arrange
            appointment.EndTime = DateTime.Now.TimeOfDay.Subtract(TimeSpan.Parse("10:00:00"));

            //Act

            //Assert
            Assert.ThrowsException<InclusiveStartTimeEndTimeRequiredException>(
                () => sut.Save(appointment.AppointmentId, appointment));

            mockAppointmentRepository
                .Verify(a => a.Create(appointment), Times.Never());
        }

        [TestMethod]
        public void Save_AppointmentStartTimeAndEndTimeIsEqual_ThrowsInclusiveStartTimeEndTimeRequiredException()
        {
            //Arrange
            appointment.EndTime = appointment.StartTime;

            //Act

            //Assert
            Assert.ThrowsException<InclusiveStartTimeEndTimeRequiredException>(
                () => sut.Save(appointment.AppointmentId, appointment));

            mockAppointmentRepository
                .Verify(a => a.Create(appointment), Times.Never());
        }
    }
}
