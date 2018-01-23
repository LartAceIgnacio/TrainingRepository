
using BlastAsia.DigiBook.Domain.Appointments;
using BlastAsia.DigiBook.Domain.Appointments.Exceptions;
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
        private Employee employee;
        private Contact contact;

        private Mock<IAppointmentRepository> mockAppointmentRepository;
        private Mock<IEmployeeRepository> mockEmployeeRepository;
        private Mock<IContactRepository> mockContactRepository;

        private AppointmentService sut;
            
        private Guid existingAppointmentId = Guid.NewGuid();
        private Guid nonExistingAppointmentId = Guid.Empty;

        private Guid existingContactId = Guid.NewGuid();
        private Guid nonExistingContactId = Guid.Empty;

        private Guid existingEmployeeId = Guid.NewGuid();
        private Guid nonExistingEmployeeId = Guid.Empty;


        [TestInitialize]
        public void Initialize()
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
                Notes = "Notes"
            };
            employee = new Employee();
            contact = new Contact();

            mockAppointmentRepository = new Mock<IAppointmentRepository>();
            mockEmployeeRepository = new Mock<IEmployeeRepository>();
            mockContactRepository = new Mock<IContactRepository>();

            sut = new AppointmentService(mockAppointmentRepository.Object, mockEmployeeRepository.Object,
                mockContactRepository.Object);

            //mockAppointmentRepository
            // .Setup(a => a.Create(appointment))
            // .Callback(() => appointment.AppointmentId = Guid.NewGuid())
            // .Returns(appointment);

            mockEmployeeRepository
               .Setup(e => e.Retrieve(appointment.HostId))
               .Returns(employee);

            mockContactRepository
                .Setup(g => g.Retrieve(appointment.GuestId))
                .Returns(contact);

            mockAppointmentRepository
                .Setup(a => a.Retrieve(existingAppointmentId))
                .Returns(appointment);
            

            mockAppointmentRepository
                .Setup(a => a.Retrieve(nonExistingAppointmentId))
                .Returns<Appointment>(null);
        }

        [TestMethod]
        public void Save_NewAppointmentWithValidData_ShouldCallRepositoryCreate()
        {
            //Act
            sut.Save(appointment.AppointmentId, appointment);

            //Assert
            mockAppointmentRepository
              .Setup(a => a.Create(appointment))
              .Callback(() => appointment.AppointmentId = Guid.NewGuid())
              .Returns(appointment);

            mockAppointmentRepository
                .Verify(a => a.Retrieve(appointment.AppointmentId), Times.Once);

            mockAppointmentRepository
                .Verify(a => a.Create(appointment), Times.Once);
        }
        [TestMethod]
        public void Save_AppointmentWithGuestId_ThrowsGuestIdRequiredException()
        {
            //Arrange
            appointment.GuestId = Guid.Empty;

            //Assert
            Assert.ThrowsException<ContactIdRequiredException>
                (() => sut.Save(appointment.AppointmentId, appointment));
            mockAppointmentRepository
                .Verify(a => a.Retrieve(It.IsAny<Guid>()), Times.Never);
        }
        [TestMethod]
        public void Save_AppointmentWithHostId_ThrowsHostIdRequiredException()
        {
            //Arrange
            appointment.HostId = Guid.Empty;

            //Assert
            Assert.ThrowsException<EmployeeIdRequiredException>(
                () => sut.Save(appointment.AppointmentId, appointment));

            mockAppointmentRepository
                .Verify(a => a.Retrieve(It.IsAny<Guid>()), Times.Never);
        }
        [TestMethod]
        public void Save_AppointmentWithValidData_ShouldCallRepositoryUpdate()
        {
            //Arrange
            appointment.HostId = existingEmployeeId;
            appointment.GuestId = existingContactId;
            appointment.AppointmentId = existingAppointmentId;

            //Act
            sut.Save(appointment.AppointmentId, appointment);

            //Assert
            mockAppointmentRepository
                .Verify(a => a.Retrieve(existingAppointmentId), Times.Once);
            mockAppointmentRepository
                .Verify(a => a.Update(existingAppointmentId, appointment), Times.Once);
        }

        [TestMethod]
        public void Save_WithAppointmentDateLessThanDateToday_ThrowsAppointmentDateLessThanDateTodayException()
        {
            //Arrange
            appointment.AppointmentDate = DateTime.Today.AddMonths(-5);

            //Act

            //Assert
            Assert.ThrowsException<AppointmentDateLessThanDateTodayException>(
                () => sut.Save(appointment.AppointmentId, appointment));
            mockAppointmentRepository
                .Verify(a => a.Retrieve(It.IsAny<Guid>()), Times.Never);
        }
        [TestMethod]
        public void Save_WithAppointmentStartDateGreatherThanEndTime_ThrowsAppointmentDateException()
        {
            //Arrange         
            appointment.StartTime = DateTime.Now.TimeOfDay.Add(TimeSpan.Parse("02:00:00"));
            appointment.EndTime = DateTime.Now.TimeOfDay;

            //Assert
            Assert.ThrowsException<AppointmentDateException>(
                () => sut.Save(appointment.AppointmentId, appointment));
            mockAppointmentRepository
                .Verify(a => a.Retrieve(It.IsAny<Guid>()), Times.Never);
        }
        [TestMethod]
        public void Save_WithAppointmentEndDateLessThanStartTime_ThrowsAppointmentDateException()
        {
            //Arrange
            appointment.StartTime = DateTime.Now.TimeOfDay;
            appointment.EndTime = DateTime.Now.TimeOfDay.Subtract(TimeSpan.Parse("2:00:00"));
           

            //Assert
            Assert.ThrowsException<AppointmentDateException>(
                () => sut.Save(appointment.AppointmentId, appointment));
            mockAppointmentRepository
                .Verify(a => a.Retrieve(It.IsAny<Guid>()), Times.Never);
        }

    }
}
