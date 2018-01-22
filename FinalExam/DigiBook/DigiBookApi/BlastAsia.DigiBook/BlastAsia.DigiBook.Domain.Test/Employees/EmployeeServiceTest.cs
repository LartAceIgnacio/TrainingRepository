
using BlastAsia.DigiBook.Domain.Employees;
using BlastAsia.DigiBook.Domain.Models.Employees;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Test.Employees
{
    [TestClass]
    public class EmployeeServiceTest
    {
        private Mock<IEmployeeRepository> mockEmployeeRepository;
        private Employee employee;
        private EmployeeService sut;
        private Guid existingEmployeeId = Guid.NewGuid();
        private Guid nonExistingEmployeeId = Guid.Empty;

        [TestInitialize]
        public void InitializeTest()
        {
            employee = new Employee
            {
                EmployeeId = new Guid(),
                FirstName = "Duane",
                LastName = "De Guzman",
                MobilePhone = "09158959384",
                EmailAddress = "deguzmanduane@gmail.com",
                Photo = "duane.jpg",
                OfficePhone = "09123456790",
                Extension = "101"

            };

            mockEmployeeRepository = new Mock<IEmployeeRepository>();
            sut = new EmployeeService(mockEmployeeRepository.Object);
           
            mockEmployeeRepository
                .Setup(e => e.Retrieve(existingEmployeeId))
                .Returns(employee);
            mockEmployeeRepository
               .Setup(e => e.Retrieve(nonExistingEmployeeId))
               .Returns<Employee>(null);

        }

        [TestCleanup]
        public void CleanupTest()
        {

        }

        [TestMethod]
        public void Save_NewEmployeeWithValidData_ShouldCallRepositoryCreate()
        {
            // Act
            var result = sut.Save(employee.EmployeeId, employee);

            // Assert
            mockEmployeeRepository
                .Verify(e => e.Retrieve(nonExistingEmployeeId), Times.Once);
            mockEmployeeRepository
                .Verify(e => e.Create(employee), Times.Once);
        }

        [TestMethod]
        public void Save_WithExistingEmployee_ShouldCallRepositoryUpdate()
        {
            // Arrange
            employee.EmployeeId = existingEmployeeId;

            // Act
            sut.Save(employee.EmployeeId, employee);

            // Assert
            mockEmployeeRepository
                .Verify(e => e.Retrieve(existingEmployeeId), Times.Once);
            mockEmployeeRepository
                .Verify(e => e.Update(existingEmployeeId, employee), Times.Once);
        }

        [TestMethod]
        public void Save_EmployeeWithValidData_ReturnsNewEmployeeWithEmployeeId()
        {
            // Arrange
            mockEmployeeRepository
                .Setup(e => e.Create(employee))
                .Callback(() => employee.EmployeeId = Guid.NewGuid())
                .Returns(employee);

            // Act
             sut.Save(employee.EmployeeId, employee);

            // Assert
            Assert.IsTrue(employee.EmployeeId != Guid.Empty);
        }

        [TestMethod]
        public void Save_WithBlankFirstName_ThrowsNameRequiredException()
        {
            // Arrange
            employee.FirstName = "";

            // Assert
            mockEmployeeRepository
                .Verify(e => e.Create(employee), Times.Never);
            Assert.ThrowsException<NameRequiredException>(
                () => sut.Save(employee.EmployeeId, employee));
        }

        [TestMethod]
        public void Save_WithBlankLastName_ThrowsNameRequiredException()
        {
            // Arrange
            employee.LastName = "";
            // Assert
            mockEmployeeRepository
                .Verify(e => e.Create(employee), Times.Never);
            Assert.ThrowsException<NameRequiredException>(
                () => sut.Save(employee.EmployeeId, employee));
        }

        [TestMethod]
        public void Save_WithBlankMobilePhone_ThrowsMobilePhoneRequiredException()
        {
            // Arrange
            employee.MobilePhone = "";
            // Assert
            mockEmployeeRepository
                .Verify(e => e.Create(employee), Times.Never);
            Assert.ThrowsException<MobilePhoneRequiredException>(
                () => sut.Save(employee.EmployeeId, employee));
        }

        [DataTestMethod]
        [DataRow("deguzmanduane.com")]
        [DataRow("deguzmanduane@gmail..com")]
        [DataRow("deguzmanduane@gmail.com.")]
        [DataRow("deguzmanduane@@gmail.com")]
        [DataRow("deguzmanduane@gmailcom")]
        [TestMethod]
        public void Save_WithInvalidEmailAddress_ThrowsInvalidEmailRequiredException(string EmailAddress)
        {
            // Arrange
            employee.EmailAddress = EmailAddress;
            
            // Assert
            mockEmployeeRepository
                .Verify(e => e.Create(employee), Times.Never);
            Assert.ThrowsException<InvalidEmailAddressException>(
                () => sut.Save(employee.EmployeeId, employee));
        }

        //[TestMethod]
        //public void Save_WithBlankPhoto_ThrowsPhotoRequiredException()
        //{
        //    // Arrange
        //    employee.Photo = "";
        //    // Assert
        //    mockEmployeeRepository
        //        .Verify(e => e.Create(employee), Times.Never);
        //    Assert.ThrowsException<PhotoRequiredException>(
        //        () => sut.Save(employee.EmployeeId, employee));
        //}

        [TestMethod]
        public void Save_WithBlankOfficePhone_ThrowsOfficePhoneRequiredException()
        {
            // Arrange
            employee.OfficePhone = "";
            // Assert
            mockEmployeeRepository
                .Verify(e => e.Create(employee), Times.Never);
            Assert.ThrowsException<OfficePhoneRequiredException>(
                () => sut.Save(employee.EmployeeId, employee));
        }

        [TestMethod]
        public void Save_WithBlankExtension_ThrowsExtenstionRequiredException()
        {
            // Arrange
            employee.Extension = "";
            // Assert
            mockEmployeeRepository
                .Verify(e => e.Create(employee), Times.Never);
            Assert.ThrowsException<ExtensionRequiredException>(
                () => sut.Save(employee.EmployeeId, employee));
        }
    }
}
