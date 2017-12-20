using BlastAsia.DigiBook.Domain.Contacts;
using BlastAsia.DigiBook.Domain.Models.Contacts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace BlastAsia.DigiBook.Domain.Test
{
    [TestClass]
    public class ContactServiceTest
    {
        private Mock<IContactRepository> mockContactRepository;
        private ContactService sut;
        private Contact contact;
        private Guid existingContactId = Guid.NewGuid();
        private Guid nonExistingContactId = Guid.Empty;

        [TestInitialize]
        public void InitializeTest()
        {
            contact = new Contact
            {
                FirstName = "Luigi", // required
                LastName = "Abille", // required
                MobilePhone = "09568717617", // required 
                StreetAddress = "Northridge", // required
                CityAddress = "Quezon City", // required
                ZipCode = 1117, // required
                Country = "Philippines", // required
                EmailAddress = "luigiabille@gmail.com",
                IsActive = false,
                DateActivated = new Nullable<DateTime>()

            };

            mockContactRepository = new Mock<IContactRepository>();

            sut = new ContactService(mockContactRepository.Object);

            mockContactRepository
                .Setup(c => c.Retrieve(existingContactId))
                .Returns(contact);
            mockContactRepository
                .Setup(c => c.Retrieve(nonExistingContactId))
                .Returns<Contact>(null);

               
        }

        [TestCleanup]
        public void CleanupTest()
        {

        }

        [TestMethod]
        public void Save_NewContactWithValidData_ShouldCallRepositoryCreate()
        {
            //Act

            var result = sut.Save(contact.ContactId, contact);

            //Assert

            mockContactRepository
                .Verify(c => c.Retrieve(nonExistingContactId), Times.Once);
            mockContactRepository
                .Verify(c => c.Create(contact), Times.Once);
        }

        [TestMethod]
        public void Save_WithExistingContact_ShouldCallRepositoryUpdate()
        {
            // Arrange

            contact.ContactId = existingContactId;

            // Act

            sut.Save(contact.ContactId, contact);

            // Assert

            mockContactRepository
                .Verify(c => c.Retrieve(existingContactId), Times.Once);
            mockContactRepository
                .Verify(c => c.Update(existingContactId, contact), Times.Once);
        }

        [TestMethod]
        public void Save_WithValidData_ReturnsNewContactWithContactId()
        {
            // Arrange

            mockContactRepository
                .Setup(c => c.Create(contact))
                .Callback(() => contact.ContactId = Guid.NewGuid())
                .Returns(contact);

            // Act

            var newContact = sut.Save(contact.ContactId, contact);

            // Assert

            Assert.IsTrue(newContact.ContactId != Guid.Empty);
        }

        [TestMethod]
        public void Save_WithBlankFirstName_ThrowsNameRequiredException()
        {
            // Arrange

            contact.FirstName = "";

            // Assert

            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
            Assert.ThrowsException<NameRequiredException>(
                () => sut.Save(contact.ContactId, contact));
        }

        [TestMethod]
        public void Save_WithBlankLastName_ThrowsNameRequiredException()
        {
            // Arrange

            contact.LastName = "";

            // Assert

            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
            Assert.ThrowsException<NameRequiredException>(
                () => sut.Save(contact.ContactId, contact));
        }

        [TestMethod]
        public void Save_WithBlankMobilePhone_ThrowsMobilePhoneRequiredException()
        {
            // Arrange

            contact.MobilePhone = "";

            // Assert

            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
            Assert.ThrowsException<MobilePhoneRequiredException>(
                () => sut.Save(contact.ContactId, contact));
        }

        [TestMethod]
        public void Save_WithBlankStreetAddress_ThrowsAddressRequiredException()
        {
            // Arrange

            contact.StreetAddress = "";

            // Assert

            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
            Assert.ThrowsException<AddressRequiredException>(
                () => sut.Save(contact.ContactId, contact));
        }

        [TestMethod]
        public void Save_WithBlankCityAddress_ThrowsAddressRequiredException()
        {
            // Arrange

            contact.CityAddress = "";

            // Assert

            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
            Assert.ThrowsException<AddressRequiredException>(
                () => sut.Save(contact.ContactId, contact));
        }

        [TestMethod]
        public void Save_WithNegativeZipCode_ThrowsAddressRequiredException()
        {
            // Arrange

            contact.ZipCode = -8;

            // Assert

            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
            Assert.ThrowsException<AddressRequiredException>(
                () => sut.Save(contact.ContactId, contact));
        }

        [TestMethod]
        public void Save_WithBlankCountry_ThrowsAddressRequiredException()
        {
            // Arrange

            contact.Country = "";

            // Assert

            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
            Assert.ThrowsException<AddressRequiredException>(
                () => sut.Save(contact.ContactId, contact));
        }

        [DataTestMethod]
        [DataRow("luigiabille.com")]
        [DataRow("luigiabille@gmail..com")]
        [DataRow("luigiabille@gmail.com.")]
        [TestMethod]
        public void Save_WithInvalidEmailAddress_ThrowsInvalidEmailRequiredException(string EmailAddress)
        {
            // Arrange

            contact.EmailAddress = EmailAddress;

            // Assert

            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never);
            Assert.ThrowsException<InvalidEmailAddressException>(
                () => sut.Save(contact.ContactId, contact));
        }
    }
}
