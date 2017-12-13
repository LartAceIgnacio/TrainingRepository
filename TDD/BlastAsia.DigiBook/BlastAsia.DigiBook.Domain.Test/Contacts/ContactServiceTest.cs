using BlastAsia.DigiBook.Domain.Contacts;
using BlastAsia.DigiBook.Domain.Test.Contacts.Contacts;
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
        private Mock<IContactRepository> mockContactRepository;
        ContactService sut;
        private Contact contact;
        

        [TestInitialize]
        public void initializeTest()
        {
            mockContactRepository = new Mock<IContactRepository>(); //Contact Object

            sut = new ContactService(mockContactRepository.Object);

            contact = new Contact // Contact
            {
                FirstName = "Igi",
                LastName = "Abille",
                MobilePhone = "09568717617",
                StreetAddress = "B-10 L7 Narra St. North Ridge Park Subd., Sta. Monica, Nova.",
                CityAddress = "Quezon City",
                ZipCode = 1117,
                Country = "Philippines",
                EmailAddress = "luigiabille@gmail.com",
                IsActive = false,
                DateActivated = new Nullable<DateTime>()
            };
        }
        [TestMethod]
        public void Create_WithValidData_ShouldCallRepositoryCreate()
        {
            // Arrange

            // Act

            var result = sut.Create(contact);

            // Assert

            mockContactRepository
                .Verify(c => c.Create(contact),
                Times.Once());

        }
        [TestMethod]
        public void Create_WithValidData_ReturnsNewContactWithContactId()
        {
            // Arrange

            mockContactRepository
                .Setup(c => c.Create(contact))
                .Callback(() => contact.ContactId = Guid.NewGuid())
                .Returns(contact);

            //public void SetContactId(Contact contact) //Instead of .Callback()
            //{
            //    contact.ContactId = Guid.NewGuid();
            //    Action<int, int> add = (a,b) => Console.WriteLine(a +b); // Action returns void
            //}

            // Act

            var newContact = sut.Create(contact);

            // Assert

            Assert.IsTrue(newContact.ContactId != Guid.Empty);
        }
        [TestMethod]
        public void Create_WithBlankFirstName_ThrowsNameRequiredException()
        {

            // Arrange

            contact = new Contact
            {
                FirstName = ""
            };

            // Act

            // Assert

            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
            Assert.ThrowsException<NameRequiredException>(
                () => sut.Create(contact));
        }
        [TestMethod]
        public void Create_WithBlankLastName_ThrowsNameRequiredException()
        {

            // Arrange

            contact.LastName = "";

            // Act

            // Assert
                       
            Assert.ThrowsException<NameRequiredException>(
                () => sut.Create(contact));
            mockContactRepository
               .Verify(c => c.Create(contact), Times.Never());
        }
        [TestMethod]
        public void Create_WithBlankMobilePhone_ThrowsMobilePhoneRequiredException()
        {

            // Arrange

            contact.MobilePhone = "";

            // Act

            // Assert

            Assert.ThrowsException<MobilePhoneRequiredException>(
                () => sut.Create(contact));
            mockContactRepository
               .Verify(c => c.Create(contact), Times.Never());
        }
        [TestMethod]
        public void Create_WithBlankStreetAddress_ThrowsStreetAddressRequiredException()
        {

            // Arrange

            contact.StreetAddress = "";

            // Act

            // Assert

            Assert.ThrowsException<AddressException>(
                () => sut.Create(contact));
            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
        }
        [TestMethod]
        public void Create_WithBlankCityAddress_ThrowsCityAddressException()
        {

            // Arrange

            contact.CityAddress = "";

            // Act

            // Assert

            Assert.ThrowsException<AddressException>(
                () => sut.Create(contact));
            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
        }
        [TestMethod]
        public void Create_WithNegativeZipCode_ThrowsZipCodeException()
        {

            // Arrange

            contact.ZipCode = -1;

            // Act

            // Assert

            Assert.ThrowsException<ZipCodeException>(
                () => sut.Create(contact));
            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
        }
        [TestMethod]
        public void Create_WithBlankCountry_ThrowsCountryException()
        {

            // Arrange

            contact.Country = "";

            // Act

            // Assert

            Assert.ThrowsException<CountryException>(
                () => sut.Create(contact));
            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
        }
        [TestMethod]
        public void Create_ValidEmail_ThrowsValidEmailRequiredException()
        {
            // Arrange

            contact.EmailAddress = "luigiabillegmail.com";

            // Act

            // Assert
            Assert.ThrowsException<ValidEmailRequiredException>(
                () => sut.Create(contact));
            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
        }

    }
}
