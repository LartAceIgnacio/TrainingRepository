using BlastAsia.DigiBook.Domain.Contacts;
using BlastAsia.DigiBook.Domain.Models.Contacts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

// + public
// - private
// # protected
// _ static

namespace BlastAsia.DigiBook.Domain.Test.Contacts
{
    [TestClass]

    public class ContactServiceTest
    {
        private Contact contact;
        private Mock<IContactRepository> mockContactRepository;
        ContactService sut;
        private Guid existingContactId = Guid.NewGuid();
        private Guid nonExistingContactId = Guid.Empty;

        [TestInitialize]
        public void TestInitialize()
        {
            contact = new Contact
            {
                FirstName = "John Karl",
                LastName = "Matencio",
                MobilePhone = "09957206817",
                StreetAddress = "7th St. Metrogate, Dasmariñas",
                CityAddress = "Cavite",
                ZipCode = 1604,
                Country = "Philippines",
                EmailAddress = "jhnkrl15@gmail.com",
                IsActive = false,
                DateActivated = new Nullable<DateTime>()
            };

            mockContactRepository = new Mock<IContactRepository>();

            mockContactRepository
                .Setup(c => c.Create(contact))
                    .Callback(() =>
                    {
                        contact.ContactId = Guid.NewGuid();
                        contact.DateActivated = DateTime.Now;
                    })
                    .Returns(contact);

            mockContactRepository
                .Setup(c => c.Retrieve(existingContactId))
                    .Returns(contact);

            mockContactRepository
                .Setup(c => c.Retrieve(nonExistingContactId))
                    .Returns<Contact>(null);

            sut = new ContactService(mockContactRepository.Object);
        }

        [TestCleanup]
        public void CleanupTeset()
        {

        }

        [TestMethod]
        public void Save_NewContactWithValidData_ShouldCallRepositoryCreate()
        {
            //Arrange
            //Act
            var result = sut.Save(contact.ContactId, contact);
            // Assert
            mockContactRepository
                .Verify(cr => cr.Retrieve(nonExistingContactId), Times.Once());

            mockContactRepository
                .Verify(cr => cr.Create(contact), Times.Once());
        }

        [TestMethod]
        public void Save_WithExistingContact_CallsRepositoryUpdate()
        {
            // Arrange
            contact.ContactId = existingContactId;
            // act
            sut.Save(contact.ContactId,contact);
            // assert
            mockContactRepository
                .Verify(cr => cr.Retrieve(existingContactId), Times.Once);

            mockContactRepository
                .Verify(cr => cr.Update(existingContactId, contact), Times.Once);
        }

        [TestMethod]
        public void Save_WithValidData_ReturnsNewContactWithContactID()
        {
            //Arrange
            mockContactRepository
                .Setup(c => c.Create(contact))
                .Callback(() => contact.ContactId = Guid.NewGuid())
                .Returns(contact);


            mockContactRepository
                .Setup(c => c.Retrieve(existingContactId))
                .Returns(contact);

            mockContactRepository
                .Setup(c => c.Retrieve(nonExistingContactId))
                .Returns<Contact>(null);

            //Act
            var newContact = sut.Save(contact.ContactId, contact);
            //Assert
            Assert.IsTrue(newContact.ContactId != Guid.Empty);
        }

        [TestMethod]
        public void Save_WithBlankFirstName_ThrowsNameRequiredException()
        {
            //Arrange
            contact.FirstName = "";
            //Act
            //Assert
            Assert.ThrowsException<NameRequiredException>(
                () => sut.Save(contact.ContactId, contact));
            mockContactRepository.
              Verify(c => c.Create(contact), Times.Never);
        }

        [TestMethod]
        public void Save_WithBlankLastName_ThrowsNameRequiredException()
        {
            //Arrange
            contact.LastName = "";
            //Act
            //Assert
            Assert.ThrowsException<NameRequiredException>(
                () => sut.Save(contact.ContactId, contact));
            mockContactRepository.
              Verify(c => c.Create(contact), Times.Never);
        }

        [TestMethod]
        public void Save_WithBlankMobilePhone_ThrowsMobilePhoneRequiredException()
        {
            //Arrange
            contact.MobilePhone = "";
            //Act
            //Assert
            Assert.ThrowsException<MobilePhoneRequiredException>(
                () => sut.Save(contact.ContactId, contact));
            mockContactRepository.
              Verify(c => c.Create(contact), Times.Never);
        }

        [TestMethod]
        public void Save_WithBlankStreetAddress_ThrowsStreetAddressRequiredException()
        {
            //Arrange
            contact.StreetAddress = "";
            //Act

            //Assert
            Assert.ThrowsException<AddressRequiredException>(
                () => sut.Save(contact.ContactId, contact));
            mockContactRepository.
              Verify(c => c.Create(contact), Times.Never);
        }

        [TestMethod]
        public void Save_WithBlankSCityAddress_ThrowsCityAddressRequiredException()
        {
            //Arrange
            contact.CityAddress = "";
            //Act
            //Assert
            Assert.ThrowsException<AddressRequiredException>(
                () => sut.Save(contact.ContactId, contact));
            mockContactRepository.
              Verify(c => c.Create(contact), Times.Never);
        }

        [TestMethod]
        public void Save_WithNegativeZipcode_ThrowsNegativeZipcodeInvalidException()
        {
            //Arrange
            contact.ZipCode = -1;
            //Act
            //Assert
            Assert.ThrowsException<ZipcodeShouldBePositiveException>(
                () => sut.Save(contact.ContactId, contact));
            mockContactRepository.
              Verify(c => c.Create(contact), Times.Never);
        }

        [TestMethod]
        public void Save_WithBlankCountry_ThrowsCountryRequiredException()
        {
            //Arrange
            contact.Country = "";
            //Act
            //Assert
            Assert.ThrowsException<CountryRequiredException>(
                () => sut.Save(contact.ContactId, contact));
            mockContactRepository.
              Verify(c => c.Create(contact), Times.Never);
        }
    }
}
