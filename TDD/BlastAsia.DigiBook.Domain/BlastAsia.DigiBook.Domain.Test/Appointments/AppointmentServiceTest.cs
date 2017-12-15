using BlastAsia.DigiBook.Domain.Appointments;
using BlastAsia.DigiBook.Domain.Appointments.Exceptions;
using BlastAsia.DigiBook.Domain.Contacts.Interfaces;
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
        private Mock<IAppointmentRepository> mockAppointRepository;
        private Mock<IEmployeeRepository> mockEmployeeRepository;
        private Mock<IContactRepository> mockContactRepository;

        private readonly Guid nonExistingGuestId = Guid.Empty;
        private readonly Guid existingGuestId = Guid.NewGuid();

        private readonly Guid nonExistingAppointmentId = Guid.Empty;
        private readonly Guid existingAppointmentId = Guid.NewGuid();

        private readonly Guid nonExistingHostId = Guid.Empty;
        private readonly Guid existingHostId = Guid.NewGuid();
        private AppointmentService sut;
        private Appointment appointment;

        [TestInitialize]
        public void InitializeTest()
        {
            mockAppointRepository = new Mock<IAppointmentRepository>();
            mockEmployeeRepository = new Mock<IEmployeeRepository>();
            mockContactRepository = new Mock<IContactRepository>();

            sut = new AppointmentService(
                mockAppointRepository.Object,
                mockContactRepository.Object,
                mockEmployeeRepository.Object);

            appointment = new Appointment
            {
                AppointmentDate = DateTime.Today.AddDays(1),
                GuestId = existingGuestId,
                HostId = existingHostId,
                StartTime = DateTime.Now.TimeOfDay,
                EndTime = DateTime.Now.TimeOfDay.Add(TimeSpan.Parse("01:00:00")),
                IsCancelled = false,
                IsDone = true,
                Notes = "Sucess"
            };
            mockAppointRepository
              .Setup(ar => ar.Create(appointment))
              .Callback(() => appointment.AppointmentId = Guid.NewGuid())
                  .Returns(appointment);

            mockContactRepository
               .Setup(cr => cr.Retrieve(nonExistingGuestId))
               .Returns<Contact>(null);

            mockEmployeeRepository
              .Setup(cr => cr.Retrieve(nonExistingHostId))
              .Returns<Employee>(null);

            mockAppointRepository
              .Setup(
                  rc => rc.Retrieve(existingAppointmentId)
              )
              .Returns(appointment);

            mockContactRepository
                .Setup(
                    rc => rc.Retrieve(existingGuestId)
                )
                .Returns(new Contact { ContactId = Guid.NewGuid() });

            mockEmployeeRepository
                .Setup(
                    rc => rc.Retrieve(existingHostId)
                )
                .Returns(new Employee { EmployeeId = Guid.NewGuid() });

        }

       

        [TestMethod]
        public void Save_AppointmentWithInvalidAppointmentDate_ShouldThrowInvalidAppointmentDateException()
        {
            appointment.AppointmentDate = DateTime.Now.AddDays(-2);

            Assert.ThrowsException<InvalidAppointmentDateException>(
                () => sut.Save(appointment));

            mockAppointRepository
                .Verify(ar => ar.Create(appointment), Times.Never);
        }
        
        [TestMethod]
        public void Save_AppointmentWithInvalidStartAndEndTime_ShouldThrowInvalidStartAndEndTimeException()
        {
            appointment.StartTime = appointment.EndTime.Add(TimeSpan.Parse("2:00:00"));
            Assert.ThrowsException<InvalidStartAndEndTimeException>(
                () => sut.Save(appointment));

            mockAppointRepository
                .Verify(ar => ar.Create(appointment), Times.Never);
        }


        [TestMethod]
        public void Save_AppointmentWithNonExistingGuestId_ShouldThrowInvalidGuestIdException()
        {

            appointment.GuestId = nonExistingGuestId;

            
            Assert.ThrowsException<InvalidGuestIdException>(
                () => sut.Save(appointment)
                );
         
            mockContactRepository
                .Verify(
                    cr => cr.Retrieve(nonExistingGuestId), Times.Once()
                );
            
            mockAppointRepository
                .Verify(
                    ar => ar.Create(appointment), Times.Never()
                );
        }

        [TestMethod]
        public void Save_AppointmentWithExistingGuestIdAndHostId_ShouldCallContactRepositoryEmployeeRepositoryRetrieveAndAppointmentRepositoryCreate()
        {

            sut.Save(appointment);

            mockContactRepository
                .Verify(cr => cr.Retrieve(appointment.GuestId), Times.Once);


            mockEmployeeRepository
                .Verify(cr => cr.Retrieve(appointment.HostId), Times.Once);


            mockAppointRepository
                .Verify(cr => cr.Create(appointment), Times.Once);
        }

        [TestMethod]
        public void Save_AppointmentWithExistingAppointmentId_ShouldCallAppointmentRepositoryUpdate()
        {
            appointment.AppointmentId = existingAppointmentId;
            sut.Save(appointment);

            mockAppointRepository
                .Verify(ar => ar.Retrieve(appointment.AppointmentId), Times.Once);

            mockAppointRepository
                .Verify(ar => ar.Update(appointment.AppointmentId), Times.Once);
        }

        [TestMethod]
        public void Save_AppointmentWithNonExistingHostId_ShouldThrowInvalidHostIdException()
        {

            appointment.HostId = nonExistingHostId;

            Assert.ThrowsException<InvalidHostIdException>(
                () => sut.Save(appointment)
                );

            mockEmployeeRepository
                .Verify(
                    cr => cr.Retrieve(nonExistingHostId), Times.Once()
                );

            mockAppointRepository
                .Verify(
                    ar => ar.Create(appointment), Times.Never()
                );
        }

        [TestMethod]
        public void Save_AppointmentWithValidData_ShouldReturnAppointmentId()
        {


            sut.Save(appointment);

            Assert.IsTrue(appointment.AppointmentId != Guid.Empty);
        }
    }
}
