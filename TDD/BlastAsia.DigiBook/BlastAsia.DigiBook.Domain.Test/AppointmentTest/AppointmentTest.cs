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
    public class AppointmentTest
    {
        private Appointment _appointment;
        private AppointmentService _sut;
        private Contact _contact;
        private Employee _employee;

        private Mock<IContactRepository> _mockContactRepo;
        private Mock<IEmployeeRepository> _mockEmployeeRepo;
        private Mock<IAppointmentRepository> _mockAppointmentRepo;
        private Mock<IDateTimeWrapper> _mockDateTimeWrapper;


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

            _mockEmployeeRepo = new Mock<IEmployeeRepository>();
            _mockContactRepo = new Mock<IContactRepository>();
            _mockAppointmentRepo = new Mock<IAppointmentRepository>();
            _mockDateTimeWrapper = new Mock<IDateTimeWrapper>();

            _mockDateTimeWrapper.Setup(d => d.GetNow())
                .Returns(_appointmentDate);

            _mockDateTimeWrapper.Setup(t => t.GetTime())
                .Returns(_timeStart);

            _mockDateTimeWrapper.Setup(t => t.GetTime())
                .Returns(_timeEnd);

            //// Arrange for host and guest
            _existingEmployeeId = Guid.NewGuid();
            _existingContactId = Guid.NewGuid();

            _employee = new Employee();
            _contact = new Contact();

            _appointment = new Appointment
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
            

            _sut = new AppointmentService(_mockContactRepo.Object, _mockEmployeeRepo.Object
                , _mockAppointmentRepo.Object, _mockDateTimeWrapper.Object);
            

            _mockEmployeeRepo.Setup(e => e.Retrieve(_appointment.HostId))
                //.Callback(() => _employee.Id = _appointment.HostId)
                .Returns(_employee);

            _mockContactRepo.Setup(c => c.Retrieve(_appointment.GuestId))
               //.Callback(() => _contact.ContactId = _appointment.GuestId)
               .Returns(_contact);

            _mockAppointmentRepo.Setup(a => a.Retrieve(_existingAppointmentId))
                .Returns(_appointment);

            _mockAppointmentRepo
                .Setup(x => x.Retrieve(_nonExistingAppointmentId))
                .Returns<Appointment>(null);

            
        }

        [TestCleanup]
        public void CleanUp() { }

        [TestMethod]
        public void Save_NewAppointmentWithValidData_ShouldCallRepositoryCreate()
        {
            //Act
            _sut.Save(_appointment);

            //Assert
            _mockAppointmentRepo.Verify(a => a.Retrieve(_appointment.AppointmentId), Times.Once);
            _mockAppointmentRepo.Verify(a => a.Create(_appointment), Times.Once);

        }
        

        [TestMethod]
        public void Save_WithNotExistingHostId_ThrowsHostRequiredException()
        {
            // Arrange
            _appointment.HostId = Guid.Empty;

            // Act

            // Assert
            Assert.ThrowsException<HostRequiredException>(
                () => _sut.Save(_appointment));
            _mockAppointmentRepo.Verify(a => a.Retrieve(_appointment.AppointmentId), Times.Once);
            _mockAppointmentRepo.Verify(a => a.Create(_appointment), Times.Never);
        }

        [TestMethod]
        public void Save_WithNonExistingGuestId_ThrowsGuestRequiredException()
        {
            // Arrange
            _appointment.GuestId = Guid.Empty;

            // Act

            // Assert
            Assert.ThrowsException<GuestRequiredException>(
                () => _sut.Save(_appointment));
            _mockAppointmentRepo.Verify(a => a.Retrieve(_appointment.AppointmentId), Times.Once);
            _mockAppointmentRepo.Verify(a => a.Create(_appointment), Times.Never);
        }

        [TestMethod]
        public void Save_EndTimeIsLessThanStartTime_ThrowsInvalidTimeScheduleException()
        {
            // Arrange
            _appointment.EndTime = _appointment.StartTime.Subtract(TimeSpan.Parse("2:00:00"));

            // Act
            // Assert
            Assert.ThrowsException<InvalidTimeScheduleException>(
                () => _sut.Save(_appointment));
            _mockAppointmentRepo.Verify(a => a.Retrieve(_appointment.AppointmentId), Times.Never);
            _mockAppointmentRepo.Verify(a => a.Create(_appointment), Times.Never);

        }

        [TestMethod]
        public void Save_EndTimeEqualToStartTime_ThrowsInvalidTimeScheduleException()
        {
            // Arrange
            _appointment.StartTime = _appointment.EndTime;

            // Act
            // Assert
            Assert.ThrowsException<InvalidTimeScheduleException>(
                () => _sut.Save(_appointment));
            _mockAppointmentRepo.Verify(a => a.Retrieve(_appointment.AppointmentId), Times.Never);
            _mockAppointmentRepo.Verify(a => a.Create(_appointment), Times.Never);
        }

        [TestMethod]
        public void Save_AppointmentDateLessThanCurrentDay_ThrowsInvalidScheduleTimeException()
        {
            _appointment.AppointmentDate = _appointmentDate.AddDays(-1);

            // Act
            // Assert
            Assert.ThrowsException<InvalidTimeScheduleException>(
                () => _sut.Save(_appointment));
            _mockAppointmentRepo.Verify(a => a.Retrieve(_appointment.AppointmentId), Times.Never);
            _mockAppointmentRepo.Verify(a => a.Create(_appointment), Times.Never);
        }
    }
}
