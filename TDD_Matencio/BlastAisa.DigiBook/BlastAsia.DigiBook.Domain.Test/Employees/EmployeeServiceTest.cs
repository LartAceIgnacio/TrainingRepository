using BlastAsia.DigiBook.Domain.Employees;
using BlastAsia.DigiBook.Domain.Models.Employees;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Test.Employees
{
    [TestClass]
    public class EmployeeServiceTest
    {
        private Employee employee;
        private Mock<IEmployeeRepository> mockEmployeeRepository;
        private Guid existingEmployeeId = Guid.NewGuid();
        private Guid nonExistingEmployeeId = Guid.Empty;
        EmpolyeeService sut;

        [TestInitialize]
        public void TestInitialize()
        {
            employee = new Employee
            {
                employeeId = new Guid(),
                firstName = "John Karl",
                lastName = "Matencio",
                mobilePhone = "09957206817",
                emailAddress = "jhnkrl15@gmail.com",
                photo = new MemoryStream(),
                officePhone = "4848766",
                extension = "1001"
            };
            mockEmployeeRepository = new Mock<IEmployeeRepository>();
            sut = new EmpolyeeService(mockEmployeeRepository.Object);

            mockEmployeeRepository
                .Setup(c => c.Create(employee))
                .Returns(employee);

            mockEmployeeRepository
                .Setup(c => c.Retrieve(existingEmployeeId))
                .Returns(employee);

            mockEmployeeRepository
               .Setup(c => c.Retrieve(nonExistingEmployeeId))
                   .Returns<Employee>(null);
        }

        [TestCleanup]
        public void CleanupTest()
        {

        }

        [TestMethod]
        public void Save_NewEmployeeWithValidData_ShouldCallRepositoryCreate()
        {
            //Arrange
            //Act
            var result = sut.Save(employee);
            //Assert  
            mockEmployeeRepository
                .Verify(e => e.Retrieve(nonExistingEmployeeId), Times.Once);
            mockEmployeeRepository
               .Verify(e => e.Create(employee), Times.Once());
        }

        [TestMethod]
        public void Save_WithValidData_ReturnsNewEmployeeWithEmployeeId()
        {
            //Arrange
            mockEmployeeRepository
                .Setup(c => c.Create(employee))
                .Callback(() => employee.employeeId = Guid.NewGuid())
                .Returns(employee);


            mockEmployeeRepository
                .Setup(c => c.Retrieve(existingEmployeeId))
                .Returns(employee);

            //Act
            var newEmployee = sut.Save(employee);
            //Assert
            Assert.IsTrue(newEmployee.employeeId  != Guid.Empty);
        }

        [TestMethod]
        public void Save_WithExistingEmployee_CallsRepositoryUpdate()
        {
            // Arrange
            employee.employeeId = existingEmployeeId;
            // act
            sut.Save(employee);
            // assert
            mockEmployeeRepository
                .Verify(e => e.Retrieve(existingEmployeeId), Times.Once);

            mockEmployeeRepository
                .Verify(e => e.Update(existingEmployeeId, employee), Times.Once);
        }

        [TestMethod]
        public void Save_WithBlankFirstName_ThrowsNameRequiredException()
        {
            //Arrange
            employee.firstName = "";
            //Act

            //Assert
            Assert.ThrowsException<NameRequiredException>(
                () => sut.Save(employee));
            mockEmployeeRepository
                .Verify(e => e.Create(employee), Times.Never);
        }

        [TestMethod]
        public void Save_WithBlankLastName_ThrowsNameRequiredException()
        {
            //Arrange
            employee.lastName = "";
            //Act

            //Assert
            Assert.ThrowsException<NameRequiredException>(
                () => sut.Save(employee));
            mockEmployeeRepository
                .Verify(e => e.Create(employee), Times.Never);
        }

        [TestMethod]
        public void Save_WithBlankMobilePhone_ThrowsMobilePhoneRequiredException()
        {
            //Arrange
            employee.mobilePhone = "";
            //Act

            //Assert
            Assert.ThrowsException<MobilePhoneRequiredException>(
                () => sut.Save(employee));
            mockEmployeeRepository
                .Verify(e => e.Create(employee), Times.Never);
        }

        [TestMethod]
        public void Save_WithBlankEmailAddress_ThrowsEmailAddressRequiredException()
        {
            //Arrange
            employee.emailAddress = "";
            //Act

            //Assert
            Assert.ThrowsException<EmailAddressRequiredException>(
                () => sut.Save(employee));
            mockEmployeeRepository
                .Verify(e => e.Create(employee), Times.Never);
        }
        
        [TestMethod]
        public void Save_WithBlankPhoto_ThrowsPhotoRequiredException()
        {
            //Arrange
            employee.photo = null;
            //Act

            //Assert
            Assert.ThrowsException<PhotoRequiredException>(
                () => sut.Save(employee));
            mockEmployeeRepository
                .Verify(e => e.Create(employee), Times.Never);
        }

        [TestMethod]
        public void Save_WithBlankOfficePhone_ThrowsOfficePhoneRequired()
        {
            //Arrange
            employee.officePhone = "";
            //Act

            //Assert
            Assert.ThrowsException<OfficePhoneRequiredException>(
                () => sut.Save(employee));
            mockEmployeeRepository
                .Verify(e => e.Create(employee), Times.Never);
        }

        [TestMethod]
        public void Save_WithBlankExtension_ThrowsExtensionRequired()
        {
            //Arrange
            employee.extension = "";
            //Act

            //Assert
            Assert.ThrowsException<ExtensionRequiredException>(
                () => sut.Save(employee));
            mockEmployeeRepository
                .Verify(e => e.Create(employee), Times.Never);
        }

        [TestMethod]
        public void Save_WithInvalidMobilePhone_ThrowsNumbersOnlyException()
        {
            //Arrange
            employee.mobilePhone = "abc";
            //Act

            //Assert
            Assert.ThrowsException<NumbersOnlyException>(
                () => sut.Save(employee));
            mockEmployeeRepository
                .Verify(c => c.Create(employee), Times.Never);
        }

        [TestMethod]
        public void Save_WithInvalidOfficePhone_ThrowsNumbersOnlyException()
        {
            //Arrange
            employee.officePhone = "abc";
            //Act

            //Assert
            Assert.ThrowsException<NumbersOnlyException>(
                () => sut.Save(employee));
            mockEmployeeRepository
                .Verify(c => c.Create(employee), Times.Never);
        }

        [TestMethod]
        public void Save_WithInvalidExtension_ThrowsNumbersOnlyException()
        {
            //Arrange
            employee.extension = "abc";
            //Act

            //Assert
            Assert.ThrowsException<NumbersOnlyException>(
                () => sut.Save(employee));
            mockEmployeeRepository
                .Verify(c => c.Create(employee), Times.Never);
        }
    }
}
