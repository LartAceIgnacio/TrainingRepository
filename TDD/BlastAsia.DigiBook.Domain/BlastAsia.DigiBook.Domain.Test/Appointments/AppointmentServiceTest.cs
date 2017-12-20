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
        private AppointmentService sut;
        private Mock<IAppointmentRepository> mockAppointmentRepository;
        private Mock<IEmployeeRepository> mockEmployeeRepository;
        private Mock<IContactRepository> mockContactRepository;

        private Guid existingAppointmentId = Guid.NewGuid();
        private Guid nonExistingAppointmentId = Guid.Empty;
        private Guid existingContactId = Guid.NewGuid();
        private Guid existingEmployeeId = Guid.NewGuid();

        [TestInitialize]
        public void Initialize()
        {
            appointment = new Appointment {
                AppointmentDate = DateTime.Today,
                GuestId = existingContactId,
                HostId = existingEmployeeId,
                StartTime = DateTime.Now.TimeOfDay,
                EndTime = DateTime.Now.TimeOfDay.Add(TimeSpan.Parse("01:00:00")),
                IsCancelled = false,
                IsDone = true,
                Notes = "Sucess"
            };
			
			contact = new Contact();
			employee = new Employee();

            mockAppointmentRepository = new Mock<IAppointmentRepository>();
            mockEmployeeRepository = new Mock<IEmployeeRepository>();
            mockContactRepository = new Mock<IContactRepository>();

            mockEmployeeRepository
                .Setup(x => x.Retrieve(appointment.HostId))
                .Returns(employee);

            mockContactRepository
                .Setup(x => x.Retrieve(appointment.GuestId))
                .Returns(contact);

            mockAppointmentRepository
                .Setup(x => x.Retrieve(existingAppointmentId))
                .Returns(appointment);

            mockAppointmentRepository
                .Setup(x => x.Retrieve(nonExistingAppointmentId))
                .Returns<Appointment>(null);

            sut = new AppointmentService(mockAppointmentRepository.Object, mockEmployeeRepository.Object, mockContactRepository.Object);
        }

        [TestCleanup]
        public void Cleanup()
        {

        }

        [TestMethod]
        public void Save_WithValidData_ReturnsNewAppointmentWithAppointmentId()
        {
            //Arrange
            mockAppointmentRepository
                .Setup(x => x.Create(It.IsAny<Appointment>()))
                .Callback(() => appointment.AppointmentId = Guid.NewGuid())
                .Returns(appointment);

            //Act
            var newAppointment = sut.Save(appointment.AppointmentId, appointment);

            //Assert
            Assert.IsTrue(newAppointment.AppointmentId != Guid.Empty);
        }

        [TestMethod]
        public void Save_NewAppointmentWithValidData_ShouldCallRepositoryCreate()
        {
            //Arrange
            appointment.HostId = existingEmployeeId;
            appointment.GuestId = existingContactId;

            //Act
            sut.Save(appointment.AppointmentId, appointment);

            //Assert
            //mockAppointmentRepository.Verify(x => x.Retrieve(appointment.AppointmentId), Times.Once);
            mockAppointmentRepository.Verify(x => x.Create(appointment), Times.Once);
        }

        [TestMethod]
        public void Save_WithValidData_ShouldCallRepositoryUpdate()
        {
            //Arrange
            appointment.HostId = existingEmployeeId;
            appointment.GuestId = existingContactId;
            appointment.AppointmentId = existingAppointmentId;

            //Act
            sut.Save(appointment.AppointmentId, appointment);

            //Assert
            //mockAppointmentRepository.Verify(x => x.Retrieve(appointment.AppointmentId), Times.Once);
            mockAppointmentRepository.Verify(x => x.Update(appointment.AppointmentId, appointment), Times.Once);
        }

        [TestMethod]
        public void Save_WithGuestIdNotExisted_ThrowsContactIdNotExistedException()
        {
            //Arrange
            appointment.HostId = existingEmployeeId;
            appointment.GuestId = Guid.Empty;
            appointment.AppointmentId = existingAppointmentId;

            //Act

            //Assert
            Assert.ThrowsException<ContactIdNotExistedException>(() => sut.Save(appointment.AppointmentId, appointment));
            mockAppointmentRepository.Verify(x => x.Retrieve(It.IsAny<Guid>()), Times.Never);
        }

        [TestMethod]
        public void Save_WithHostIdNotExisted_ThrowsEmployeeIdNotExistedException()
        {
            //Arrange
            appointment.HostId = Guid.Empty;
            appointment.GuestId = existingContactId;
            appointment.AppointmentId = existingAppointmentId;

            //Act

            //Assert
            Assert.ThrowsException<EmployeeIdNotExistedException>(() => sut.Save(appointment.AppointmentId, appointment));
            mockAppointmentRepository.Verify(x => x.Retrieve(It.IsAny<Guid>()), Times.Never);
        }

        [TestMethod]
        public void Save_WithAppointmentDateLessThanDateToday_ThrowsAppointmentDateLessThanDateTodayException()
        {
            //Arrange
            appointment.AppointmentDate = DateTime.Today.AddMonths(-5);

            //Act


            //Assert
            Assert.ThrowsException<AppointmentDateLessThanDateTodayException>(() => sut.Save(appointment.AppointmentId, appointment));
            mockAppointmentRepository.Verify(x => x.Retrieve(It.IsAny<Guid>()), Times.Never);
        }

        [TestMethod]
        public void Save_WithStartTimeGreaterThanEndTime_ThrowsTimeInclusiveException()
        {
            //Arrange
            appointment.EndTime = DateTime.Now.TimeOfDay;
            appointment.StartTime = DateTime.Now.TimeOfDay.Add(TimeSpan.Parse("01:00:00"));

            //Act


            //Assert
            Assert.ThrowsException<TimeInclusiveException>(() => sut.Save(appointment.AppointmentId, appointment));
            mockAppointmentRepository.Verify(x => x.Retrieve(It.IsAny<Guid>()), Times.Never);
        }

        [TestMethod]
        public void Save_WithEndTimeLessThanOrEqualStartTime_ThrowsTimeInclusiveException()
        {
            //Arrange
            appointment.EndTime = DateTime.Now.TimeOfDay.Subtract(TimeSpan.Parse("01:00:00"));
            appointment.StartTime = DateTime.Now.TimeOfDay;

            //Act


            //Assert
            Assert.ThrowsException<TimeInclusiveException>(() => sut.Save(appointment.AppointmentId, appointment));
            mockAppointmentRepository.Verify(x => x.Retrieve(It.IsAny<Guid>()), Times.Never);
        }
    }
}
