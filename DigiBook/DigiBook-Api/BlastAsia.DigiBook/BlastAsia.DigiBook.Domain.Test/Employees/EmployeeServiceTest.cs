using BlastAsia.DigiBook.Domain.Employees;
using BlastAsia.DigiBook.Domain.Employees.Exceptions;
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

        private Guid existingEmployeeId = Guid.NewGuid();
        private Guid nonExistingEmployeeId = Guid.Empty;

        [TestInitialize]
        public void Initialize()
        {
            employee = new Employee
            {
                FirstName = "Eugene",
                LastName = "Ravina",
                MobilePhone = "09277109530",
                EmailAddress = "eravina@blastasia.com",
                Photo = new MemoryStream(),
                OfficePhone = "373-30-32",
                Extension = "Extension",
                IsActive = false,
                DateActivated = new Nullable<DateTime>()
            };
            mockEmployeeRepository = new Mock<IEmployeeRepository>();
            sut = new EmployeeService(mockEmployeeRepository.Object);

            mockEmployeeRepository
             .Setup(c => c.Create(employee))
             .Callback(() => employee.EmployeeId = Guid.NewGuid())
             .Returns(employee);

            mockEmployeeRepository
                .Setup(c => c.Retrieve(existingEmployeeId))
                .Returns(employee);

            mockEmployeeRepository
              .Setup(c => c.Retrieve(nonExistingEmployeeId))
              .Returns<Employee>(null);
        }

        [TestMethod]
        public void Save_NewEmployeeWithValidData_ShouldCallRepositoryCreate()
        {
            // Act
            var result = sut.Save(employee.EmployeeId, employee);

            //Assert
            mockEmployeeRepository
                .Verify(c => c.Retrieve(nonExistingEmployeeId), Times.Once);

            mockEmployeeRepository
                .Verify(c => c.Create(employee), Times.Once());
        }

        [TestMethod]
        public void Save_WithValidData_ReturnsNewEmployeeWithEmployeeId()
        {
            mockEmployeeRepository
               .Setup(c => c.Create(employee))
               .Callback(() => employee.EmployeeId = Guid.NewGuid())
               .Returns(employee);

            mockEmployeeRepository
                .Setup(c => c.Retrieve(existingEmployeeId))
                .Returns(employee);

            mockEmployeeRepository
              .Setup(c => c.Retrieve(nonExistingEmployeeId))
              .Returns<Employee>(null);


            // Act

            var newEmployee = sut.Save(employee.EmployeeId, employee);

            //Assert
            Assert.IsTrue(newEmployee.EmployeeId != Guid.Empty);

        }
        [TestMethod]
        public void Save_WithBlankEmployeeFirstName_ThrowsNameRequiredException()
        {
            // Arrange 
            employee.FirstName = "";

            //Asert
            Assert.ThrowsException<NameRequiredException>(
                () => sut.Save(employee.EmployeeId, employee));

            mockEmployeeRepository
              .Verify(c => c.Create(employee), Times.Never());
        }
        [TestMethod]
        public void Save_WithBlankEmployeeLastName_ThrowsNameRequiredException()
        {
            // Arrange
            employee.LastName = "";

            // Assert
            Assert.ThrowsException<NameRequiredException>(
                () => sut.Save(employee.EmployeeId, employee));

            mockEmployeeRepository
                .Verify(c => c.Create(employee), Times.Never());
        }

        [TestMethod]
        public void Save_WithBlankEmployeeMobilePhone_ThrowsMobilePhoneRequiredException()
        {
            // Arrange
            employee.MobilePhone = "";

            //Assert
            Assert.ThrowsException<MobilePhoneRequiredException>(
                () => sut.Save(employee.EmployeeId, employee));

            mockEmployeeRepository
               .Verify(c => c.Create(employee), Times.Never());
        }

        [TestMethod]
        public void Save_WithBlankEmailAddress_ThrowsEmailAddressRequiredException()
        {
            // Arrange
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
        //    // Arrange
        //    employee.Photo = null;

        //    //Assert
        //    Assert.ThrowsException<PhotoRequiredException>(
        //        () => sut.Save(employee.EmployeeId, employee));

        //    mockEmployeeRepository
        //       .Verify(c => c.Create(employee), Times.Never());
        //}

        [TestMethod]
        public void Save_WithEmployeeOfficePhone_ThrowsOfficePhoneRequiredException()
        {
            // Arrange

            employee.OfficePhone = "";

            //Assert
            Assert.ThrowsException<OfficePhoneRequiredException>(
                () => sut.Save(employee.EmployeeId, employee));

            mockEmployeeRepository
                .Verify(c => c.Create(employee), Times.Never());

        }

        [TestMethod]
        public void Save_WithEmployeeExtension_ThrowsExtensionRequiredException()
        {
            // Arrange

            employee.Extension = "";

            //Assert
            Assert.ThrowsException<ExtensionRequiredException>(
                () => sut.Save(employee.EmployeeId, employee));

            mockEmployeeRepository
                .Verify(c => c.Create(employee), Times.Never());
        }
    }

}
