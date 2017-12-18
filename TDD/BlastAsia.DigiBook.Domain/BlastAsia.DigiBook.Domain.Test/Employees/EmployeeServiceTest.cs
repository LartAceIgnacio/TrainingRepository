using BlastAsia.DigiBook.Domain.Employees;
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
        private Employee employee;
        private EmployeeService sut;
        private Mock<IEmployeeRepository> mockEmployeeRepository; // set mock
        private Guid existingEmployeeId = Guid.NewGuid();
        private Guid nonExistingEmployeeId = Guid.Empty;


        [TestInitialize]
        public void Initialize()
        {
            employee = new Employee {
                FirstName = "Chris",
                LastName = "Manuel",
                MobilePhone = "09156879240",
                EmailAddress = "cmanuel@blastasia.com",
                Photo = new MemoryStream(),
                OfficePhone = "758-5224",
                Extension = "02"
            };

            mockEmployeeRepository = new Mock<IEmployeeRepository>(); //instantiate mockRepository

            mockEmployeeRepository
                .Setup(x => x.Retrieve(existingEmployeeId))
                .Returns(employee);

            mockEmployeeRepository
                .Setup(x => x.Retrieve(nonExistingEmployeeId))
                .Returns<Employee>(null);

            sut = new EmployeeService(mockEmployeeRepository.Object); //pass the mockrepositry to be a copy as an Object

        }

        // Clean up every Initialized variables needed to be cleaned
        [TestCleanup]
        public void Cleanup()
        {

        }

        [TestMethod]
        public void Save_WithValidData_ReturnsNewEmployeeWithEmployeeId()
        {
            //Arrange
            mockEmployeeRepository
                .Setup(x => x.Create(It.IsAny<Employee>()))
                .Callback(() => employee.EmployeeId = Guid.NewGuid())
                .Returns(employee);

            //Act
            var newEmployee = sut.Save(employee);

            //Assert
            Assert.IsTrue(newEmployee.EmployeeId != Guid.Empty);
        }

        [TestMethod]
        public void Save_NewEmployeeWithValidData_ShouldCallRepositoryCreate()
        {
            //Arrange


            //Act
            sut.Save(employee);

            //Assert
            mockEmployeeRepository.Verify(x => x.Retrieve(nonExistingEmployeeId), Times.Once);
            mockEmployeeRepository.Verify(x => x.Create(employee), Times.Once);
        }

        [TestMethod]
        public void Save_WithValidDate_ShouldCallRepositoryUpdate()
        {
            //Arrange
            employee.EmployeeId = existingEmployeeId;

            //Act
            sut.Save(employee);

            //Assert
            mockEmployeeRepository.Verify(x => x.Retrieve(employee.EmployeeId), Times.Once);
            mockEmployeeRepository.Verify(x => x.Update(employee.EmployeeId, employee), Times.Once);
        }

        [TestMethod]
        public void Save_WithBlankFirstName_ThrowsNameRequiredException()
        {
            //Arrange
            employee.FirstName = "";

            //Act

            //Assert
            Assert.ThrowsException<NameRequiredException>(() => sut.Save(employee));
            mockEmployeeRepository.Verify(x => x.Retrieve(It.IsAny<Guid>()), Times.Never);
        }

        [TestMethod]
        public void Save_WithBlankLastName_ThrowsNameRequiredException()
        {
            //Arrange
            employee.LastName = "";

            //Act


            //Assert
            Assert.ThrowsException<NameRequiredException>(() => sut.Save(employee));
            mockEmployeeRepository.Verify(x => x.Retrieve(It.IsAny<Guid>()), Times.Never);
        }

        [TestMethod]
        public void Save_WithBlankMobilePhone_ThrowsPhoneRequiredException()
        {
            //Arrange
            employee.MobilePhone = "";

            //Act


            //Assert
            Assert.ThrowsException<PhoneRequiredException>(() => sut.Save(employee));
            mockEmployeeRepository.Verify(x => x.Retrieve(It.IsAny<Guid>()), Times.Never);
        }

        [TestMethod]
        public void Save_WithBlankOfficePhone_ThrowsPhoneRequiredException()
        {
            //Arrange
            employee.OfficePhone = "";

            //Act


            //Assert
            Assert.ThrowsException<PhoneRequiredException>(() => sut.Save(employee));
            mockEmployeeRepository.Verify(x => x.Retrieve(It.IsAny<Guid>()), Times.Never);
        }

        [TestMethod]
        public void Save_WithBlankEmailAddress_ThrowsEmailAddressRequiredException()
        {
            //Arrange
            employee.EmailAddress = "";

            //Act


            //Assert
            Assert.ThrowsException<EmailAddressRequiredException>(() => sut.Save(employee));
            mockEmployeeRepository.Verify(x => x.Retrieve(It.IsAny<Guid>()), Times.Never);
        }

        [TestMethod]
        [DataTestMethod]
        [DataRow("cmanuel@@blastasia.com")]
        [DataRow("@13_cmanuel@@blastasia.com@")]
        [DataRow("cmanuel@blastasia,com")]
        public void Save_InvalidEmailAddress_ThrowsInvalidEmailAddressException(string email)
        {
            //Arrange
            employee.EmailAddress = email;

            //Act


            //Assert
            Assert.ThrowsException<InvalidEmailAddressException>(() => sut.Save(employee));
            mockEmployeeRepository.Verify(x => x.Retrieve(It.IsAny<Guid>()), Times.Never);
        }

        [TestMethod]
        public void Save_WithBlankPhoto_ThrowsPhotoRequiredException()
        {
            //Arrange
            employee.Photo = null;

            //Act


            //Assert
            Assert.ThrowsException<PhotoRequiredException>(() => sut.Save(employee));
            mockEmployeeRepository.Verify(x => x.Retrieve(It.IsAny<Guid>()), Times.Never);
        }

        [TestMethod]
        public void Save_WithBlankExtension_ThrowsExtensionRequiredException()
        {
            //Arrange
            employee.Extension = "";

            //Act


            //Assert
            Assert.ThrowsException<ExtensionRequiredException>(() => sut.Save(employee));
            mockEmployeeRepository.Verify(x => x.Retrieve(It.IsAny<Guid>()), Times.Never);
        }
    }
}
