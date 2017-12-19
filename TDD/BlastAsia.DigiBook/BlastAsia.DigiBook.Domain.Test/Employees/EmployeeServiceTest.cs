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
        private EmployeeService sut;
        private Employee employee;
        private Guid existingEmployeeId;
        private Guid nonExistingEmployeeId;

        [TestInitialize]
        public void EmployeesInitialize()
        {
            employee = new Employee
            {
                FirstName = "Gelo",
                LastName = "Celis",
                MobilePhone = "09279553255",
                EmailAddress = "anjacelis21@gmail.com",
                Photo = "Photo",
                OfficePhone = "8569521",
                Extension = "09"

            };

            existingEmployeeId = Guid.NewGuid();
            nonExistingEmployeeId = Guid.Empty;


            mockEmployeeRepository = new Mock<IEmployeeRepository>();
            sut = new EmployeeService(mockEmployeeRepository.Object);

           
        }

        

        [TestMethod]
        public void Save_NewEmployeesWithValidData_ShouldCallRepositoryCreate()
        {
            //Arrange
            mockEmployeeRepository
               .Setup(e => e.Retrieve(nonExistingEmployeeId))
               .Returns<Employee>(null);

            employee.EmployeeId = nonExistingEmployeeId;

            //Act
            var result = sut.Save(employee.EmployeeId, employee);

            //Assert
            mockEmployeeRepository
                .Verify(e => e.Retrieve(employee.EmployeeId), Times.Once());

            mockEmployeeRepository
                .Verify(e => e.Create(employee), Times.Once());



        }

        [TestMethod]
        public void Save_WithExistingEmployees_ShouldCallRepositoryUpdate()
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
                .Verify(e => e.Retrieve(employee.EmployeeId), Times.Once());


            mockEmployeeRepository
               .Verify(e => e.Update(employee.EmployeeId, employee), Times.Once());

        }

        [TestMethod]
        public void Save_WithValidData_ReturnsNewContactWithContactId()
        {
            //Arrange


            mockEmployeeRepository
                .Setup(e => e.Create(employee))
                .Callback(() => {
                    employee.EmployeeId = Guid.NewGuid();
                })
                .Returns(employee);

            //Act
            var newContact = sut.Save(employee.EmployeeId, employee);

            //Assert


            Assert.IsTrue(newContact.EmployeeId != Guid.Empty);

        }

        [TestMethod]
        public void Save_WithBlankFirstName_ThrowsFirstNameRequiredException()
        {
            //Arrange
            employee.FirstName = "";

            //Assert
            Assert.ThrowsException<FirstNameRequiredException>(
                () => sut.Save(employee.EmployeeId, employee)
                );

            mockEmployeeRepository
                .Verify(e => e.Create(employee), Times.Never());
        }
        [TestMethod]
        public void Save_WithBlankLastName_ThrowsLastNameRequiredException()
        {
            //Arrange
            employee.LastName = "";

            //Assert
            Assert.ThrowsException<LastNameRequiredException>(
                () => sut.Save(employee.EmployeeId, employee)
                );

            mockEmployeeRepository
                .Verify(e => e.Create(employee), Times.Never());

        }
        [TestMethod]
        public void Save_WithBlankMobilePhone_ThrowsMobilePhoneRequiredException()
        {
            //Arrange
            employee.MobilePhone = "";

            //Assert
            Assert.ThrowsException<MobilePhoneRequiredException>(
                () => sut.Save(employee.EmployeeId, employee)
                );

            mockEmployeeRepository
                .Verify(e => e.Create(employee), Times.Never());

        }

        [TestMethod]
        public void Save_WithBlankEmailAddress_ThrowsEmailAddressRequiredException()
        {
            //Arrange
            employee.EmailAddress = "";

            //Assert
            Assert.ThrowsException<EmailAddressRequiredException>(
                () => sut.Save(employee.EmployeeId, employee)
                );

            mockEmployeeRepository
                .Verify(e => e.Create(employee), Times.Never());

        }

        [DataTestMethod]
        [DataRow("anjacelis21gmailcom")]
        [DataRow("angelou21@gmail")]
        [DataRow("angelou21.gmail.com")]
        public void Save_WithValidEmailAddress_ThrowsValidEmailAddressRequiredException(string email)
        {
            //Arrange
            employee.EmailAddress = email;

            //Assert
            Assert.ThrowsException<ValidEmailAddressRequiredException>(
                () => sut.Save(employee.EmployeeId, employee)
                );

            mockEmployeeRepository
                .Verify(e => e.Create(employee), Times.Never());

        }

        [TestMethod]
        public void Save_WithBlankPhoto_ThrowsPhotoRequiredException()
        {
            //Arrange
            employee.Photo = "";

            //Assert
            Assert.ThrowsException<PhotoRequiredException>(
                () => sut.Save(employee.EmployeeId, employee)
                );

            mockEmployeeRepository
                .Verify(e => e.Create(employee), Times.Never());

        }

        [TestMethod]
        public void Save_WithBlankOfficePhone_ThrowsOfficePhoneRequiredException()
        {
            //Arrange
            employee.OfficePhone = "";

            //Assert
            Assert.ThrowsException<OfficePhoneRequiredException>(
                () => sut.Save(employee.EmployeeId, employee)
                );

            mockEmployeeRepository
                .Verify(e => e.Create(employee), Times.Never());

        }

        [TestMethod]
        public void Save_WithBlankExtension_ThrowsExtensionRequiredException()
        {
            //Arrange
            employee.Extension = "";

            //Assert
            Assert.ThrowsException<ExtensionRequiredException>(
                () => sut.Save(employee.EmployeeId, employee)
                );

            mockEmployeeRepository
                .Verify(e => e.Create(employee), Times.Never());

        }

    }
}
