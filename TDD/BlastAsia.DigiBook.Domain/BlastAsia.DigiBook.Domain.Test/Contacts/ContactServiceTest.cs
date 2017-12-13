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
            sut = new ContactService(mockContactRepository.Object); //pass the mockrepositry to be a copy as an Object

        }

        // Clean up every Initialized variables needed to be cleaned
        [TestCleanup]
        public void Cleanup()
        {

        }

        [TestMethod]
        public void Create_WithValidData_ShouldCallRepositoryCreate()
        {
            //Arrange

            //Act
            var result = sut.Create(contact);

            //Assert
            mockContactRepository.Verify(x => x.Create(It.IsAny<Contact>()), Times.Once());
        }

        [TestMethod]
        public void Create_WithValidData_ReturnsNewContactWithContactId()
        {
            //Arrange
            mockContactRepository
                .Setup(x => x.Create(It.IsAny<Contact>()))
                .Callback(() => contact.ContactId = Guid.NewGuid())
                .Returns(contact);

            //Act
            var newContact = sut.Create(contact);

            //Assert
            Assert.IsTrue(newContact.ContactId != Guid.Empty);
        }

        [TestMethod]
        public void Create_WithBlankFirstName_ThrowsNameRequiredException()
        {
            //Arrange
            contact.FirstName = "";

            //Act

            //Asset
            Assert.ThrowsException<NameRequiredException>(() => sut.Create(contact));
            mockContactRepository.Verify(x => x.Create(It.IsAny<Contact>()), Times.Never);
        }

        [TestMethod]
        public void Create_WithBlankLastName_ThrowsNameRequiredException()
        {
            //Arrange
            contact.LastName = "";

            //Act

            //Asset
            Assert.ThrowsException<NameRequiredException>(() => sut.Create(contact));
            mockContactRepository.Verify(x => x.Create(It.IsAny<Contact>()), Times.Never);
        }

        [TestMethod]
        public void Create_WithBlankMobilePhone_ThrowsMobilePhoneRequiredException()
        {
            //Arrange
            contact.MobilePhone = "";

            //Act

            //Asset
            Assert.ThrowsException<MobilePhoneRequiredException>(() => sut.Create(contact));
            mockContactRepository.Verify(x => x.Create(It.IsAny<Contact>()), Times.Never);
        }

        [TestMethod]
        public void Create_WithBlankStreetAddress_ThrowsAddressRequiredException()
        {
            //Arrange
            contact.StreetAddress = "";

            //Act

            //Asset
            Assert.ThrowsException<AddressRequiredException>(() => sut.Create(contact));
            mockContactRepository.Verify(x => x.Create(It.IsAny<Contact>()), Times.Never);
        }

        [TestMethod]
        public void Create_WithBlankCityAddress_ThrowsAddressRequiredException()
        {
            //Arrange
            contact.CityAddress = "";

            //Act

            //Asset
            Assert.ThrowsException<AddressRequiredException>(() => sut.Create(contact));
            mockContactRepository.Verify(x => x.Create(It.IsAny<Contact>()), Times.Never);
        }

        [TestMethod]
        public void Create_WithBlankZipCode_ThrowsAddressRequiredException()
        {
            //Arrange
            contact.ZipCode = 0;

            //Act

            //Asset
            Assert.ThrowsException<AddressRequiredException>(() => sut.Create(contact));
            mockContactRepository.Verify(x => x.Create(It.IsAny<Contact>()), Times.Never);
        }

        [TestMethod]
        public void Create_WithNegativeZipCode_ThrowsNagativeZipCodeException()
        {
            //Arrange
            contact.ZipCode = -11;

            //Act

            //Asset
            Assert.ThrowsException<NagativeZipCodeException>(() => sut.Create(contact));
            mockContactRepository.Verify(x => x.Create(It.IsAny<Contact>()), Times.Never);
        }

        [TestMethod]
        public void Create_WithBlankCountry_ThrowsAddressRequiredException()
        {
            //Arrange
            contact.Country = "";

            //Act

            //Asset
            Assert.ThrowsException<AddressRequiredException>(() => sut.Create(contact));
            mockContactRepository.Verify(x => x.Create(It.IsAny<Contact>()), Times.Never);
        }

        [DataTestMethod]
        [DataRow("cmanuel.blastasia.com")]
        [DataRow("cmanuel@blastasiacom")]
        [DataRow("cmanuel@@blast.asia.com.")]
        [TestMethod]
        public void Create_InvalidEmailAddress_ThrowsInvalidEmailAddressException(string email)
        {
            //Arrange
            contact.EmailAddress = email;

            //Act

            //Assert
            Assert.ThrowsException<InvalidEmailAddressException>(() => sut.Create(contact));
            mockContactRepository.Verify(x => x.Create(It.IsAny<Contact>()), Times.Never);
        }
    }
}
