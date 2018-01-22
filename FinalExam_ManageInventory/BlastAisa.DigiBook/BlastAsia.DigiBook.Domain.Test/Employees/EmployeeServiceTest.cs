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
                EmployeeId = new Guid(),
                FirstName = "John Karl",
                LastName = "Matencio",
                MobilePhone = "09957206817",
                EmailAddress = "jhnkrl15@gmail.com",
                Photo = new MemoryStream(),
                OfficePhone = "4848766",
                Extension = "1001"
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
            var result = sut.Save(employee.EmployeeId, employee);
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
                .Callback(() => employee.EmployeeId = Guid.NewGuid())
                .Returns(employee);


            mockEmployeeRepository
                .Setup(c => c.Retrieve(existingEmployeeId))
                .Returns(employee);

            //Act
            var newEmployee = sut.Save(employee.EmployeeId, employee);
            //Assert
            Assert.IsTrue(newEmployee.EmployeeId  != Guid.Empty);
        }

        [TestMethod]
        public void Save_WithExistingEmployee_CallsRepositoryUpdate()
        {
            // Arrange
            employee.EmployeeId = existingEmployeeId;
            // act
            sut.Save(employee.EmployeeId, employee);
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
            employee.FirstName = "";
            //Act

            //Assert
            Assert.ThrowsException<NameRequiredException>(
                () => sut.Save(employee.EmployeeId, employee));
            mockEmployeeRepository
                .Verify(e => e.Create(employee), Times.Never);
        }

        [TestMethod]
        public void Save_WithBlankLastName_ThrowsNameRequiredException()
        {
            //Arrange
            employee.LastName = "";
            //Act

            //Assert
            Assert.ThrowsException<NameRequiredException>(
                () => sut.Save(employee.EmployeeId, employee));
            mockEmployeeRepository
                .Verify(e => e.Create(employee), Times.Never);
        }

        [TestMethod]
        public void Save_WithBlankMobilePhone_ThrowsMobilePhoneRequiredException()
        {
            //Arrange
            employee.MobilePhone = "";
            //Act

            //Assert
            Assert.ThrowsException<MobilePhoneRequiredException>(
                () => sut.Save(employee.EmployeeId, employee));
            mockEmployeeRepository
                .Verify(e => e.Create(employee), Times.Never);
        }

        [TestMethod]
        public void Save_WithBlankEmailAddress_ThrowsEmailAddressRequiredException()
        {
            //Arrange
            employee.EmailAddress = "";
            //Act

            //Assert
            Assert.ThrowsException<EmailAddressRequiredException>(
                () => sut.Save(employee.EmployeeId, employee));
            mockEmployeeRepository
                .Verify(e => e.Create(employee), Times.Never);
        }
        
        //[TestMethod]
        //public void Save_WithBlankPhoto_ThrowsPhotoRequiredException()
        //{
        //    //Arrange
        //    employee.photo = null;
        //    //Act

        //    //Assert
        //    Assert.ThrowsException<PhotoRequiredException>(
        //        () => sut.Save(employee.employeeId, employee));
        //    mockEmployeeRepository
        //        .Verify(e => e.Create(employee), Times.Never);
        //}

        [TestMethod]
        public void Save_WithBlankOfficePhone_ThrowsOfficePhoneRequired()
        {
            //Arrange
            employee.OfficePhone = "";
            //Act

            //Assert
            Assert.ThrowsException<OfficePhoneRequiredException>(
                () => sut.Save(employee.EmployeeId, employee));
            mockEmployeeRepository
                .Verify(e => e.Create(employee), Times.Never);
        }

        [TestMethod]
        public void Save_WithBlankExtension_ThrowsExtensionRequired()
        {
            //Arrange
            employee.Extension = "";
            //Act

            //Assert
            Assert.ThrowsException<ExtensionRequiredException>(
                () => sut.Save(employee.EmployeeId, employee));
            mockEmployeeRepository
                .Verify(e => e.Create(employee), Times.Never);
        }

        [TestMethod]
        public void Save_WithInvalidMobilePhone_ThrowsNumbersOnlyException()
        {
            //Arrange
            employee.MobilePhone = "abc";
            //Act

            //Assert
            Assert.ThrowsException<NumbersOnlyException>(
                () => sut.Save(employee.EmployeeId, employee));
            mockEmployeeRepository
                .Verify(c => c.Create(employee), Times.Never);
        }

        [TestMethod]
        public void Save_WithInvalidOfficePhone_ThrowsNumbersOnlyException()
        {
            //Arrange
            employee.OfficePhone = "abc";
            //Act

            //Assert
            Assert.ThrowsException<NumbersOnlyException>(
                () => sut.Save(employee.EmployeeId, employee));
            mockEmployeeRepository
                .Verify(c => c.Create(employee), Times.Never);
        }

        [TestMethod]
        public void Save_WithInvalidExtension_ThrowsNumbersOnlyException()
        {
            //Arrange
            employee.Extension = "abc";
            //Act

            //Assert
            Assert.ThrowsException<NumbersOnlyException>(
                () => sut.Save(employee.EmployeeId, employee));
            mockEmployeeRepository
                .Verify(c => c.Create(employee), Times.Never);
        }
    }
}
