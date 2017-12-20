using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BlastAsia.Digibook.Domain.Models.Employees;
using BlastAsia.Digibook.Domain.Employees;
using System.IO;

namespace BlastAsia.Digibook.Domain.Test.Employees
{
    [TestClass]
    public class EmployeesServiceTest
    {
        private Mock<IEmployeeRepository> mockEmployeeRepository;
        private Employee employee;
        private EmployeeService sut;
        private Guid existingEmployeeId = Guid.NewGuid();
        private Guid nonExistingEmployeeId = Guid.Empty;

        [TestInitialize]
        public void InitializeTest()
        {
            mockEmployeeRepository = new Mock<IEmployeeRepository>();
            sut = new EmployeeService(mockEmployeeRepository.Object);

            employee = new Employee
            {
                FirstName = "Glenn Alexander",
                LastName = "Cano",
                MobilePhone = "09173723594",
                EmailAddress = "gcano@blastasia.com",
                Photo = new MemoryStream(),
                OfficePhone = "9144456",
                Extension = "421"
            };

            mockEmployeeRepository
                .Setup(e => e.Create(employee))
                .Callback(()=>employee.EmployeeId=Guid.NewGuid())
                .Returns(employee);

            mockEmployeeRepository
                .Setup(c => c.Retrieve(existingEmployeeId))
                    .Returns(employee);
            // if non existing
            mockEmployeeRepository
                .Setup(c => c.Retrieve(nonExistingEmployeeId))
                    .Returns<Employee>(null);
        }

        [TestMethod]
        public void Save_NewEmployeeWithValidData_ShouldCallRepositoryCreate()
        {
            var result = sut.Save(employee);

            mockEmployeeRepository
                .Verify(e => e.Retrieve(nonExistingEmployeeId), Times.Once);

            mockEmployeeRepository
                .Verify(e => e.Create(employee), Times.Once);
        }

        [TestMethod]
        public void Save_ExistingEmployeeWithValidData_ShouldCallRepositoryEdit()
        {
            employee.EmployeeId = existingEmployeeId;
            var result = sut.Save(employee);

            mockEmployeeRepository
                .Verify(e => e.Retrieve(employee.EmployeeId), Times.Once);

            mockEmployeeRepository
                .Verify(e => e.Update(employee.EmployeeId,employee), Times.Once);
        }

        [TestMethod]
        public void Save_NewEmployeeWithValidData_ShouldReturnEmployeeId()
        {

            mockEmployeeRepository
                .Setup(e => e.Create(employee))
                .Callback(() => employee.EmployeeId = Guid.NewGuid())
                .Returns(employee);

            sut = new EmployeeService(mockEmployeeRepository.Object);
            var newEmployee = sut.Save(employee);

            Assert.IsTrue(newEmployee.EmployeeId != Guid.Empty);
        }

        [TestMethod]
        public void Save_WithBlankFirstName_ThrowsInvalidNameFormatException()
        {
            employee.FirstName = "";

            Assert.ThrowsException<InvalidNameFormatException>(
                () => sut.Save(employee));
            mockEmployeeRepository
                .Verify(e => e.Create(employee), Times.Never);
        }

        [TestMethod]
        public void Save_WithBlankLastName_ThrowsInvalidNameFormatException()
        {
            employee.LastName = "";
            Assert.ThrowsException<InvalidNameFormatException>(
                () => sut.Save(employee));
            mockEmployeeRepository
                .Verify(e => e.Create(employee), Times.Never);
        }

        [TestMethod]
        public void Save_WithBlankMobilePhone_ThrowsInvalidPhoneFormatException()
        {
            employee.MobilePhone = "";
            Assert.ThrowsException<InvalidPhoneFormatException>(
                () => sut.Save(employee));
            mockEmployeeRepository
                .Verify(e => e.Create(employee), Times.Never);
        }

        [TestMethod]
        public void Save_WithBlankEmailAddress_ThrowsInvalidEmailAddressFormatException()
        {
            employee.EmailAddress = "";
            Assert.ThrowsException<InvalidEmailAddressFormatException>(
                () => sut.Save(employee));
            mockEmployeeRepository
                .Verify(e => e.Create(employee), Times.Never);
        }

        [TestMethod]
        public void Save_WithNoPhoto_ThrowsInvalidPhotoException()
        {
            employee.Photo = null;
            Assert.ThrowsException<InvalidPhotoException>(
                () => sut.Save(employee));
            mockEmployeeRepository
                .Verify(e => e.Create(employee), Times.Never);
        }

        [TestMethod]
        public void Save_WithBlankOfficePhone_ThrowsInvalidPhoneFormatException()
        {
            employee.OfficePhone = "";
            Assert.ThrowsException<InvalidPhoneFormatException>(
                () => sut.Save(employee));
            mockEmployeeRepository
                .Verify(e => e.Create(employee), Times.Never);
        }

        [TestMethod]
        public void Save_WithBlankExtension_ThrowsInvalidExtensionFormatException()
        {
            employee.Extension = "";
            Assert.ThrowsException<InvalidExtensionFormatException>(
                () => sut.Save(employee));
            mockEmployeeRepository
                .Verify(e => e.Create(employee), Times.Never);
        }
    }
}
