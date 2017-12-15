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
        private Appointement appointment;
        private Employee employee = new Employee();
        private Contact contact = new Contact();
        private Guid existingAppointId;
        private Guid nonExistingAppointmentId;


        [TestInitialize]
        public void AppointmentInitialize()
        {
            appointment = new Appointement
            {
                AppointmnetDate = DateTime.Today,
                StartTime = DateTime.Now.TimeOfDay,
                EndTime = DateTime.Now.TimeOfDay.Add(TimeSpan.Parse("1:00:00")),
                IsCanceleld = false,
                IsDone = true,
                Notes = ""

            };

            existingAppointId = Guid.NewGuid();
            nonExistingAppointmentId = Guid.Empty;

            mockEmployeeRepository = new Mock<IEmployeeRepository>();
            mockContactRepository = new Mock<IContactRepository>();
            mockAppointmentRepository = new Mock<IAppointmentRepository>();

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
               .Returns<Appointement>(null);

            appointment.AppointmentId = nonExistingAppointmentId;
            employee.EmployeeId = Guid.NewGuid();
            contact.ContactId = Guid.NewGuid();

            //Act
            var result = sut.Save(appointment,employee,contact);

            //Assert
            mockAppointmentRepository
                .Verify(a => a.Retrieve(appointment.AppointmentId), Times.Once());

            mockAppointmentRepository
                .Verify(a => a.Create(appointment, employee.EmployeeId, contact.ContactId), Times.Once());

        }

        [TestMethod]
        public void Save_WithValidData_ReturnsAppointmentWithAppointmentId()
        {

            //Arrange
            mockAppointmentRepository
                .Setup(a => a.Create(appointment, employee.EmployeeId, contact.ContactId))
                    .Callback(() => {
                        appointment.AppointmentId = Guid.NewGuid();
                        employee.EmployeeId = Guid.NewGuid();
                        contact.ContactId = Guid.NewGuid();
                    })
                    .Returns(appointment);

            //Act
            var newAppointment = sut.Save(appointment, employee, contact);

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
            var result = sut.Save(appointment, employee, contact);

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

            appointment.AppointmnetDate = null;

            //Assert
            Assert.ThrowsException<AppointmentDateRequiredException>(
                () => sut.Save(appointment, employee, contact)
                );

            mockAppointmentRepository
                .Verify(e => e.Create(appointment, employee.EmployeeId, contact.ContactId), Times.Never());

        }

       
        [TestMethod]
        public void Save_WithValuehaveGreaterThanTodayAppointmentDate_ThrowAppointmentDateRequiredException()
        {
            //Arrange
            appointment.AppointmnetDate = DateTime.Now.AddDays(-1);

            //Assert
            Assert.ThrowsException<AppointmentDateRequiredException>(
                () => sut.Save(appointment, employee, contact)
                );

            mockAppointmentRepository
                .Verify(e => e.Create(appointment, employee.EmployeeId, contact.ContactId), Times.Never());

        }

        [TestMethod]
        public void Save_WithBlackStartTime_ThrowStartDateRequiredException()
        {
            //Arrange

            appointment.StartTime = null;

            //Assert
            Assert.ThrowsException<StartDateRequiredException>(
                () => sut.Save(appointment, employee, contact)
                );

            mockAppointmentRepository
                .Verify(e => e.Create(appointment, employee.EmployeeId, contact.ContactId), Times.Never());

        }

        
        [TestMethod]
        public void Save_WithBlackEndTIme_ThrowEndDateRequiredException()
        {
            //Arrange

            appointment.EndTime = null;

            //Assert
            Assert.ThrowsException<EndDateRequiredException>(
                () => sut.Save(appointment, employee, contact)
                );

            mockAppointmentRepository
                .Verify(e => e.Create(appointment, employee.EmployeeId, contact.ContactId), Times.Never());

        }

        [TestMethod]
        public void Save_WithStartDateIsLessThanEndDate_ThrowStartDateRequiredException()
        {
            //Arrange
            appointment.StartTime = DateTime.Now.TimeOfDay.Add(TimeSpan.Parse("1:00:00"));
            appointment.EndTime = DateTime.Now.TimeOfDay;

            //Assert
            Assert.ThrowsException<StartDateRequiredException>(
                () => sut.Save(appointment, employee, contact)
                );

            mockAppointmentRepository
                .Verify(e => e.Create(appointment, employee.EmployeeId, contact.ContactId), Times.Never());

        }

    }
}
