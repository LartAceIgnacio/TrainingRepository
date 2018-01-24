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
        private EmployeeService sut;
        private Mock<IEmployeeRepository> mockEmployeeRepository;
        private Employee employee;
        private Guid nonExistingEmployeeId;
        private Guid existingEmployeeId;

        [TestInitialize]
        public void Initialize()
        {

            mockEmployeeRepository = new Mock<IEmployeeRepository>();
            sut = new EmployeeService(mockEmployeeRepository.Object);

            employee = new Employee
            {
                Firstname = "Rose",
                Lastname = "Quilicol",
                MobilePhone = "09123456789",
                EmailAddress = "rquilicol@xyz.com",
                //Photo = new MemoryStream(),
                OfficePhone = "1234567",
                Extension = "02"
            };

            nonExistingEmployeeId = Guid.Empty;
            existingEmployeeId = Guid.NewGuid();

            mockEmployeeRepository.Setup(e => e.Retrieve(existingEmployeeId))
                .Returns(employee);

            mockEmployeeRepository.Setup(e => e.Retrieve(nonExistingEmployeeId))
                .Returns<Employee>(null);
            
        }

        [TestCleanup]
        public void CleanUp() { }

        [TestMethod]
        public void Save_NewEmployeeWithValidData_ShouldCallEmployeeRepositoryCreate()
        {
            // Arrange

            // Act
            var result = sut.Save(employee.Id,employee);

            // Assert
            mockEmployeeRepository.Verify(c => c.Retrieve(nonExistingEmployeeId), Times.Once);
            mockEmployeeRepository.Verify(c => c.Create(employee), Times.Once);

        }


        [TestMethod]
        public void Save_WithExistingEmployee_ShouldCallEmployeeRepositoryUpdate()
        {
            // Arrange
            employee.Id = existingEmployeeId;
            // Act
            var result = sut.Save(employee.Id, employee);

            // Assert
            mockEmployeeRepository.Verify(e => e.Retrieve(employee.Id), Times.Once);
            mockEmployeeRepository.Verify(e => e.Update(existingEmployeeId, employee), Times.Once);

        }

        [TestMethod]
        public void Save_WithValidData_ReturnsNewEmployeeWithEmployeeId()
        {
            // Arrange
            mockEmployeeRepository.Setup(c => c.Create(employee))
                .Callback(() => employee.Id = Guid.NewGuid())
                .Returns(employee);

            // Act
            var newEmployee = sut.Save(employee.Id, employee);

            // Assert
            Assert.IsTrue(newEmployee.Id != Guid.Empty);
        }

        [TestMethod]
        public void Save_WithBlankFirstName_ThrowsInvalidNameException()
        {
            employee.Firstname = string.Empty;

            Assert.ThrowsException<NameRequiredException>(
                () => sut.Save(employee.Id, employee));

            mockEmployeeRepository.Verify(e => e.Create(employee), Times.Never);
        }

        [TestMethod]
        public void Save_WithBlankLastName_ThrowsInvalidNameException()
        {
            employee.Lastname = string.Empty;

            Assert.ThrowsException<NameRequiredException>(
                () => sut.Save(employee.Id, employee));

            mockEmployeeRepository.Verify(e => e.Create(employee), Times.Never);
        }

        [TestMethod]
        public void Save_WithBlankMobilePhone_ThrowsContactNumberRequiredException()
        {
            employee.MobilePhone = string.Empty;

            Assert.ThrowsException<ContactNumberRequiredException>(
                () => sut.Save(employee.Id, employee));

            mockEmployeeRepository.Verify(e => e.Create(employee), Times.Never);
        }

        [TestMethod]
        public void Save_MinimumNumberOfMobilePhone_ThrowsContactNumberMinimumLengthException()
        {
            employee.MobilePhone = "092933";

            Assert.ThrowsException<ContactNumberMinimumLength>(
                () => sut.Save(employee.Id, employee));

            mockEmployeeRepository.Verify(e => e.Create(employee), Times.Never);
        }

        [DataTestMethod]
        [DataRow("mmendezblastasia.com")]
        [DataRow("mmendezblastasia.com")]
        [DataRow("mmendez.blastasia.com")]
        public void Save_InvalidEmailFormat_ThrowsInvalidEmailFormatException(string email)
        {

            employee.EmailAddress = email;
            // Assert
            Assert.ThrowsException<InvalidEmailFormatException>(
                () => sut.Save(employee.Id, employee));
            mockEmployeeRepository.Verify(e => e.Create(employee), Times.Never);
        }

        //[TestMethod]
        public void Save_WithNullPhoto_ThrowsEmployeeImageException()
        {
            //_employee.Photo = Stream.Null;

            Assert.ThrowsException<EmployeeImageException>(
                () => sut.Save(employee.Id, employee));
            mockEmployeeRepository.Verify(e => e.Create(employee), Times.Never);
        }

        [TestMethod]
        public void Save_WithBlankOfficePhone_ThrowsContactNumberRequiredException()
        {
            employee.OfficePhone = string.Empty;

            Assert.ThrowsException<ContactNumberRequiredException>(
                () => sut.Save(employee.Id, employee));

            mockEmployeeRepository.Verify(e => e.Create(employee), Times.Never);
        }

        [TestMethod]
        public void Save_MinimumNumberOfOfficePhone_ThrowsContactNumberMinimumLengthException()
        {
            employee.OfficePhone = "123456";

            Assert.ThrowsException<ContactNumberMinimumLength>(
                () => sut.Save(employee.Id, employee));

            mockEmployeeRepository.Verify(e => e.Create(employee), Times.Never);
        }

        [TestMethod]
        public void Save_WithBlankExtension_ThrowsExtensionNumberException()
        {
            employee.Extension = string.Empty;

            Assert.ThrowsException<ExtensionNumberException>(
                () => sut.Save(employee.Id, employee));

            mockEmployeeRepository.Verify(e => e.Create(employee), Times.Never);
        }
    }
}
