using BlastAsia.DigiBook.Domain.Appointments;
using BlastAsia.DigiBook.Domain.Appointments.Exceptions;
using BlastAsia.DigiBook.Domain.Contacts;
using BlastAsia.DigiBook.Domain.Employees;
using BlastAsia.DigiBook.Domain.Models.Appointments;
using BlastAsia.DigiBook.Domain.Models.Employees;
using BlastAsia.DigiBook.Domain.Test.Contacts.Contacts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace BlastAsia.DigiBook.Domain.Test.Appointments
{
    [TestClass]
    public class AppointmentServiceTest
    {
        private Appointment appointment;
        private Contact contact = new Contact();
        private Employee employee = new Employee();
        private AppointmentService sut;
        private Mock<IAppointmentRepository> mockAppoinmentRepository;
        private Mock<IEmployeeRepository> mockEmployeeRepository;
        private Mock<IContactRepository> mockContactRepository;

        private Guid existingAppoinmentId = Guid.NewGuid();
        private Guid nonExistingAppoinmentId = Guid.Empty;
        private Guid existingContactId = Guid.NewGuid();
        private Guid existingEmployeeId = Guid.NewGuid();

        [TestInitialize]
        public void Initialize()
        {
            mockAppoinmentRepository = new Mock<IAppointmentRepository>();
            mockEmployeeRepository = new Mock<IEmployeeRepository>();
            mockContactRepository = new Mock<IContactRepository>();

            sut = new AppointmentService(
                mockAppoinmentRepository.Object,
                mockEmployeeRepository.Object,
                mockContactRepository.Object);

            appointment = new Appointment
            {
                AppointmentDate = DateTime.Today.AddDays(1),
                GuestId = existingContactId,
                HostId = existingEmployeeId,
                StartTime = DateTime.Now.TimeOfDay,
                EndTime = DateTime.Now.TimeOfDay.Add(TimeSpan.Parse("02:00:00")),
                IsCancelled = false,
                IsDone = false,
                Notes = "Sample"
            };

            mockAppoinmentRepository
                .Setup(c => c.Retrieve(existingAppoinmentId))
                .Returns(appointment); // AppointmentID
            mockContactRepository
                .Setup(c => c.Retrieve(appointment.GuestId))
                .Returns(contact); // GuestID
            mockEmployeeRepository
                .Setup(c => c.Retrieve(appointment.HostId))
                .Returns(employee); // HostID
            mockAppoinmentRepository
                .Setup(c => c.Retrieve(nonExistingAppoinmentId))
                .Returns<Appointment>(null);
        }

        [TestCleanup]
        public void Cleanup()
        {

        }

        [TestMethod]
        public void Create_NewAppointmentWithValidData_ShouldCallRepositoryCreate()
        {
            //Arrange

            appointment.GuestId = existingContactId;
            appointment.HostId = existingEmployeeId;


            //Act

            sut.Create(appointment);

            //Assert

            mockAppoinmentRepository
                .Verify(c => c.Retrieve(appointment.AppointmentId)
                , Times.Once);
            mockAppoinmentRepository
                .Verify(c => c.Create(appointment)
                , Times.Once);
        }

        [TestMethod]
        public void Create_WithValidData_ShouldCallRepositoryUpdate()
        {
            //Arrange

            appointment.AppointmentId = existingAppoinmentId;
            appointment.GuestId = existingContactId;
            appointment.HostId = existingEmployeeId;
            
            //Act

            sut.Create(appointment);

            //Assert

            mockAppoinmentRepository
                .Verify(c => c.Retrieve(appointment.AppointmentId)
                , Times.Once);
            mockAppoinmentRepository
                .Verify(c => c.Update(appointment.AppointmentId, appointment)
                , Times.Once);
        }

        [TestMethod]
        public void Create_WithGuestIdNotExisting_ThrowsGuestIdException()
        {
            //Arrange

            appointment.AppointmentId = existingAppoinmentId;
            appointment.GuestId = Guid.Empty;
            appointment.HostId = existingEmployeeId;
         
            //Act

            //Assert

            Assert.ThrowsException<GuestIdException>(
                () => sut.Create(appointment));
            mockAppoinmentRepository
                .Verify(c => c.Create(appointment)
                , Times.Never);
        }

        [TestMethod]
        public void Create_WithHostIdNotExisting_ThrowsEmployeeIdException()
        {
            //Arrange

            appointment.AppointmentId = existingAppoinmentId;            
            appointment.GuestId = existingContactId;
            appointment.HostId = Guid.Empty;
            
            //Act

            //Assert
            Assert.ThrowsException<EmployeeIdException>(
                () => sut.Create(appointment));
            mockAppoinmentRepository
                .Verify(c => c.Create(appointment)
                , Times.Never);
        }

        [TestMethod]
        public void Create_WithAppointmentDateLessThanDateToday_ThrowsAppointmentDateException()
        {
            //Arrange

            appointment.AppointmentDate = DateTime.Today.AddMonths(-1);

            //Act


            //Assert

            Assert.ThrowsException<AppointmentDateException>(
                () => sut.Create(appointment));
            mockAppoinmentRepository
                .Verify(c => c.Create(appointment)
            , Times.Never);
        }

        [TestMethod]
        public void Create_WithStartTimeGreaterThanEndTime_ThrowsTimeInclusiveException()
        {
            //Arrange

            appointment.StartTime = DateTime.Now.TimeOfDay
                  .Add(TimeSpan.Parse("05:00:00"));
            appointment.EndTime = DateTime.Now.TimeOfDay;
            
            //Act
            
            //Assert

            Assert.ThrowsException<TimeInclusiveException>(
                () => sut.Create(appointment));
            mockAppoinmentRepository
                .Verify(c => c.Create(appointment)
                , Times.Never);
        }

        [TestMethod]
        public void Create_WithStartTimeEqualToEndTime_ThrowsTimeInclusiveException()
        {
            //Arrange

            appointment.StartTime = DateTime.Now.TimeOfDay;
            appointment.EndTime = DateTime.Now.TimeOfDay;
            //Act

            //Assert

            Assert.ThrowsException<TimeInclusiveException>(
                () => sut.Create(appointment));
            mockAppoinmentRepository
                .Verify(c => c.Create(appointment)
                , Times.Never);
        }
    }
}
