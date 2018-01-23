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
        private Guid nonExistingEmployeeId = Guid.Empty;
        private Guid existingEmployeeId = Guid.NewGuid();

        [TestInitialize]
        public void initialize()
        {
            employee = new Employee
            {
                FirstName = "Ryan Karl",
                LastName = "Oribello",
                MobilePhone = "09264709989",
                EmailAddress = "oribelloryan@gmail.com",
                Photo = "string",
                OfficePhone = "432-232-244",
                Extension = "101"
            };

            mockEmployeeRepository = new Mock<IEmployeeRepository>();
            sut = new EmployeeService(mockEmployeeRepository.Object);

            mockEmployeeRepository
               .Setup(c => c.Retrieve(existingEmployeeId))
                   .Returns(employee);
            //// if non existing
            mockEmployeeRepository
               .Setup(c => c.Retrieve(nonExistingEmployeeId))
                    .Returns<Employee>(null);
        }

        [TestMethod]
        public void Save_NewEmployeeWithValidData_ShouldCallEmployeeRepositoryCreate()
        {
            sut.Save(employee.EmployeeId, employee);

            mockEmployeeRepository
                   .Verify(er => er.Create(employee), Times.Once);
            mockEmployeeRepository
                .Verify(er => er.Retrieve(employee.EmployeeId), Times.Once);
        }

        [TestMethod]
        public void Save_ExistingIdWithhValidData_ShouldCallRepositoryUpdate()
        {

            employee.EmployeeId = existingEmployeeId;

            sut.Save(employee.EmployeeId, employee);

            mockEmployeeRepository
               .Verify(c => c.Retrieve(existingEmployeeId), Times.Once);
            mockEmployeeRepository
               .Verify(c => c.Update(existingEmployeeId, employee), Times.Once);
        }

        [TestMethod]
        public void Save_WithBlankFirstname_ThrowsEmployeeDetailRequiredException()
        {
            employee.FirstName = "";

            //sut.Save(employee);

            Assert.ThrowsException<EmployeeDetailRequiredException>(
                () => sut.Save(employee.EmployeeId, employee));
            mockEmployeeRepository
                .Verify(er => er.Create(employee), Times.Never);
        }

        [TestMethod]
        public void Save_WithBlankLastName_ThrowsEmployeeDetailRequiredException()
        {
            employee.LastName = "";
            //sut.Save(employee);
            Assert.ThrowsException<EmployeeDetailRequiredException>(
                () => sut.Save(employee.EmployeeId, employee));
            mockEmployeeRepository
                .Verify(er => er.Create(employee), Times.Never);
        }
        [TestMethod]
        public void Save_WithBlankMobilePhone_ThrowsEmployeeDetailRequiredException()
        {
            employee.MobilePhone = "";
            Assert.ThrowsException<EmployeeDetailRequiredException>(
               () => sut.Save(employee.EmployeeId, employee));
            mockEmployeeRepository
                .Verify(er => er.Create(employee), Times.Never);
        }
        [TestMethod]
        public void Save_WithBlankEmailAddress_ThrowsEmployeeDetailRequiredException()
        {
            employee.EmailAddress = "";
            Assert.ThrowsException<EmployeeDetailRequiredException>(
               () => sut.Save(employee.EmployeeId, employee));
            mockEmployeeRepository
                .Verify(er => er.Create(employee), Times.Never);
        }
        [TestMethod]
        public void Save_WithBlankOfficePhone_ThrowsEmployeeDetailRequiredException()
        {
            employee.OfficePhone = "";
            Assert.ThrowsException<EmployeeDetailRequiredException>(
               () => sut.Save(employee.EmployeeId, employee));
            mockEmployeeRepository
                .Verify(er => er.Create(employee), Times.Never);
        }
        [TestMethod]
        public void Save_WithBlankExtension_ThrowsEmployeeDetailRequiredException()
        {
            employee.Extension = "";
            Assert.ThrowsException<EmployeeDetailRequiredException>(
               () => sut.Save(employee.EmployeeId, employee));
            mockEmployeeRepository
                .Verify(er => er.Create(employee), Times.Never);
        }

        [DataTestMethod]
        [DataRow("oribelloryanyahoo.com")]
        [DataRow("oribelloryan@yahoo")]
        [DataRow("oribelloryanyahoo.com")]
        [DataRow("oribello_ryanyahoo.com")]
        public void Save_EmailInvalidFormat_ThrowsEmailInvalidFormatException(string email)
        {
            employee.EmailAddress = email;

            Assert.ThrowsException<EmailInvalidFormatException>(
                () => sut.Save(employee.EmployeeId, employee));
            mockEmployeeRepository
                .Verify(c => c.Create(employee), Times.Never());
        }

        //[TestMethod]
        //public void Save_WithBlankPhoto_ThrowsEmployeeDetailRequiredException()
        //{
        //    employee.Photo = null;
        //    Assert.ThrowsException<EmployeeDetailRequiredException>(
        //       () => sut.Save(employee.EmployeeId, employee));
        //    mockEmployeeRepository
        //        .Verify(er => er.Create(employee), Times.Never);
        //}
        
    }
}
