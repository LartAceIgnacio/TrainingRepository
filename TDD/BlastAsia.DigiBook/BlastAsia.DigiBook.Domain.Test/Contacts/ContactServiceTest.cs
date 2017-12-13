
using BlastAsia.DigiBook.Domain.Contacts;
using BlastsAsia.DigiBook.Domain.Models.Contacts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Test.Contacts
{
    [TestClass]
    public class ContactServiceTest
    {
        private Mock<IContactRepository> _mockContactRepository;
        private Contact _contact;
        private ContactService _sut;

        [TestInitialize]
        public void Initialize()
        {

            _mockContactRepository = new Mock<IContactRepository>();
            _sut = new ContactService(_mockContactRepository.Object);
            _contact = new Contact
            {

                Firstname = "Matt",
                Lastname = "Mendez",
                MobilePhone = "09293235700",
                StreetAddress = "318 Saint Michael St., Brgy. Holy Spirit",
                CityAddress = "Quezon City",
                ZipCode = 1127,
                EmailAddress = "mmendez@blastasia.com",
                IsActive = false,
                DateActivated = new Nullable<DateTime>()
            };



        }

        [TestCleanup]
        public void Cleanup(){}

        [TestMethod]
        public void Create_WithValidData_ShouldCallRepository()
        {
            // Act
            _sut.Create(_contact);
            
            //Assert
            _mockContactRepository.Verify(o => o.Create(_contact), Times.Once());
        }

        [TestMethod]
        public void Create_WithValidData_ReturnsNewContactWithContactId()
        {
            // Arrange
            _mockContactRepository.Setup(c => c.Create(_contact))
                .Callback(() => _contact.ContactId = Guid.NewGuid())
                .Returns(_contact);

            // Act
            var newContact = _sut.Create(_contact);

            // Assert
            Assert.IsTrue(newContact.ContactId != Guid.Empty);
        }

        [TestMethod]
        public void Create_WithBlankFirstname_ThrowsNameRequiredException()
        {
            // Arrange
            _contact.Firstname = string.Empty;

            // Act
            // Assert
            Assert.ThrowsException<NameRequiredException>(() => _sut.Create(_contact));

            _mockContactRepository.Verify(c => c.Create(_contact), Times.Never);
        }

        [TestMethod]
        public void Create_WithBlankLastname_ThrowsNameRequiredException()
        {
            // Arrange
            _contact.Lastname = string.Empty;

            // Act
            // Assert

            Assert.ThrowsException<NameRequiredException>(
                    () => _sut.Create(_contact));
            _mockContactRepository.Verify(c => c.Create(_contact), Times.Never);
        }

        [TestMethod]
        public void Create_WithBlankAddress_ThrowsAddressRequiredException()
        {
            // Arrange
            _contact.StreetAddress = string.Empty;

            // Act
            // Assert

            Assert.ThrowsException<AddressRequiredException>(
                () => _sut.Create(_contact));

            _mockContactRepository.Verify(c => c.Create(_contact), Times.Never);
        }

        [TestMethod]
        public void Create_WithBlankMobileNumber_ThrowsContactNumerRequiredException()
        {
            // Arrange
            _contact.MobilePhone = string.Empty;

            // Act
            // Assert

            Assert.ThrowsException<ContactNumberRequiredException>(
                () => _sut.Create(_contact));

            _mockContactRepository.Verify(c => c.Create(_contact), Times.Never);
        }

        [TestMethod]
        public void Create_WithBlankCityAddress_ThrowsCityAddressRequiredException()
        {
            // Arrange
            _contact.CityAddress = string.Empty;

            // Act
            // Assert
            Assert.ThrowsException<CityAddressRequiredException>(
                () => _sut.Create(_contact));

            _mockContactRepository.Verify(c => c.Create(_contact), Times.Never);
        }

        [TestMethod]
        public void Create_WithNegativeZip_ThrowInvalidZipCodeException()
        {
            // Arrange
            _contact.ZipCode = -1;

            // Act
            // Assert
            Assert.ThrowsException<InvalidZipCodeException>(
                ()=> _sut.Create(_contact));

            _mockContactRepository.Verify(c => c.Create(_contact), Times.Never);

        }

        [TestMethod]
        public void Create_WithBlankZip_ThrowInvalidZipCodeException()
        {
            // Arrange
            _contact.ZipCode = null;

            // Act
            // Assert
            Assert.ThrowsException<InvalidZipCodeException>(
                () => _sut.Create(_contact));

            _mockContactRepository.Verify(c => c.Create(_contact), Times.Never);

        }

        [DataTestMethod]
        [DataRow("mmendezblastasia.com")]
        [DataRow("mmendezblastasia.com")]
        [DataRow("mmendez.blastasia.com")]        
        public void Create_InvalidEmailFormat_ThrowsInvalidEmailFormatException(string email)
        {
            _contact.EmailAddress = email;
            // ACT
            // ASSERT
            Assert.ThrowsException<InvalidEmailFormatException>(
                    () => _sut.Create(_contact));

            _mockContactRepository.Verify(c => c.Create(_contact), Times.Never);
        }
    }
}
