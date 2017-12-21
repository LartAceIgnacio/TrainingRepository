using BlastAsia.DigiBook.Domain.Contacts.ContactExceptions;
using BlastAsia.DigiBook.Domain.Employees;
using BlastAsia.DigiBook.Domain.Employees.EmployeeExceptions;
using BlastAsia.DigiBook.Domain.Employees.Services;
using BlastAsia.DigiBook.Domain.Models.Employees;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Test.EmployeeTest
{
    [TestClass]
    public class EmployeeServiceTest
    {
        private EmployeeService _sut;
        private Mock<IEmployeeRepository> _mockEmployeeRepository;
        private Employee _employee;
        private Guid _nonExistingEmployeeId;
        private Guid _existingEmployeeId;

        [TestInitialize]
        public void Initialize()
        {

            _mockEmployeeRepository = new Mock<IEmployeeRepository>();
            _sut = new EmployeeService(_mockEmployeeRepository.Object);

            _employee = new Employee
            {
                Firstname = "Rose",
                Lastname = "Quilicol",
                MobilePhone = "09123456789",
                EmailAddress = "rquilicol@xyz.com",
                //Photo = new MemoryStream(),
                OfficePhone = "1234567",
                Extension = "02"
            };

            _nonExistingEmployeeId = Guid.Empty;
            _existingEmployeeId = Guid.NewGuid();

            _mockEmployeeRepository.Setup(e => e.Retrieve(_existingEmployeeId))
                .Returns(_employee);

            _mockEmployeeRepository.Setup(e => e.Retrieve(_nonExistingEmployeeId))
                .Returns<Employee>(null);
            
        }

        [TestCleanup]
        public void CleanUp() { }

        [TestMethod]
        public void Save_NewEmployeeWithValidData_ShouldCallEmployeeRepositoryCreate()
        {
            // Arrange

            // Act
            var result = _sut.Save(_employee.Id,_employee);

            // Assert
            _mockEmployeeRepository.Verify(c => c.Retrieve(_nonExistingEmployeeId), Times.Once);
            _mockEmployeeRepository.Verify(c => c.Create(_employee), Times.Once);

        }


        [TestMethod]
        public void Save_WithExistingEmployee_ShouldCallEmployeeRepositoryUpdate()
        {
            // Arrange
            _employee.Id = _existingEmployeeId;
            // Act
            var result = _sut.Save(_employee.Id, _employee);

            // Assert
            _mockEmployeeRepository.Verify(e => e.Retrieve(_employee.Id), Times.Once);
            _mockEmployeeRepository.Verify(e => e.Update(_existingEmployeeId, _employee), Times.Once);

        }

        [TestMethod]
        public void Save_WithValidData_ReturnsNewEmployeeWithEmployeeId()
        {
            // Arrange
            _mockEmployeeRepository.Setup(c => c.Create(_employee))
                .Callback(() => _employee.Id = Guid.NewGuid())
                .Returns(_employee);

            // Act
            var newEmployee = _sut.Save(_employee.Id, _employee);

            // Assert
            Assert.IsTrue(newEmployee.Id != Guid.Empty);
        }

        [TestMethod]
        public void Save_WithBlankFirstName_ThrowsInvalidNameException()
        {
            _employee.Firstname = string.Empty;

            Assert.ThrowsException<NameRequiredException>(
                () => _sut.Save(_employee.Id, _employee));

            _mockEmployeeRepository.Verify(e => e.Create(_employee), Times.Never);
        }

        [TestMethod]
        public void Save_WithBlankLastName_ThrowsInvalidNameException()
        {
            _employee.Lastname = string.Empty;

            Assert.ThrowsException<NameRequiredException>(
                () => _sut.Save(_employee.Id, _employee));

            _mockEmployeeRepository.Verify(e => e.Create(_employee), Times.Never);
        }

        [TestMethod]
        public void Save_WithBlankMobilePhone_ThrowsContactNumberRequiredException()
        {
            _employee.MobilePhone = string.Empty;

            Assert.ThrowsException<ContactNumberRequiredException>(
                () => _sut.Save(_employee.Id, _employee));

            _mockEmployeeRepository.Verify(e => e.Create(_employee), Times.Never);
        }

        [TestMethod]
        public void Save_MinimumNumberOfMobilePhone_ThrowsContactNumberMinimumLengthException()
        {
            _employee.MobilePhone = "092933";

            Assert.ThrowsException<ContactNumberMinimumLength>(
                () => _sut.Save(_employee.Id, _employee));

            _mockEmployeeRepository.Verify(e => e.Create(_employee), Times.Never);
        }

        [DataTestMethod]
        [DataRow("mmendezblastasia.com")]
        [DataRow("mmendezblastasia.com")]
        [DataRow("mmendez.blastasia.com")]
        public void Save_InvalidEmailFormat_ThrowsInvalidEmailFormatException(string email)
        {

            _employee.EmailAddress = email;
            // Assert
            Assert.ThrowsException<InvalidEmailFormatException>(
                () => _sut.Save(_employee.Id, _employee));
            _mockEmployeeRepository.Verify(e => e.Create(_employee), Times.Never);
        }

        //[TestMethod]
        public void Save_WithNullPhoto_ThrowsEmployeeImageException()
        {
            //_employee.Photo = Stream.Null;

            Assert.ThrowsException<EmployeeImageException>(
                () => _sut.Save(_employee.Id, _employee));
            _mockEmployeeRepository.Verify(e => e.Create(_employee), Times.Never);
        }

        [TestMethod]
        public void Save_WithBlankOfficePhone_ThrowsContactNumberRequiredException()
        {
            _employee.OfficePhone = string.Empty;

            Assert.ThrowsException<ContactNumberRequiredException>(
                () => _sut.Save(_employee.Id, _employee));

            _mockEmployeeRepository.Verify(e => e.Create(_employee), Times.Never);
        }

        [TestMethod]
        public void Save_MinimumNumberOfOfficePhone_ThrowsContactNumberMinimumLengthException()
        {
            _employee.OfficePhone = "123456";

            Assert.ThrowsException<ContactNumberMinimumLength>(
                () => _sut.Save(_employee.Id, _employee));

            _mockEmployeeRepository.Verify(e => e.Create(_employee), Times.Never);
        }

        [TestMethod]
        public void Save_WithBlankExtension_ThrowsExtensionNumberException()
        {
            _employee.Extension = string.Empty;

            Assert.ThrowsException<ExtensionNumberException>(
                () => _sut.Save(_employee.Id, _employee));

            _mockEmployeeRepository.Verify(e => e.Create(_employee), Times.Never);
        }
    }
}
