using BlastAsia.DigiBook.Domain.Employees;
using BlastAsia.DigiBook.Domain.Employees.Exceptions;
using BlastAsia.DigiBook.Domain.Models.Employees;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.IO;

namespace BlastAsia.DigiBook.Domain.Test.Employees
{
    [TestClass]
    public class EmployeeServiceTest
    {
        private Mock<IEmployeeRepository> mockEmployeeRepository;
        EmployeeService sut;
        private Employee employee;
        private Guid existingEmployeeId = Guid.NewGuid();
        private Guid nonExistingEmployeeId = Guid.Empty;

        [TestInitialize]
        public void initializeTest()
        {
            mockEmployeeRepository = new Mock<IEmployeeRepository>();
            sut = new EmployeeService(mockEmployeeRepository.Object);
            employee = new Employee
            {
                FirstName = "Luigi Antonio",
                LastName = "Abille",
                MobileNumber = "09568717617",
                EmailAddress = "luigiabille@gmail.com",
                Photo = new MemoryStream(),
                OfficePhone = "9111111",
                Extension = "8080"
            };
            mockEmployeeRepository
                .Setup(c => c.Retrieve(existingEmployeeId))
                .Returns(employee);
            mockEmployeeRepository
                .Setup(c => c.Retrieve(nonExistingEmployeeId))
                .Returns<Employee>(null);
        }

        [TestCleanup]
        public void testCleanup()
        {

        }

        [TestMethod]
        public void Create_NewEmployeeWithValidData_ShouldCallRepositoryCreate()
        {
            // Arrange

            // Act

            var result = sut.Create(employee);

            // Assert

            mockEmployeeRepository
                .Verify(c => c.Retrieve(nonExistingEmployeeId),
                Times.Once());
            mockEmployeeRepository
                .Verify(c => c.Create(employee),
                Times.Once());
        }
        [TestMethod]
        public void Create_WithExostingEmployee_CallsRepositoryUpdate()
        {
            // Arrange

            employee.EmployeeId = existingEmployeeId;

            // Act

            sut.Create(employee);

            // Assert

            mockEmployeeRepository
                .Verify(c => c.Retrieve(employee.EmployeeId),
                Times.Once);
            mockEmployeeRepository
                .Verify(c => c.Update(existingEmployeeId, employee),
                Times.Once);
        }
        [TestMethod]
        public void Create_WithValidData_ReturnsNewEmployeeWithEmployeeId()
        {
            // Arrange

            mockEmployeeRepository
                .Setup(c => c.Create(employee))
                .Callback(() => employee.EmployeeId = Guid.NewGuid())
                .Returns(employee);

            // Act

            var newEmployee = sut.Create(employee);

            // Assert

            Assert.IsTrue(
                newEmployee.EmployeeId != Guid.Empty);
        }
        [TestMethod]
        public void Create_WithBlankFirstName_ThrowsNameRequiredException()
        {
            // Arrange

            employee.FirstName = "";

            // Act

            // Assert

            Assert.ThrowsException<NameRequiredException>(
                () => sut.Create(employee));
            mockEmployeeRepository
                .Verify(c => c.Create(employee), Times.Never());
        }
        [TestMethod]
        public void Create_WithBlankLastName_ThrowsNameRequiredException()
        {
            // Arrange

            employee.LastName = "";

            // Act

            // Assert

            Assert.ThrowsException<NameRequiredException>(
                () => sut.Create(employee));
            mockEmployeeRepository
                .Verify(c => c.Create(employee),
                Times.Never());
        }
        [TestMethod]
        public void Create_WithBlankMobilePhone_ThrowsMobilePhoneRequiredExcecption()
        {
            // Arrange

            employee.MobileNumber = "";

            // Act

            // Assert

            Assert.ThrowsException<MobilePhoneRequiredException>(
                () => sut.Create(employee));
            mockEmployeeRepository
                .Verify(c => c.Create(employee),
                Times.Never());
        }
        [TestMethod]
        public void Create_WithBlankEmailAddress_ThrowsEmailAddressRequiredException()
        {
            // Arrange

            employee.EmailAddress = "";

            // Act

            // Assert

            Assert.ThrowsException<EmailAddressRequiredException>(
                () => sut.Create(employee));
            mockEmployeeRepository
                .Verify(c => c.Create(employee),
                Times.Never());
        }
        [TestMethod]
        public void Create_WithBlankPhoto_ThrowsPhotoRequiredException()
        {
            // Arrange

            employee.Photo = null;

            // Act

            // Assert

            Assert.ThrowsException<PhotoRequiredException>(
                () => sut.Create(employee));
            mockEmployeeRepository
                .Verify(c => c.Create(employee),
                Times.Never());
        }
        [TestMethod]
        public void Create_WithBlankOfficePhone_ThrowsOfficePhoneRequiredException()
        {
            // Arrange

            employee.OfficePhone = "";

            // Act

            // Assert

            Assert.ThrowsException<OfficePhoneRequiredException>(
                () => sut.Create(employee));
            mockEmployeeRepository
                .Verify(c => c.Create(employee),
                Times.Never());
        }
        [TestMethod]
        public void Create_WithBlankExtension_ThrowsExtensionRequiredException()
        {
            // Arrange

            employee.Extension = "";

            // Act

            // Assert

            Assert.ThrowsException<ExtensionRequiredException>(
                () => sut.Create(employee));
            mockEmployeeRepository
                .Verify(c => c.Create(employee),
                Times.Never());
        }
    }
}
