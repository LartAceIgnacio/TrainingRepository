using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Moq;
using BlastAsia.DigiBook.Domain.Employees;
using BlastAsia.DigiBook.Domain.Models.Employees;
using BlastAsia.DigiBook.Domain.Contacts;
using BlastAsia.DigiBook.Domain.Contacts.Exception;

namespace BlastAsia.DigiBook.Domain.Test.Employees
{
    [TestClass]
    public class EmployeeServiceTest
    {
        // Global Variables
        private Mock<IEmployeeRepository> mockEmployeeServiceRepository;
        private EmployeeService sut;
        private Employee employee;

        private Guid nonExistingEmployeeId = Guid.Empty;
        private Guid existingEmployeeId = Guid.NewGuid();

        [TestInitialize]
        public void Initialize()
        {
            employee = new Employee
            {
                EmployeeId = new Guid(),
                FirstName = "Emmanuel",
                LastName = "Magadia",
                MobilePhone = "09279528841",
                EmailAddress = "emagadia@blastasia.com",
                Photo = new MemoryStream(),
                OfficePhone = "123-123-123",
                Extension = "asdasd"
            };
         
            mockEmployeeServiceRepository = new Mock<IEmployeeRepository>();

            mockEmployeeServiceRepository
                .Setup(
                    er => er.Create(employee)
                );

            // if existing is used return actual employee
            mockEmployeeServiceRepository
                .Setup(
                    er => er.Retrieve(existingEmployeeId)
                )
               .Returns(employee);
            // if nonexisting is used return null employee
            mockEmployeeServiceRepository
                .Setup(
                    er => er.Retrieve(nonExistingEmployeeId)
                 )
                .Returns<Employee>(null);

            // setup sut
            sut = new EmployeeService(mockEmployeeServiceRepository.Object);

        }
        [TestCleanup]
        public void CleanUp()
        {

        }

        [TestMethod]
        public void Save_EmployeeWithValidData_ShouldReturnEmployeeId()
        {

            mockEmployeeServiceRepository
            .Setup(
                er => er.Create(employee)
            )
            .Callback(
                () =>
                {
                    employee.EmployeeId = Guid.NewGuid();
                }
            )
            .Returns(employee);

            // act
            var newContact = sut.Save(employee.EmployeeId, employee);

            // assert 
            Assert.IsTrue(newContact.EmployeeId != Guid.Empty);

        }

        [TestMethod]
        public void Save_WithExistingEmployeeId_ShouldCallsRepositoryRetrieveAndUpdate()
        {
            // arrange
            employee.EmployeeId = existingEmployeeId;
            // act
            sut.Save(employee.EmployeeId, employee);
            // assert
            // if exist it should call both Retrieve and Update
            mockEmployeeServiceRepository
                .Verify(
                    er => er.Retrieve(existingEmployeeId), Times.Once()
                );

            mockEmployeeServiceRepository
                .Verify(
                    er => er.Update(existingEmployeeId, employee), Times.Once()
                );
        }

        [TestMethod]
        public void Save_WithNoneExistingEmployeeId_ShouldCallsRepositoryRetrieveAndCreate()
        {
            // arrange
            employee.EmployeeId = nonExistingEmployeeId;
            // act
            sut.Save(employee.EmployeeId, employee);
            // assert 
            // if not exist it should call both Retrieve and Create
            mockEmployeeServiceRepository
                .Verify(
                    er => er.Retrieve(nonExistingEmployeeId), Times.Once()
                );

            mockEmployeeServiceRepository
                .Verify(
                    er => er.Create(employee), Times.Once()
                );
        }


        // Validations 

        [DataTestMethod]
        [DataRow("")]
        [DataRow(" ")]
        public void Save_WithBlankFirstName_ShouldThrowNameRequiredException(string FirstName)
        {
            // arrange
            employee.FirstName = FirstName;
            // act
            // assert
            Assert.ThrowsException<NameRequiredException>(
                () => sut.Save(employee.EmployeeId, employee)
                );

            mockEmployeeServiceRepository
                .Verify(
                    er => er.Create(employee), Times.Never()
                );
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow(" ")]
        public void Save_WithBlankLastName_ShouldThrowNameRequiredException(string lastName)
        {
            // arrange
            employee.LastName = lastName;
            // act
            // assert
            Assert.ThrowsException<NameRequiredException>(
                () => sut.Save(employee.EmployeeId, employee)
                );

            mockEmployeeServiceRepository
                .Verify(
                    er => er.Create(employee), Times.Never()
                );
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow(" ")]
        public void Save_WithBlankMobileNumber_ShouldThrowMobilePhoneRequiredException(string mobileNumber)
        {
            // arrange
            employee.MobilePhone = mobileNumber;
            // act
            // assert
            Assert.ThrowsException<MobilePhoneRequiredException>(
                () => sut.Save(employee.EmployeeId, employee)
                );

            mockEmployeeServiceRepository
                .Verify(
                    er => er.Create(employee), Times.Never()
                );
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("emagadiablastasiacom")]
        [DataRow("emagadia@blastasia")]
        [DataRow("emagadia.blastasia.com")]

        public void Save_WithInvalidEmailAddress_ShouldThrowEmailAddressInvalidException(string email)
        {
            // arrange
            employee.EmailAddress = email;
            // act
            // assert
            Assert.ThrowsException<InvalidEmailException>(
                () => sut.Save(employee.EmployeeId, employee)
                );

            mockEmployeeServiceRepository
                .Verify(
                    er => er.Create(employee), Times.Never()
                );
        }

        //[TestMethod]
        //public void Save_WithNullPhoto_ShouldThrowPhotoRequiredException()
        //{
        //    // arrange
        //    employee.Photo = null;
        //    // act
        //    // assert
        //    Assert.ThrowsException<PhotoRequiredException>(
        //        () => sut.Save(employee.EmployeeId, employee)
        //        );

        //    mockEmployeeServiceRepository
        //        .Verify(
        //            er => er.Create(employee), Times.Never()
        //        );
        //}

        [DataTestMethod]
        [DataRow("")]
        [DataRow(" ")]
        public void Save_WithBlankOfficePhone_ShouldThrowMobilePhoneRequiredException(string officePhone)
        {
            // arrange
            employee.OfficePhone = officePhone;
            // act
            // assert
            Assert.ThrowsException<MobilePhoneRequiredException>(
                () => sut.Save(employee.EmployeeId, employee)
                );

            mockEmployeeServiceRepository
                .Verify(
                    er => er.Create(employee), Times.Never()
                );
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow(" ")]
        public void Save_WithBlackExtension_ShouldThrowExtentionRequiredException(string extention)
        {
            // arrange
            employee.Extension = extention;
            // act
            // assert
            Assert.ThrowsException<MobilePhoneRequiredException>(
                () => sut.Save(employee.EmployeeId, employee)
                );

            mockEmployeeServiceRepository
                .Verify(
                    er => er.Create(employee), Times.Never()
                );
        }
    }

}
