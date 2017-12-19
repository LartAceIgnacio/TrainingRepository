using BlastAsia.DigiBook.Domain.Contacts;
using BlastAsia.DigiBook.Domain.Models.Contacts;
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
        private Contact contact;
        private Mock<IContactRepository> mockContactRepository;
        private ContactService sut;
        private Guid existingContactID;
        private Guid nonExistingContactID;

        [TestInitialize]
        public void InitializeTest()
        {
            contact = new Contact
            {
                FirstName = "Jasmin",
                LastName = "Magdaleno",
                MobilePhone = "09057002880",
                StreetAddress = "L2 B15 Utex St., Litex Vill., San Jose",
                CityAddress = "Rodriguez, Rizal",
                ZipCode = 1860,
                Country = "Philippines",
                EmailAddress = "jasminmagdaleno@blastasia.com",
                IsActive = false,
                DateActivated = new Nullable<DateTime>()
            };

            mockContactRepository = new Mock<IContactRepository>();
            sut = new ContactService(mockContactRepository.Object);

            existingContactID = Guid.NewGuid();
            nonExistingContactID = Guid.Empty;

            mockContactRepository
                .Setup(c => c.Retrieve(nonExistingContactID))
                .Returns<Contact>(null);

            mockContactRepository
                .Setup(c => c.Retrieve(existingContactID))
                .Returns(contact);
        }

        [TestCleanup]
        public void CleanUp()
        {

        }

        [TestMethod]
        public void Save_NewContactWithValidData_ShouldCallRepositoryCreate()
        {
            //Arrange

            //Act
            var result = sut.Save(contact.ContactId,contact);

            //Assert
            mockContactRepository
                .Verify(c => c.Retrieve(nonExistingContactID), Times.Once());

            mockContactRepository
                .Verify(c => c.Create(contact), Times.Once());
        }

        [TestMethod]
        public void Save_WithExistingContact_ShouldCallRepositoryUpdate()
        {
            //Arrange
            contact.ContactId = existingContactID;

            //Act
            sut.Save(contact.ContactId, contact);

            //Assert
            mockContactRepository
                .Verify(c => c.Retrieve(existingContactID), Times.Once());

            mockContactRepository
                .Verify(c => c.Update(existingContactID, contact), Times.Once());
        }

        [TestMethod]
        public void Save_WithValidData_ReturnsNewContactWithContactID()
        {
            //Arrange
            mockContactRepository
                .Setup(c => c.Create(contact))
                .Callback(() => contact.ContactId = Guid.NewGuid())
                .Returns(contact);
            
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

            //Assert
            Assert.ThrowsException<NameRequiredException>(
                () => sut.Save(contact.ContactId, contact));

            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
        }

        [TestMethod]
        public void Save_WithBlankLastName_ThrowsNameRequiredException()
        {
            //Arrange
            contact.LastName = "";

            //Assert
            Assert.ThrowsException<NameRequiredException>(
                () => sut.Save(contact.ContactId, contact));

            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
        }

        [TestMethod]
        public void Save_WithBlankMobileNumber_ThrowsMobileNumberRequiredException()
        {
            //Arrange
            contact.MobilePhone = "";

            //Assert
            Assert.ThrowsException<MobileNumberRequiredException>(
                () => sut.Save(contact.ContactId, contact));

            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
        }

        [TestMethod]
        public void Save_WithBlankStreetAddress_ThrowsAddressRequiredException()
        {
            //Arrange
            contact.StreetAddress = "";

            //Assert
            Assert.ThrowsException<AddressRequiredException>(
                () => sut.Save(contact.ContactId, contact));

            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
        }

        [TestMethod]
        public void Save_WithBlankCityAddress_ThrowsAddressRequiredException()
        {
            //Arrange
            contact.CityAddress = "";

            //Assert
            Assert.ThrowsException<AddressRequiredException>(
                () => sut.Save(contact.ContactId, contact));

            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
        }

        [TestMethod]
        public void Save_WithBlankZipCode_ThrowsAddressRequiredException()
        {
            //Arrange
            contact.ZipCode = 0;

            //Assert
            Assert.ThrowsException<AddressRequiredException>(
                () => sut.Save(contact.ContactId, contact));

            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
        }

        [TestMethod]
        public void Save_WithNegativeZipCode_ThrowsValidZipCodeRequiredException()
        {
            //Arrange
            contact.ZipCode = -1;

            //Assert
            Assert.ThrowsException<ValidZipCodeRequiredException>(
                () => sut.Save(contact.ContactId, contact));

            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
        }

        [TestMethod]
        public void Save_WithBlankCountry_ThrowsAddressRequiredException()
        {
            //Arrange
            contact.Country = "";

            //Assert
            Assert.ThrowsException<AddressRequiredException>(
                () => sut.Save(contact.ContactId, contact));

            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
        }

        [TestMethod]
        public void Save_WithInvalidEmailAddress_ThrowsEmailAddressRequiredException()
        {
            //Arrange
            contact.EmailAddress = "jmagdalenoblastasiacom";

            //Assert
            Assert.ThrowsException<EmailAddressRequiredException>(
                    () => sut.Save(contact.ContactId, contact));

            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
        }
    }
}
