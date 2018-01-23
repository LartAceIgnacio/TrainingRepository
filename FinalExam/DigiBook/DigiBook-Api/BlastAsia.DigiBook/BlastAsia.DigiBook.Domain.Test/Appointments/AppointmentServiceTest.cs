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
        private AppointmentService sut;
        private Appointment appointment;
        private Employee employee = new Employee();
        private Contact contact = new Contact();
        private Guid existingAppointId;
        private Guid nonExistingAppointmentId;
        private Guid HostId = Guid.NewGuid();
        private Guid GuestId = Guid.NewGuid();


        [TestInitialize]
        public void AppointmentInitialize()
        {
            appointment = new Appointment
            {
                AppointmentDate = DateTime.Today,
                StartTime = DateTime.Now.TimeOfDay,
                GuestId = GuestId,
                HostId = HostId,
                EndTime = DateTime.Now.TimeOfDay.Add(TimeSpan.Parse("1:00:00")),
                IsCancelled = false,
                IsDone = true,
                Notes = ""

            };

            existingAppointId = Guid.NewGuid();
            nonExistingAppointmentId = Guid.Empty;

            mockEmployeeRepository = new Mock<IEmployeeRepository>();
            mockContactRepository = new Mock<IContactRepository>();
            mockAppointmentRepository = new Mock<IAppointmentRepository>();

            mockEmployeeRepository
                .Setup(e => e.Retrieve(appointment.HostId))
                .Returns(employee);

            mockContactRepository
                .Setup(c => c.Retrieve(appointment.GuestId))
                .Returns(contact);

            sut = new AppointmentService(mockAppointmentRepository.Object
                , mockEmployeeRepository.Object
                , mockContactRepository.Object);
        }

        [TestMethod]
        public void Save_NewAppointmentWithValidData_ShouldCallRepositoryRetriveAndCreate()
        {
            //Arrange

            mockAppointmentRepository
               .Setup(e => e.Retrieve(nonExistingAppointmentId))
               .Returns<Appointment>(null);

            appointment.AppointmentId = nonExistingAppointmentId;
            employee.EmployeeId = Guid.NewGuid();
            contact.ContactId = Guid.NewGuid();

            //Act
            var result = sut.Save(appointment.AppointmentId, appointment);

            //Assert
            mockAppointmentRepository
                .Verify(a => a.Retrieve(appointment.AppointmentId), Times.Once());

            mockAppointmentRepository
                .Verify(a => a.Create(appointment), Times.Once());

        }

        [TestMethod]
        public void Save_WithValidData_ReturnsAppointmentWithAppointmentId()
        {

            //Arrange
            mockAppointmentRepository
                .Setup(a => a.Create(appointment))
                    .Callback(() => {
                        appointment.AppointmentId = Guid.NewGuid();
                        appointment.HostId = Guid.NewGuid();
                        appointment.GuestId = Guid.NewGuid();
                    })
                    .Returns(appointment);

            //Act
            var newAppointment = sut.Save(appointment.AppointmentId, appointment);

            //Assert

            Assert.IsTrue(newAppointment.AppointmentId != Guid.Empty);
        }

        
        [TestMethod]
        public void Save_WithExistingAppointmentWithValidData_ShouldCallRepositoryRetrieveAndUpdate()
        {
            //Arrange

            mockAppointmentRepository
               .Setup(e => e.Retrieve(existingAppointId))
               .Returns(appointment);

            appointment.AppointmentId = existingAppointId;

            //Act
            var result = sut.Save(appointment.AppointmentId, appointment);

            //Assert
            mockAppointmentRepository
                .Verify(e => e.Retrieve(appointment.AppointmentId), Times.Once());

            mockAppointmentRepository
                .Verify(e => e.Update(appointment.AppointmentId, appointment), Times.Once());

        }

        [TestMethod]
        public void Save_WithBlackAppointmentDate_ThrowAppointmentDateRequiredException()
        {
            //Arrange

            appointment.AppointmentDate = null;

            //Assert
            Assert.ThrowsException<AppointmentDateRequiredException>(
                () => sut.Save(appointment.AppointmentId, appointment)
                );

            mockAppointmentRepository
                .Verify(e => e.Create(appointment), Times.Never());

        }

       
        [TestMethod]
        public void Save_WithValuehaveGreaterThanTodayAppointmentDate_ThrowAppointmentDateRequiredException()
        {
            //Arrange
            appointment.AppointmentDate = DateTime.Now.AddDays(-1);

            //Assert
            Assert.ThrowsException<AppointmentDateRequiredException>(
                () => sut.Save(appointment.AppointmentId, appointment)
                );

            mockAppointmentRepository
                .Verify(e => e.Create(appointment), Times.Never());

        }

        [TestMethod]
        public void Save_WithBlackStartTime_ThrowStartDateRequiredException()
        {
            //Arrange

            appointment.StartTime = null;

            //Assert
            Assert.ThrowsException<StartDateRequiredException>(
                () => sut.Save(appointment.AppointmentId, appointment)
                );

            mockAppointmentRepository
                .Verify(e => e.Create(appointment), Times.Never());

        }

        
        [TestMethod]
        public void Save_WithBlackEndTIme_ThrowEndDateRequiredException()
        {
            //Arrange

            appointment.EndTime = null;

            //Assert
            Assert.ThrowsException<EndDateRequiredException>(
                () => sut.Save(appointment.AppointmentId, appointment)
                );

            mockAppointmentRepository
                .Verify(e => e.Create(appointment), Times.Never());

        }

        [TestMethod]
        public void Save_WithStartDateIsLessThanEndDate_ThrowStartDateRequiredException()
        {
            //Arrange
            appointment.StartTime = DateTime.Now.TimeOfDay.Add(TimeSpan.Parse("1:00:00"));
            appointment.EndTime = DateTime.Now.TimeOfDay;

            //Assert
            Assert.ThrowsException<StartDateRequiredException>(
                () => sut.Save(appointment.AppointmentId, appointment)
                );

            mockAppointmentRepository
                .Verify(e => e.Create(appointment), Times.Never());

        }

        [TestMethod]
        public void Save_WithBlankBHostId_ThrowHostIdRequiredException()
        {
            //Arrange
            appointment.HostId = Guid.Empty;


            //Assert
            Assert.ThrowsException<HostIdRequiredException>(
                ()=> sut.Save(appointment.AppointmentId, appointment)
                );

            mockAppointmentRepository
                .Verify(e => e.Create(appointment), Times.Never());


        }

        [TestMethod]
        public void Save_WithBlankBGuestId_ThrowGuestIdRequiredException()
        {
            //Arrange
            appointment.GuestId = Guid.Empty;


            //Assert
            Assert.ThrowsException<GuestIdRequiredException>(
                () => sut.Save(appointment.AppointmentId, appointment)
                );

            mockAppointmentRepository
                .Verify(e => e.Create(appointment), Times.Never());


        }

    }
}
