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
        private Mock<IAppointmentRepository> mockRepoAppointment;
        private Mock<IContactRepository> mockRepoContact;
        private Mock<IEmployeeRepository> mockRepoEmployee;

        private AppointmentService sut;
        private Appointment appointment;

        private Guid nonExistingGuestId = Guid.Empty;
        private Guid existingGuestId = Guid.NewGuid();

        private Guid nonExistingHostId = Guid.Empty;
        private Guid existingHostId = Guid.NewGuid();

        private Guid nonExistingAppointmentId = Guid.Empty;
        private Guid existingAppointmentId = Guid.NewGuid();

        [TestInitialize]
        public void Initialize()
        {
            appointment = new Appointment
            {
                AppointmentId = new Guid(),
                AppointmentDate =  DateTime.Now.AddDays(1),
                GuestId = existingGuestId,
                HostId = existingHostId,
                StartTime = new DateTime().TimeOfDay,
                EndTime = new DateTime().TimeOfDay,
                IsCancelled = false,
                IsDone = true,
                Notes = "Sample Notes"
            };

            mockRepoAppointment = new Mock<IAppointmentRepository>();
            mockRepoContact = new Mock<IContactRepository>();
            mockRepoEmployee = new Mock<IEmployeeRepository>();

            // for  nonExistingGuestId testing
            mockRepoContact
              .Setup(
                  rc => rc.Retrieve(nonExistingGuestId)
              )
              .Returns<Contact>(null);

            // for existing guest id testing
            mockRepoContact
             .Setup(
                 rc => rc.Retrieve(existingGuestId)
             )
             .Returns(new Contact { FirstName = "emem", LastName = "Magadia" });

            // for  non Existing HostId testing
            mockRepoEmployee
              .Setup(
                  rc => rc.Retrieve(nonExistingHostId)
              )
              .Returns<Employee>(null);

            // for Existing Host Id testing
            mockRepoEmployee
              .Setup(
                  rc => rc.Retrieve(existingHostId)
              )
              .Returns(new Employee { FirstName = "emem", LastName = "Magada" });

            // for  non Existing Appointment id testing
            mockRepoAppointment
              .Setup(
                  rc => rc.Retrieve(nonExistingAppointmentId)
              )
              .Returns<Appointment>(null);

            // for   Existing Appointment id testing
            mockRepoAppointment
              .Setup(
                  rc => rc.Retrieve(existingAppointmentId)
              )
              .Returns(appointment);

            sut = new AppointmentService(mockRepoAppointment.Object, mockRepoContact.Object, mockRepoEmployee.Object);

        }
        [TestMethod]
        public void Save_AppointmentWithValidData_ShouldReturnAppointmentId()
        {

            // arrange 
            mockRepoAppointment
                .Setup(
                    ra => ra.Create(appointment)
                )
                .Callback(
                    () =>
                    {
                        appointment.AppointmentId = Guid.NewGuid();
                    }
                )
                .Returns(appointment);

            // act 
            var result = sut.Save(appointment.AppointmentId, appointment);

            Assert.IsTrue(result.AppointmentId != Guid.Empty);
        }

        [TestMethod]
        public void Save_AppointmentWithNonExistingGuestId_ShouldInvokeContactRetrieveAndThrowInvalidGuestException()
        {
            // arrange 
            appointment.GuestId = nonExistingGuestId;
            // assert
            Assert.ThrowsException<InvalidGuestIdException>(
                () => sut.Save(appointment.AppointmentId, appointment)
                );

            mockRepoContact
                .Verify(
                    rc => rc.Retrieve(nonExistingGuestId), Times.Once()
                );

            mockRepoAppointment
                .Verify(
                    ra => ra.Create(appointment), Times.Never()
                );

        }


        [TestMethod]
        public void Save_AppointmentWithNonExistinHostId_ShouldInvokeRepositoryEmployeeRetrieveAndThrowInvalidHostException()
        {
            // arrange 

            appointment.HostId = nonExistingHostId;

            // assert
            Assert.ThrowsException<InvalidHostIdException>(
                () => sut.Save(appointment.AppointmentId, appointment)
                );

            mockRepoEmployee
                .Verify(
                    rc => rc.Retrieve(nonExistingHostId), Times.Once()
                );

            mockRepoAppointment
                .Verify(
                    ra => ra.Create(appointment), Times.Never()
                );

        }

        [TestMethod]
        public void Save_AppointmentWithExistingGuestAndHostId_ShouldInvokeEmployeeAndContactRepositoryRetrieveAndAppointmentCreateRepository()
        {
            // act 
            sut.Save(appointment.AppointmentId, appointment);
            // assert
            // employee
            mockRepoEmployee
                .Verify(
                    re => re.Retrieve(appointment.HostId), Times.Once()
                );
            // contact
            mockRepoContact
                .Verify(
                    rc => rc.Retrieve(appointment.GuestId), Times.Once()
              );
            //appointment
            mockRepoAppointment
                .Verify(
                    ra => ra.Create(appointment), Times.Once()
                );
        }

        [TestMethod]
        public void Save_WithExistingAppointmentId_ShouldCall_AppointmentRepositoryRetrieveAndUpdate()
        {
            //arrange
            appointment.AppointmentId = existingAppointmentId;
            // act 
            sut.Save(appointment.AppointmentId, appointment);
            // assert
            // retrieve
            mockRepoAppointment
                .Verify(
                    ra => ra.Retrieve(existingAppointmentId), Times.Once()
                );
            // Create
            mockRepoAppointment
                .Verify(
                    ra => ra.Update(existingAppointmentId, appointment), Times.Once()
                );
        }

        [TestMethod]
        public void Save_WithNonExistingAppointmentId_ShouldCall_AppointmentRepositoryRetrieveAndCreate()
        {
            //arrange
            appointment.AppointmentId = nonExistingAppointmentId;
            // act 
            sut.Save(appointment.AppointmentId, appointment);
            // assert
            // retrieve
            mockRepoAppointment
                .Verify(
                    ra => ra.Retrieve(nonExistingAppointmentId), Times.Once()
                );
            // Create
            mockRepoAppointment
                .Verify(
                    ra => ra.Create(appointment), Times.Once()
                );
        }

        [TestMethod]
        public void Save_WithPreviousDate_ShouldThrowInvalidAppointmentDateException()
        {
            // arrange 
            appointment.AppointmentDate = DateTime.Now.AddDays(-1);
            // act
            // assert
            Assert.ThrowsException<InvalidAppointmentDateException>(
                () => sut.Save(appointment.AppointmentId, appointment)
                );

            mockRepoAppointment
                .Verify(
                    ra => ra.Create(appointment), Times.Never()
                );
        }

        [TestMethod]
        public void Save_WithLargerEndTimeThanStartTime_ShouldThrowsNotInclusiveStartAndEndTime()
        {
            // arrange 

            appointment.StartTime = new DateTime().AddHours(2).TimeOfDay;
            appointment.EndTime = new DateTime().AddHours(1).TimeOfDay;

            // act
            // assert
            Assert.ThrowsException<NotInclusiveStartAndEndTime>(
                () => sut.Save(appointment.AppointmentId, appointment)
                );

            mockRepoAppointment
                .Verify(
                    ra => ra.Create(appointment), Times.Never()
                );
        }


    }
}
