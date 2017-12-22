using BlastAsia.DigiBook.Domain.Appointments.Adapters;
using BlastAsia.DigiBook.Domain.Appointments.AppointmentExceptions;
using BlastAsia.DigiBook.Domain.Appointments.Services;
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

namespace BlastAsia.DigiBook.Domain.Test.AppointmentTest
{
    [TestClass]
    public class AppointmentServiceTest
    {
        private Appointment appointment;
        private AppointmentService sut;
        private Contact contact;
        private Employee employee;

        private Mock<IContactRepository> mockContactRepo;
        private Mock<IEmployeeRepository> mockEmployeeRepo;
        private Mock<IAppointmentRepository> mockAppointmentRepo;
        private Mock<IDateTimeWrapper> mockDateTimeWrapper;


        private Guid _appointmentId;
        private DateTime _appointmentDate;
        private Guid _existingAppointmentId;
        private Guid _nonExistingAppointmentId;
        private Guid _existingContactId;
        private Guid _existingEmployeeId;
        private TimeSpan _timeStart;
        private TimeSpan _timeEnd;

        [TestInitialize]
        public void Initialize()
        {
            _appointmentId = Guid.NewGuid();
            _appointmentDate = DateTime.Now;
            _timeStart = DateTime.Now.TimeOfDay;
            _timeEnd = DateTime.Now.TimeOfDay.Add(TimeSpan.Parse("2:0:0"));

            mockEmployeeRepo = new Mock<IEmployeeRepository>();
            mockContactRepo = new Mock<IContactRepository>();
            mockAppointmentRepo = new Mock<IAppointmentRepository>();
            mockDateTimeWrapper = new Mock<IDateTimeWrapper>();

            mockDateTimeWrapper.Setup(d => d.GetNow())
                .Returns(_appointmentDate);

            mockDateTimeWrapper.Setup(t => t.GetTime())
                .Returns(_timeStart);

            mockDateTimeWrapper.Setup(t => t.GetTime())
                .Returns(_timeEnd);

            //// Arrange for host and guest
            _existingEmployeeId = Guid.NewGuid();
            _existingContactId = Guid.NewGuid();

            employee = new Employee();
            contact = new Contact();

            appointment = new Appointment
            {
                AppointmentId = _appointmentId,
                AppointmentDate = _appointmentDate,
                GuestId = _existingContactId,
                HostId = _existingEmployeeId,
                StartTime = _timeStart,
                EndTime = _timeEnd,
                IsCancelled = false,
                IsDone = false,
                Notes = ""
            };

            _nonExistingAppointmentId = Guid.Empty;
            _existingAppointmentId = Guid.NewGuid();
            

            sut = new AppointmentService(mockContactRepo.Object, mockEmployeeRepo.Object
                , mockAppointmentRepo.Object, mockDateTimeWrapper.Object);
            

            mockEmployeeRepo.Setup(e => e.Retrieve(appointment.HostId))
                //.Callback(() => _employee.Id = _appointment.HostId)
                .Returns(employee);

            mockContactRepo.Setup(c => c.Retrieve(appointment.GuestId))
               //.Callback(() => _contact.ContactId = _appointment.GuestId)
               .Returns(contact);

            mockAppointmentRepo.Setup(a => a.Retrieve(_existingAppointmentId))
                .Returns(appointment);

            mockAppointmentRepo
                .Setup(x => x.Retrieve(_nonExistingAppointmentId))
                .Returns<Appointment>(null);
            
        }

        [TestCleanup]
        public void CleanUp() { }

        [TestMethod]
        public void Save_NewAppointmentWithValidData_ShouldCallRepositoryCreate()
        {
            //Act
            sut.Save(Guid.Empty, appointment);

            //Assert
            mockAppointmentRepo.Verify(a => a.Retrieve(appointment.AppointmentId), Times.Once);
            mockAppointmentRepo.Verify(a => a.Create(appointment), Times.Once);

        }

        [TestMethod]
        public void Save_WithExistingAppointment_CallRepositoryUpdate()
        {
            // Arrange
            appointment.AppointmentId= _existingAppointmentId;

            // Act
            sut.Save(appointment.AppointmentId, appointment);


            // Assert
            mockAppointmentRepo.Verify(a => a.Retrieve(appointment.AppointmentId), Times.AtLeastOnce);// since calling a retrieve to check if existing object and retrieve first before update
            mockAppointmentRepo.Verify(c => c.Update(_existingAppointmentId, appointment), Times.Once);
        }

        [TestMethod]
        public void Save_WithNotExistingHostId_ThrowsHostRequiredException()
        {
            // Arrange
            appointment.HostId = Guid.Empty;

            // Act

            // Assert
            Assert.ThrowsException<HostRequiredException>(
                () => sut.Save(appointment.AppointmentId, appointment));
            mockAppointmentRepo.Verify(a => a.Retrieve(appointment.AppointmentId), Times.Once);
            mockAppointmentRepo.Verify(a => a.Create(appointment), Times.Never);
        }

        [TestMethod]
        public void Save_WithNonExistingGuestId_ThrowsGuestRequiredException()
        {
            // Arrange
            appointment.GuestId = Guid.Empty;

            // Act
            // Assert
            Assert.ThrowsException<GuestRequiredException>(
                () => sut.Save(appointment.AppointmentId, appointment));
            mockAppointmentRepo.Verify(a => a.Retrieve(appointment.AppointmentId), Times.Once);
            mockAppointmentRepo.Verify(a => a.Create(appointment), Times.Never);
        }

        [TestMethod]
        public void Save_EndTimeIsLessThanStartTime_ThrowsInvalidTimeScheduleException()
        {
            // Arrange
            appointment.EndTime = appointment.StartTime.Subtract(TimeSpan.Parse("2:00:00"));

            // Act
            // Assert
            Assert.ThrowsException<InvalidTimeScheduleException>(
                () => sut.Save(appointment.AppointmentId, appointment));
            mockAppointmentRepo.Verify(a => a.Retrieve(appointment.AppointmentId), Times.Never);
            mockAppointmentRepo.Verify(a => a.Create(appointment), Times.Never);

        }

        [TestMethod]
        public void Save_EndTimeEqualToStartTime_ThrowsInvalidTimeScheduleException()
        {
            // Arrange
            appointment.StartTime = appointment.EndTime;

            // Act
            // Assert
            Assert.ThrowsException<InvalidTimeScheduleException>(
                () => sut.Save(appointment.AppointmentId, appointment));
            mockAppointmentRepo.Verify(a => a.Retrieve(appointment.AppointmentId), Times.Never);
            mockAppointmentRepo.Verify(a => a.Create(appointment), Times.Never);
        }

        [TestMethod]
        public void Save_AppointmentDateLessThanCurrentDay_ThrowsInvalidScheduleTimeException()
        {
            appointment.AppointmentDate = _appointmentDate.AddDays(-1);

            // Act
            // Assert
            Assert.ThrowsException<InvalidTimeScheduleException>(
                () => sut.Save(appointment.AppointmentId, appointment));
            mockAppointmentRepo.Verify(a => a.Retrieve(appointment.AppointmentId), Times.Never);
            mockAppointmentRepo.Verify(a => a.Create(appointment), Times.Never);
        }

        

    }
}
