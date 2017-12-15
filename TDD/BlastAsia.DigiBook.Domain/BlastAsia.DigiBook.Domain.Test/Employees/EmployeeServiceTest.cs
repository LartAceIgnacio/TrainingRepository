using BlastAsia.DigiBook.Domain.Employees;
using BlastAsia.DigiBook.Domain.Employees.Exceptions;
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
        private Guid existingEmployeeId = Guid.NewGuid();
        private Guid nonExistingEmployeeId = Guid.Empty;


        [TestInitialize]
        public void InitializeTest()
        {
            mockEmployeeRepository = new Mock<IEmployeeRepository>();
            sut = new EmployeeService(mockEmployeeRepository.Object);

            employee = new Employee
            {
                EmployeeId = new Guid(),
                FirstName = "Jhoane",
                LastName = "Garcia",
                MobilePhone = "09123456789",
                EmailAddress = "jgarcia@gmail.com",
                Photo = "Photozzzzz",
                OfficePhone = "9312062",
                Extension = "012"
            };


            mockEmployeeRepository
                .Setup(er => er.Create(employee))
                .Returns(employee);

            mockEmployeeRepository
                .Setup(er => er.Retrieve(existingEmployeeId))
                .Returns(employee);
        }

        [TestMethod]
        public void Save_EmployeeWithExistingValidData_ShouldCallEmployeeRepositoryUpdate()
        {
            employee.EmployeeId = existingEmployeeId;
            sut.Save(employee);

            mockEmployeeRepository
                .Verify(er => er.Retrieve(existingEmployeeId)
                , Times.Once);

            mockEmployeeRepository
                .Verify(er => er.Update(existingEmployeeId, employee)
                , Times.Once);
        }

        [TestMethod]
        public void Save_EmployeeWithValidData_ShouldCallEmployeeRepositoryCreate()
        {
            sut.Save(employee);

            mockEmployeeRepository
                .Verify(er => er.Retrieve(nonExistingEmployeeId)
                , Times.Once);

            mockEmployeeRepository
                .Verify(er => er.Create(employee)
                , Times.Once);
        }

        [TestMethod]
        public void Save_EmployeeWithValidData_ShouldReturnNewEmployeeWithEmployeeId()
        {
            mockEmployeeRepository
                .Setup(er => er.Create(employee))
                .Callback(() => employee.EmployeeId = Guid.NewGuid())
                    .Returns(employee);

            sut.Save(employee);

            Assert.IsTrue(employee.EmployeeId != Guid.Empty);
        }

        [TestMethod]
        public void Save_EmployeeWithBlankFirstName_ShouldThrowNameRequiredException()
        {
            employee.FirstName = "";

            Assert.ThrowsException<NameRequiredException>(
                ()=> sut.Save(employee));

            mockEmployeeRepository
                .Verify(er => er.Create(employee), Times.Never);
        }

        [TestMethod]
        public void Save_EmployeeWithBlankLastName_ShouldThrowNameRequiredException()
        {
            employee.LastName = "";

            Assert.ThrowsException<NameRequiredException>(
                () => sut.Save(employee));

            mockEmployeeRepository
                .Verify(er => er.Create(employee),Times.Never);
        }

        [TestMethod]
        public void Save_EmployeeWithBlankMobilePhone_ShouldThrowMobilePhoneRequiredException()
        {
            employee.MobilePhone = "";

            Assert.ThrowsException<MobilePhoneException>(
                ()=>sut.Save(employee));

            mockEmployeeRepository
                .Verify(er => er.Create(employee), Times.Never);
        }

        [TestMethod]
        public void Save_EmployeeWithBlankEmailAddress_ShouldThrowEmailAddressException()
        {
            employee.EmailAddress = "";

            Assert.ThrowsException<EmailAddressException>(
                ()=>sut.Save(employee));

            mockEmployeeRepository
                .Verify(er => er.Create(employee), Times.Never);
        }

        [DataTestMethod]
        [DataRow("renznebran.com")]
        [DataRow("renznebran@yahoo..com")]
        [DataRow("renznebran@yahoo.com.")]
        [DataRow("renznebran@@yahoo.com")]
        [DataRow("renznebran@yahoocom")]
        [TestMethod]
        public void Save_EmployeeWithInvalidEmailAddress_ShouldThrowEmailAddressException(string email)
        {
            employee.EmailAddress = email;

            Assert.ThrowsException<EmailAddressException>(
                ()=> sut.Save(employee));

            mockEmployeeRepository
                .Verify(er => er.Create(employee), Times.Never);

        }

        [TestMethod]
        public void Save_EmployeeWithBlankPhoto_ShouldThrowPhotoRequiredException()
        {
            employee.Photo = "";

            Assert.ThrowsException<PhotoRequiredException>(
                () => sut.Save(employee));

            mockEmployeeRepository
                .Verify(er => er.Create(employee), Times.Never);
        }

        [TestMethod]
        public void Save_EmployeeWithBlankOfficePhone_ShouldThrowOfficePhoneRequiredException()
        {
            employee.OfficePhone = "";

            Assert.ThrowsException<OfficePhoneException>(
                () => sut.Save(employee));

            mockEmployeeRepository
                .Verify(er => er.Create(employee), Times.Never);

        }

        [DataTestMethod]
        [DataRow("MobileTesting")]
        [DataRow("@@@@@")]
        [DataRow("123MobileTesting")]
        [DataRow("MobileTesting123")]
        [TestMethod]
        public void Save_EmployeeWithInvalidMobilePhone_ShouldThrowMobilePhoneException(string mobilePhone)
        {
            employee.MobilePhone = mobilePhone;

            Assert.ThrowsException<MobilePhoneException>(
                () => sut.Save(employee));

            mockEmployeeRepository
                .Verify(er => er.Create(employee), Times.Never);

        }



        [DataTestMethod]
        [DataRow("OfficeTesting")]
        [DataRow("//////")]
        [DataRow("123OfficeTesting")]
        [DataRow("OfficeTesting123")]
        [TestMethod]
        public void Save_EmployeeWithInvalidOfficePhone_ShouldThrowOfficePhoneException(string officePhone)
        {
            employee.OfficePhone = officePhone;

            Assert.ThrowsException<OfficePhoneException>(
                () => sut.Save(employee));

            mockEmployeeRepository
                .Verify(er => er.Create(employee), Times.Never);
        }

        [TestMethod]
        public void Save_EmployeeWithBlankExtension_ShouldThrowExtensionRequiredException()
        {

            employee.Extension = "";

            Assert.ThrowsException<ExtensionRequiredException>(
                () => sut.Save(employee));

            mockEmployeeRepository
                .Verify(er => er.Create(employee), Times.Never);
        }
    }
}
