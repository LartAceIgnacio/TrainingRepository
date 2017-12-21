using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlastAsia.DigiBook.Domain.Models.Employees;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using BlastAsia.DigiBook.Domain.Employees;
using System.IO;

namespace BlastAsia.DigiBook.Domain.Test.Employees
{
    [TestClass]
    public class EmployeeServiceTest
    {

        private Employee employee;
        private Mock<IEmployeeRepository> mockEmployeeRepository;
        private EmployeeService sut;
        private Guid existingEmployeeId = Guid.NewGuid();
        private Guid nonExistingEmployeeId = Guid.Empty;

        [TestInitialize]
        public void Initialize()
        {
            employee = new Employee
            {
                FirstName = "Angela Blanche",
                LastName = "Olarte",
                MobilePhone = "09981642039",
                EmailAddress = "abbieolarte@gmail.com",
                Photo = new MemoryStream(),
                OfficePhone = "555222",
                Extension = "105"
            };

            mockEmployeeRepository = new Mock<IEmployeeRepository>();

            mockEmployeeRepository
                .Setup(e => e.Retrieve(existingEmployeeId))
                .Returns(employee);

            mockEmployeeRepository
                .Setup(e => e.Retrieve(nonExistingEmployeeId))
                .Returns<Employee>(null);

            sut = new EmployeeService(mockEmployeeRepository.Object);
        }

        [TestCleanup]
        public void CleanUp()
        {

        }

        [TestMethod]
        public void Save_NewEmployeeWithValidData_ShouldCallRepositoryCreate()
        {
            // Arrange
            var result = sut.Save(employee.EmployeeId, employee);

            // Assert
            mockEmployeeRepository
                .Verify(e => e.Retrieve(nonExistingEmployeeId), Times.Once());

            mockEmployeeRepository
                .Verify(e => e.Create(employee), Times.Once());
        }

        [TestMethod]
        public void Save_ExistingEmployee_ShouldCallRepositoryUpdate()
        {
            // Arrange
            employee.EmployeeId = existingEmployeeId;

            // Act
            sut.Save(employee.EmployeeId, employee);

            //Assert
            mockEmployeeRepository
                .Verify(e => e.Retrieve(existingEmployeeId), Times.Once);

            mockEmployeeRepository
                .Verify(e => e.Update(existingEmployeeId, employee), Times.Once);
        }

        [TestMethod]
        public void Save_WithValidData_ReturnsNewEmployeeWithEmployeeId()
        {
            mockEmployeeRepository
                .Setup(e => e.Create(employee))
                .Callback(() => employee.EmployeeId = Guid.NewGuid())
                .Returns(employee);

            // Act
            var newEmployee = sut.Save(employee.EmployeeId, employee);

            //Assert
            Assert.IsTrue(employee.EmployeeId != Guid.Empty);
        }

        [TestMethod]
        public void Save_EmployeeWithBlankFirstName_ThrowsNameRequiredException()
        {
            employee.FirstName = "";

            // Assert
            Assert.ThrowsException<NameRequiredException>(
                () => sut.Save(employee.EmployeeId, employee));
            mockEmployeeRepository
                .Verify(e => e.Create(employee), Times.Never());
        }

        [TestMethod]
        public void Save_EmployeeWithBlankLastName_ThrowsNameRequiredException()
        {
            employee.LastName = "";

            // Assert
            Assert.ThrowsException<NameRequiredException>(
                () => sut.Save(employee.EmployeeId, employee));
            mockEmployeeRepository
                .Verify(e => e.Create(employee), Times.Never());
        }

        [TestMethod]
        public void Save_EmployeeWithBlankPhoneNumber_ThrowsPhoneNumberRequiredException()
        {
            employee.MobilePhone = "";

            // Assert
            Assert.ThrowsException<PhoneNumberRequiredException>(
                () => sut.Save(employee.EmployeeId, employee));
            mockEmployeeRepository
                .Verify(e => e.Create(employee), Times.Never());
        }

        [TestMethod]
        public void Save_EmployeeWithBlankEmail_ThrowsEmailRequiredException()
        {
            employee.EmailAddress = "";

            Assert.ThrowsException<EmailRequiredException>(
                () => sut.Save(employee.EmployeeId, employee));
            mockEmployeeRepository
                .Verify(e => e.Create(employee), Times.Never());
        }

        [TestMethod]
        public void Save_EmployeeWithInvalidEmail_ThrowsEmailRequiredException()
        {
            employee.EmailAddress = "abbieolarte.gmail.com";

            Assert.ThrowsException<EmailRequiredException>(
                () => sut.Save(employee.EmployeeId, employee));
            mockEmployeeRepository
                .Verify(e => e.Create(employee), Times.Never());
        }

        //[TestMethod]
        //public void Save_EmployeeWithNoPhoto_ThrowsPhotoRequiredException()
        //{
        //    employee.Photo = null;

        //    Assert.ThrowsException<PhotoRequiredException>(
        //        () => sut.Save(employee.EmployeeId, employee));
        //    mockEmployeeRepository
        //        .Verify(e => e.Create(employee), Times.Never());
        //}

        [TestMethod]
        public void Save_EmployeeWithBlankOfficePhoneNumber_ThrowsPhoneNumberRequiredException()
        {
            employee.OfficePhone = "";

            Assert.ThrowsException<PhoneNumberRequiredException>(
                () => sut.Save(employee.EmployeeId, employee));
            mockEmployeeRepository
                .Verify(e => e.Create(employee), Times.Never());
        }
    }
}
