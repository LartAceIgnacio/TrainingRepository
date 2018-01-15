using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlastAsia.DigiBook.Domain.Contacts;
using BlastAsia.DigiBook.Domain.Models.Contacts;

namespace BlastAsia.DigiBook.Domain.Test.Contacts
{
    [TestClass]
    public class ContactServiceTest
    {
        // global variables
        private Contact contact;
        private ContactService sut;
        private Mock<IContactRepository> mockContactRepository; // set mock
        private Guid existingContactId = Guid.NewGuid();
        private Guid nonExistingContactId = Guid.Empty;

        [TestInitialize]
        public void Initialize()
        {
            contact = new Contact {
                FirstName = "Chris",
                LastName = "Manuel",
                MobilePhone = "09156879240",
                StreetAddress = "#3 Pananalig St., Brgy. Mabini JRizal",
                CityAddress = "Mandaluyong City",
                ZipCode = 1550,
                Country = "Philippines",
                EmailAddress = "cmanuel@blastasia.com",
                IsActive = false,
                DateActivated = new Nullable<DateTime>()
            };

            mockContactRepository = new Mock<IContactRepository>(); //instantiate mockRepository

            mockContactRepository
                .Setup(x => x.Retrieve(existingContactId))
                .Returns(contact);

            mockContactRepository
                .Setup(x => x.Retrieve(nonExistingContactId))
                .Returns<Contact>(null);

            sut = new ContactService(mockContactRepository.Object); //pass the mockrepositry to be a copy as an Object

        }

        // Clean up every Initialized variables needed to be cleaned
        [TestCleanup]
        public void Cleanup()
        {

        }

        [TestMethod]
        public void Save_NewContactWithValidData_ShouldCallRepositoryCreate()
        {
            //Arrange

            //Act
            var result = sut.Save(contact.ContactId, contact);

            //Assert
            //mockContactRepository.Verify(x => x.Retrieve(nonExistingContactId), Times.Once());
            mockContactRepository.Verify(x => x.Create(It.IsAny<Contact>()), Times.Once());
        }

        [TestMethod]
        public void Save_WithValidData_ShouldCallRepositoryUpdate()
        {
            //Arrange
            contact.ContactId = existingContactId;

            //Act
            sut.Save(contact.ContactId, contact);

            //Assert
            //mockContactRepository.Verify(x => x.Retrieve(contact.ContactId), Times.Once());
            mockContactRepository.Verify(x => x.Update(contact.ContactId, It.IsAny<Contact>()), Times.Once());
        }

        [TestMethod]
        public void Save_WithValidData_ReturnsNewContactWithContactId()
        {
            //Arrange
            mockContactRepository
                .Setup(x => x.Create(It.IsAny<Contact>()))
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

            //Act

            //Asset
            Assert.ThrowsException<NameRequiredException>(() => sut.Save(contact.ContactId, contact));
            mockContactRepository.Verify(x => x.Create(It.IsAny<Contact>()), Times.Never);
        }

        [TestMethod]
        public void Save_WithBlankLastName_ThrowsNameRequiredException()
        {
            //Arrange
            contact.LastName = "";

            //Act

            //Asset
            Assert.ThrowsException<NameRequiredException>(() => sut.Save(contact.ContactId, contact));
            mockContactRepository.Verify(x => x.Create(It.IsAny<Contact>()), Times.Never);
        }

        [TestMethod]
        public void Save_WithBlankMobilePhone_ThrowsMobilePhoneRequiredException()
        {
            //Arrange
            contact.MobilePhone = "";

            //Act

            //Asset
            Assert.ThrowsException<MobilePhoneRequiredException>(() => sut.Save(contact.ContactId, contact));
            mockContactRepository.Verify(x => x.Create(It.IsAny<Contact>()), Times.Never);
        }

        [TestMethod]
        public void Save_WithBlankStreetAddress_ThrowsAddressRequiredException()
        {
            //Arrange
            contact.StreetAddress = "";

            //Act

            //Asset
            Assert.ThrowsException<AddressRequiredException>(() => sut.Save(contact.ContactId, contact));
            mockContactRepository.Verify(x => x.Create(It.IsAny<Contact>()), Times.Never);
        }

        [TestMethod]
        public void Save_WithBlankCityAddress_ThrowsAddressRequiredException()
        {
            //Arrange
            contact.CityAddress = "";

            //Act

            //Asset
            Assert.ThrowsException<AddressRequiredException>(() => sut.Save(contact.ContactId, contact));
            mockContactRepository.Verify(x => x.Create(It.IsAny<Contact>()), Times.Never);
        }

        [TestMethod]
        public void Save_WithBlankZipCode_ThrowsAddressRequiredException()
        {
            //Arrange
            contact.ZipCode = 0;

            //Act

            //Asset
            Assert.ThrowsException<AddressRequiredException>(() => sut.Save(contact.ContactId, contact));
            mockContactRepository.Verify(x => x.Create(It.IsAny<Contact>()), Times.Never);
        }

        [TestMethod]
        public void Save_WithNegativeZipCode_ThrowsNagativeZipCodeException()
        {
            //Arrange
            contact.ZipCode = -11;

            //Act

            //Asset
            Assert.ThrowsException<NagativeZipCodeException>(() => sut.Save(contact.ContactId, contact));
            mockContactRepository.Verify(x => x.Create(It.IsAny<Contact>()), Times.Never);
        }

        [TestMethod]
        public void Save_WithBlankCountry_ThrowsAddressRequiredException()
        {
            //Arrange
            contact.Country = "";

            //Act

            //Asset
            Assert.ThrowsException<AddressRequiredException>(() => sut.Save(contact.ContactId, contact));
            mockContactRepository.Verify(x => x.Create(It.IsAny<Contact>()), Times.Never);
        }

        [DataTestMethod]
        [DataRow("cmanuel.blastasia.com")]
        [DataRow("cmanuel@blastasiacom")]
        [DataRow("cmanuel@@blast.asia.com.")]
        [TestMethod]
        public void Save_InvalidEmailAddress_ThrowsInvalidEmailAddressException(string email)
        {
            //Arrange
            contact.EmailAddress = email;

            //Act

            //Assert
            Assert.ThrowsException<InvalidEmailAddressException>(() => sut.Save(contact.ContactId, contact));
            mockContactRepository.Verify(x => x.Create(It.IsAny<Contact>()), Times.Never);
        }
    }
}
