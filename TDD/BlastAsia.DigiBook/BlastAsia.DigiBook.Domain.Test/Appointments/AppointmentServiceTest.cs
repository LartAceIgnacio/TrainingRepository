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
        private Contact contact = new Contact();
        private Employee employee = new Employee();
        private Mock<IAppointmentRepository> mockAppointmentRepository;
        private Mock<IEmployeeRepository> mockHostRepository;
        private Mock<IContactRepository> mockGuestRepository;
        private AppointmentService sut;
        private Appointment appointment;
        private Guid existingAppointmentId = Guid.NewGuid();
        private Guid nonExistingAppointmentId = Guid.Empty;
        private Guid existingGuestId = Guid.NewGuid();
        private Guid existingHostId = Guid.NewGuid();

        [TestInitialize]
        public void Initialize()
        {
            mockAppointmentRepository = new Mock<IAppointmentRepository>();
            mockHostRepository = new Mock<IEmployeeRepository>();
            mockGuestRepository = new Mock<IContactRepository>();
            sut = new AppointmentService(mockAppointmentRepository.Object, mockGuestRepository.Object, mockHostRepository.Object);
            appointment = new Appointment
            {
                AppointmentDate = DateTime.Today,
                GuestId = existingGuestId,
                HostId = existingHostId,
                StartTime = DateTime.Now.TimeOfDay,
                EndTime = DateTime.Now.TimeOfDay.Add(TimeSpan.Parse("1:00:00")),
                IsCancelled = false,
                IsDone = false,
                Notes = "Interview"
            };
            mockGuestRepository
                .Setup(gr => gr.Retrieve(appointment.GuestId))
                .Returns(contact);

            mockHostRepository
                 .Setup(gr => gr.Retrieve(appointment.HostId))
                 .Returns(employee);

            mockAppointmentRepository
               .Setup(c => c.Retrieve(existingAppointmentId))
               .Returns(appointment);

            mockAppointmentRepository
                .Setup(c => c.Retrieve(nonExistingAppointmentId))
                .Returns<Appointment>(null);
        }

        [TestCleanup]
        public void CleanUp()
        {

        }

        [TestMethod]
        public void Create_NewAppointmentWithValidData_ShouldCallSaveRepository()
        {
            sut.Save(appointment.AppointmentId, appointment);
            mockAppointmentRepository
                .Verify(c => c.Retrieve(nonExistingAppointmentId), Times.Once);
            mockAppointmentRepository
                .Verify(ar => ar.Create(appointment), Times.Once());
        }

        [TestMethod]
        public void Create_WithValidDataWithExistingAppointmentId_ShouldCallRepositoryUpdate()
        {
            //Arrange
            appointment.AppointmentId = existingAppointmentId;

            //Act
            sut.Save(appointment.AppointmentId, appointment);

            //Assert
            mockAppointmentRepository
                .Verify(x => x.Retrieve(appointment.AppointmentId), Times.Once);
            mockAppointmentRepository
                .Verify(x => x.Update(appointment.AppointmentId, appointment), Times.Once);


        }


        [TestMethod]
        public void Create_AppointmentDateShouldNotLapsed_ThrowsAppointmentDateLapsedAlreadyException()
        {
            appointment.AppointmentDate = new DateTime(2017, 12, 11);

            Assert.ThrowsException<AppointmentDateLapsedAlreadyException>(
                () => sut.Save(appointment.AppointmentId, appointment));
            mockAppointmentRepository
                .Verify(ar => ar.Create(appointment), Times.Never());
        }

      
        [TestMethod]
        public void Create_StartTimeGreaterThanEndTime_ThrowsInclusiveTimeException()
        {
            appointment.StartTime = DateTime.Now.TimeOfDay.Add(TimeSpan.Parse("4:00:00"));

            Assert.ThrowsException<InclusiveTimeException>(
                () => sut.Save(appointment.AppointmentId, appointment));
            mockAppointmentRepository
                .Verify(ar => ar.Create(appointment), Times.Never());
        }

        [TestMethod]
        public void Create_WithNonExistingGuestId_ThrowsNonExistingContactException()
        {
 
            mockGuestRepository
                 .Setup(gr => gr.Retrieve(appointment.GuestId))
                .Returns<Contact>(null);

            Assert.ThrowsException<NonExistingContactException>(
                () => sut.Save(appointment.AppointmentId, appointment));
            mockGuestRepository
                .Verify(gr => gr.Retrieve(appointment.GuestId), Times.Once());
            mockAppointmentRepository
                .Verify(ar => ar.Create(appointment), Times.Never());
        }

        [TestMethod]
        public void Create_WithNonExistingHostId_ThrowsNonExistingEmployeeException()
        {
            mockHostRepository
                .Setup(hr => hr.Retrieve(appointment.HostId))
                .Returns<Employee>(null);

            Assert.ThrowsException<NonExistingEmployeeException>(
                () => sut.Save(appointment.AppointmentId, appointment));
            mockHostRepository
                .Verify(hr => hr.Retrieve(appointment.HostId), Times.Once());
            mockAppointmentRepository
                .Verify(ar => ar.Create(appointment), Times.Never());
        }

        [TestMethod]
        public void Create_StartTimeAndEndTimeEqual_ThrowsNonExistingEmployeeException()
        {
            appointment.StartTime = DateTime.Now.TimeOfDay;
            appointment.EndTime = DateTime.Now.TimeOfDay;

            Assert.ThrowsException<InclusiveTimeException>(
                () => sut.Save(appointment.AppointmentId, appointment));
            mockAppointmentRepository
                .Verify(ar => ar.Create(appointment), Times.Never());
        }
    }   
}
