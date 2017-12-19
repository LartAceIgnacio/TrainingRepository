
using BlastAsia.DigiBook.Domain.Contacts;
using BlastAsia.DigiBook.Domain.Models.Contacts;
using BlastAsia.DigiBook.Domain.Contacts.ContactExceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Test.ContactsTest
{
    [TestClass]
    
    public class ContactServiceTest
    {
        private Mock<IContactRepository> _mockContactRepository;
        private Contact _contact;
        private ContactService _sut;
        private Guid _existingContactId; //= Guid.NewGuid();
        private Guid _nonExistingContactId;// = Guid.Empty;

        [TestInitialize]
        public void Initialize()
        {
            _existingContactId = Guid.NewGuid();
            _nonExistingContactId = Guid.Empty;

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

            _mockContactRepository.Setup(c => c.Retrieve(_existingContactId))
                .Returns(_contact);

            _mockContactRepository.Setup(c => c.Retrieve(_nonExistingContactId))
                .Returns<Contact>(null);

            //_mockContactRepository.Setup(c => c.Retrieve(It.IsAny<Guid>()));

        }

        [TestCleanup]
        public void Cleanup(){}

        [TestMethod]
        public void Save_NewContactWithValidData_ShouldCallRepositoryCreate()
        {

            // Act
            var result = _sut.Save(_contact.ContactId, _contact);

            //Assert
            _mockContactRepository.Verify(c => c.Retrieve(_nonExistingContactId), Times.Once);
            _mockContactRepository.Verify(o => o.Create(_contact), Times.Once);
        }

        [TestMethod]
        public void Save_WithExistingContact_CallsRepositoryUpdate()
        {
            // Arrange
            _contact.ContactId = _existingContactId;

            // Act
            _sut.Save(_existingContactId, _contact);


            // Assert
            _mockContactRepository.Verify(c => c.Retrieve(_contact.ContactId), Times.Once);
            _mockContactRepository.Verify(c => c.Update(_existingContactId, _contact), Times.Once);

        }



        [TestMethod]
        public void Save_WithValidData_ReturnsNewContactWithContactId()
        {
            // Arrange
            _mockContactRepository.Setup(c => c.Create(_contact))
                .Callback(() => _contact.ContactId = Guid.NewGuid())
                .Returns(_contact);

            // Act
            var newContact = _sut.Save(_contact.ContactId, _contact);

            // Assert
            Assert.IsTrue(newContact.ContactId != Guid.Empty);
        }

        [TestMethod]
        public void Save_WithBlankFirstname_ThrowsNameRequiredException()
        {
            // Arrange
            _contact.Firstname = string.Empty;

            // Act
            // Assert
            Assert.ThrowsException<NameRequiredException>(() => _sut.Save(_contact.ContactId, _contact));

            _mockContactRepository.Verify(c => c.Create(_contact), Times.Never);
        }

        [TestMethod]
        public void Save_WithBlankLastname_ThrowsNameRequiredException()
        {
            // Arrange
            _contact.Lastname = string.Empty;

            // Act
            // Assert

            Assert.ThrowsException<NameRequiredException>(
                    () => _sut.Save(_contact.ContactId,_contact));
            _mockContactRepository.Verify(c => c.Create(_contact), Times.Never);
        }

        [TestMethod]
        public void Save_WithBlankAddress_ThrowsAddressRequiredException()
        {
            // Arrange
            _contact.StreetAddress = string.Empty;

            // Act
            // Assert

            Assert.ThrowsException<AddressRequiredException>(
                () => _sut.Save(_contact.ContactId, _contact));

            _mockContactRepository.Verify(c => c.Create(_contact), Times.Never);
        }

        [TestMethod]
        public void Save_WithBlankMobileNumber_ThrowsContactNumberRequiredException()
        {
            // Arrange
            _contact.MobilePhone = string.Empty;

            // Act
            // Assert

            Assert.ThrowsException<ContactNumberRequiredException>(
                () => _sut.Save(_contact.ContactId, _contact));

            _mockContactRepository.Verify(c => c.Create(_contact), Times.Never);
        }

        [TestMethod]
        public void Save_MininumNumberOfContactNumber_ThrowsContatctNumberException()
        {
            // Arrange
            _contact.MobilePhone = "0929332";

            // Act
            // Assert

            Assert.ThrowsException<ContactNumberMinimumLength>(
                () => _sut.Save(_contact.ContactId, _contact));

            _mockContactRepository.Verify(c => c.Create(_contact), Times.Never);
        }

        [TestMethod]
        public void Save_WithBlankCityAddress_ThrowsCityAddressRequiredException()
        {
            // Arrange
            _contact.CityAddress = string.Empty;

            // Act
            // Assert
            Assert.ThrowsException<CityAddressRequiredException>(
                () => _sut.Save(_contact.ContactId, _contact));

            _mockContactRepository.Verify(c => c.Create(_contact), Times.Never);
        }

        [TestMethod]
        public void Save_WithNegativeZip_ThrowInvalidZipCodeException()
        {
            // Arrange
            _contact.ZipCode = -1;

            // Act
            // Assert
            Assert.ThrowsException<InvalidZipCodeException>(
                ()=> _sut.Save(_contact.ContactId, (_contact)));

            _mockContactRepository.Verify(c => c.Create(_contact), Times.Never);

        }

        [TestMethod]
        public void Save_WithBlankZip_ThrowInvalidZipCodeException()
        {
            // Arrange
            _contact.ZipCode = null;

            // Act
            // Assert
            Assert.ThrowsException<InvalidZipCodeException>(
                () => _sut.Save(_contact.ContactId, _contact));

            _mockContactRepository.Verify(c => c.Create(_contact), Times.Never);

        }

        [DataTestMethod]
        [DataRow("mmendezblastasia.com")]
        [DataRow("mmendezblastasia.com")]
        [DataRow("mmendez.blastasia.com")]        
        public void Save_InvalidEmailFormat_ThrowsInvalidEmailFormatException(string email)
        {
            _contact.EmailAddress = email;
            // ACT
            // ASSERT
            Assert.ThrowsException<InvalidEmailFormatException>(
                    () => _sut.Save(_contact.ContactId, _contact));

            _mockContactRepository.Verify(c => c.Create(_contact), Times.Never);
        }
    }
}
