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
        private EmployeeService sut;
        private Guid nonExistingEmployeeId;
        private Guid existingEmployeeId;

        [TestInitialize]
        public void Initialize()
        {
            //Arrange
            employee = new Employee
            {
                FirstName = "Jasmin",
                LastName = "Magdaleno",
                MobilePhone = "09057002880",
                EmailAddress = "jasminmagdaleno@blastasia.com",
                Photo = new MemoryStream(),
                OfficePhone = "5551212",
                Extension = "1"
            };

            mockEmployeeRepository = new Mock<IEmployeeRepository>();
            sut = new EmployeeService(mockEmployeeRepository.Object);

            nonExistingEmployeeId = Guid.Empty;
            existingEmployeeId = Guid.NewGuid();
        }

        [TestCleanup]
        public void CleanUp()
        {

        }

        [TestMethod]
        public void Save_NewEmployeeWithValidEmployeeData_ShouldCallRepositoryCreate()
        {
            //Arrange

            //Act
            var result = sut.Save(employee.EmployeeId,employee);

            //Assert
            mockEmployeeRepository
                .Verify(e => e.Retrieve(nonExistingEmployeeId), Times.Once());

            mockEmployeeRepository
                .Verify(e => e.Create(employee), Times.Once());
        }

        [TestMethod]
        public void Save_EmployeeWithExistingData_ShouldCallRepositoryUpdate()
        {
            //Arrange
            mockEmployeeRepository
                .Setup(e => e.Retrieve(existingEmployeeId))
                .Returns(employee);

            employee.EmployeeId = existingEmployeeId;

            //Act
            var result = sut.Save(employee.EmployeeId, employee);

            //Assert
            mockEmployeeRepository
                .Verify(e => e.Retrieve(existingEmployeeId), Times.Once());

            mockEmployeeRepository
                .Verify(e => e.Update(existingEmployeeId, employee), Times.Once());
        }

        [TestMethod]
        public void Save_WithValidEmployeeData_ReturnsNewEmployeeWithEmployeeID()
        {
            //Arrange
            mockEmployeeRepository
                .Setup(e => e.Create(employee))
                .Callback(() => employee.EmployeeId = Guid.NewGuid())
                .Returns(employee);

            //Act
            var newEmployee = sut.Save(employee.EmployeeId, employee);

            //Assert
            Assert.IsTrue(newEmployee.EmployeeId != Guid.Empty);
        }

        [TestMethod]
        public void Save_WithBlankFirstName_ThrowsNameRequiredException()
        {
            //Arrange
            employee.FirstName = "";

            //Assert
            Assert.ThrowsException<NameRequiredException>(
                () => sut.Save(employee.EmployeeId, employee));

            mockEmployeeRepository
                .Verify(c => c.Create(employee), Times.Never());
        }

        [TestMethod]
        public void Save_WithBlankLastName_ThrowsNameRequiredException()
        {
            //Arrange
            employee.LastName = "";

            //Assert
            Assert.ThrowsException<NameRequiredException>(
                () => sut.Save(employee.EmployeeId, employee));

            mockEmployeeRepository
                .Verify(c => c.Create(employee), Times.Never());
        }

        [TestMethod]
        public void Save_WithBlankMobilePhoneNumber_ThrowsPhoneNumberRequiredException()
        {
            //Arrange
            employee.MobilePhone = "";

            //Assert
            Assert.ThrowsException<PhoneNumberRequiredException>(
                () => sut.Save(employee.EmployeeId, employee));

            mockEmployeeRepository
                .Verify(c => c.Create(employee), Times.Never());
        }

        [TestMethod]
        public void Save_WithBlankEmailAddress_ThrowsEmailAddressRequiredException()
        {
            //Arrange
            employee.EmailAddress = "";

            //Assert
            Assert.ThrowsException<EmailAddressRequiredException>(
                () => sut.Save(employee.EmployeeId, employee));

            mockEmployeeRepository
                .Verify(c => c.Create(employee), Times.Never());
        }

        //[TestMethod]
        //public void Save_WithBlankPhoto_ThrowsPhotoRequiredException()
        //{
        //    //Arrange
        //    employee.Photo = null;

        //    //Assert
        //    Assert.ThrowsException<PhotoRequiredException>(
        //        () => sut.Save(employee.EmployeeId, employee));

        //    mockEmployeeRepository
        //        .Verify(c => c.Create(employee), Times.Never());
        //}

        [TestMethod]
        public void Save_WithBlankOfficePhone_ThrowsPhoneNumberRequiredException()
        {
            //Arrange
            employee.OfficePhone = "";

            //Assert
            Assert.ThrowsException<PhoneNumberRequiredException>(
                () => sut.Save(employee.EmployeeId, employee));

            mockEmployeeRepository
                .Verify(c => c.Create(employee), Times.Never());
        }

        [TestMethod]
        public void Save_WithBlankExtension_ThrowsExtensionRequiredException()
        {
            //Arrange
            employee.Extension = "";

            //Assert
            Assert.ThrowsException<ExtensionRequiredException>(
                () => sut.Save(employee.EmployeeId, employee));

            mockEmployeeRepository
                .Verify(c => c.Create(employee), Times.Never());
        }

        [TestMethod]
        public void Save_WithInvalidEmailAddress_ThrowsEmailAddressRequiredException()
        {
            //Arrange
            employee.EmailAddress = "jmagdalenoblastasiacom";

            //Assert
            Assert.ThrowsException<EmailAddressRequiredException>(
                    () => sut.Save(employee.EmployeeId, employee));

            mockEmployeeRepository
                .Verify(c => c.Create(employee), Times.Never());
        }
    }
}
