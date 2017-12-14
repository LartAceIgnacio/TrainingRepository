using BlastAsia.DigiBook.Domain.Appointments;
using BlastAsia.DigiBook.Domain.Contacts;
using BlastAsia.DigiBook.Domain.Employees;
using BlastAsia.DigiBook.Domain.Models.Appoinments;
using BlastAsia.DigiBook.Domain.Models.Contacts;
using BlastAsia.DigiBook.Domain.Models.Employees;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Test.Appoinments
{
    [TestClass]
    public class AppointmentServiceTest
    {
        private Appoinment appoinment;
        private Contact contact = new Contact();
        private Employee employee = new Employee();
        private AppoinmentService sut;
        private Mock<IAppoinmentRepository> mockAppoinmentRepository;
        private Mock<IEmployeeRepository> mockEmployeeRepository;
        private Mock<IContactRepository> mockContactRepository;

        private Guid existingAppoinmentId = Guid.NewGuid();
        private Guid nonExistingAppoinmentId = Guid.Empty;
        private Guid existingContactId = Guid.NewGuid();
        private Guid existingEmployeeId = Guid.NewGuid();

        [TestInitialize]
        public void Initialize()
        {
            appoinment = new Appoinment {
                AppointmentDate = DateTime.Today,
                GuestId = existingContactId,
                HostId = existingEmployeeId,
                StartTime = DateTime.Now.TimeOfDay,
                EndTime = DateTime.Now.TimeOfDay.Add(TimeSpan.Parse("01:00:00")),
                IsCancelled = false,
                IsDone = true,
                Notes = "Sucess"
            };

            mockAppoinmentRepository = new Mock<IAppoinmentRepository>();
            mockEmployeeRepository = new Mock<IEmployeeRepository>();
            mockContactRepository = new Mock<IContactRepository>();

            mockEmployeeRepository
                .Setup(x => x.Retrieve(appoinment.HostId))
                .Returns(employee);

            mockContactRepository
                .Setup(x => x.Retrieve(appoinment.GuestId))
                .Returns(contact);

            mockAppoinmentRepository
                .Setup(x => x.Retrieve(existingAppoinmentId))
                .Returns(appoinment);

            mockAppoinmentRepository
                .Setup(x => x.Retrieve(nonExistingAppoinmentId))
                .Returns<Appoinment>(null);

            sut = new AppoinmentService(mockAppoinmentRepository.Object, mockEmployeeRepository.Object, mockContactRepository.Object);
        }

        [TestCleanup]
        public void Cleanup()
        {

        }

        [TestMethod]
        public void Save_NewAppointmentWithValidData_ShouldCallRepositoryCreate()
        {
            //Arrange
            appoinment.HostId = existingEmployeeId;
            appoinment.GuestId = existingContactId;

            //Act
            sut.Save(appoinment);

            //Assert
            mockAppoinmentRepository.Verify(x => x.Retrieve(appoinment.AppointmentId), Times.Once);
            mockAppoinmentRepository.Verify(x => x.Create(appoinment), Times.Once);
        }

        [TestMethod]
        public void Save_WithValidData_ShouldCallRepositoryUpdate()
        {
            //Arrange
            appoinment.HostId = existingEmployeeId;
            appoinment.GuestId = existingContactId;
            appoinment.AppointmentId = existingAppoinmentId;

            //Act
            sut.Save(appoinment);

            //Assert
            mockAppoinmentRepository.Verify(x => x.Retrieve(appoinment.AppointmentId), Times.Once);
            mockAppoinmentRepository.Verify(x => x.Update(appoinment.AppointmentId, appoinment), Times.Once);
        }

        [TestMethod]
        public void Save_WithGuestIdNotExisted_ThrowsContactIdNotExistedException()
        {
            //Arrange
            appoinment.HostId = existingEmployeeId;
            appoinment.GuestId = Guid.Empty;
            appoinment.AppointmentId = existingAppoinmentId;

            //Act

            //Assert
            Assert.ThrowsException<ContactIdNotExistedException>(() => sut.Save(appoinment));
            mockAppoinmentRepository.Verify(x => x.Retrieve(It.IsAny<Guid>()), Times.Never);
        }

        [TestMethod]
        public void Save_WithHostIdNotExisted_ThrowsEmployeeIdNotExistedException()
        {
            //Arrange
            appoinment.HostId = Guid.Empty;
            appoinment.GuestId = existingContactId;
            appoinment.AppointmentId = existingAppoinmentId;

            //Act

            //Assert
            Assert.ThrowsException<EmployeeIdNotExistedException>(() => sut.Save(appoinment));
            mockAppoinmentRepository.Verify(x => x.Retrieve(It.IsAny<Guid>()), Times.Never);
        }

        [TestMethod]
        public void Save_WithAppointmentDateLessThanDateToday_ThrowsAppointmentDateLessThanDateTodayException()
        {
            //Arrange
            appoinment.AppointmentDate = DateTime.Today.AddMonths(-5);

            //Act


            //Assert
            Assert.ThrowsException<AppointmentDateLessThanDateTodayException>(() => sut.Save(appoinment));
            mockAppoinmentRepository.Verify(x => x.Retrieve(It.IsAny<Guid>()), Times.Never);
        }

        [TestMethod]
        public void Save_WithStartTimeGreaterThanEndTime_ThrowsTimeInclusiveException()
        {
            //Arrange
            appoinment.EndTime = DateTime.Now.TimeOfDay;
            appoinment.StartTime = DateTime.Now.TimeOfDay.Add(TimeSpan.Parse("01:00:00"));

            //Act


            //Assert
            Assert.ThrowsException<TimeInclusiveException>(() => sut.Save(appoinment));
            mockAppoinmentRepository.Verify(x => x.Retrieve(It.IsAny<Guid>()), Times.Never);
        }

        [TestMethod]
        public void Save_WithEndTimeLessThanOrEqualStartTime_ThrowsTimeInclusiveException()
        {
            //Arrange
            appoinment.EndTime = DateTime.Now.TimeOfDay.Subtract(TimeSpan.Parse("01:00:00"));
            appoinment.StartTime = DateTime.Now.TimeOfDay;

            //Act


            //Assert
            Assert.ThrowsException<TimeInclusiveException>(() => sut.Save(appoinment));
            mockAppoinmentRepository.Verify(x => x.Retrieve(It.IsAny<Guid>()), Times.Never);
        }
    }
}
