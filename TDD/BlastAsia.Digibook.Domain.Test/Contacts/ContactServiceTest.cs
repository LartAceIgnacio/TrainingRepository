using BlastAsia.Digibook.Domain.Contacts;
using BlastAsia.Digibook.Domain.Models.Contacts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.Digibook.Domain.Test.Contacts
{
    [TestClass]
    public class ContactServiceTest
    {

        Mock<IContactRepository> mockContactRepository;
        ContactService sut;
        Contact contact;

        [TestInitialize]
        public void InitializeTest()
        {

            mockContactRepository = new Mock<IContactRepository>();
            sut = new ContactService(mockContactRepository.Object);

            contact = new Contact
            {
                FirstName = "Alex",
                LastName = "Cano",
                MobilePhone = "09173723594",
                StreetAddress = "16 J. Cruz St., Parang",
                CityAddress = "Marikina City",
                ZipCode = 1809,
                Country = "Philippines",
                EmailAddress = "gcano@blastasia.com",
                IsActive = false,
                DateActive = new Nullable<DateTime>()
            };
        }

        [TestMethod]
        public void Create_WithValidData_ShouldCallRepositoryCreate()
        {
            var result = sut.Create(contact);

            mockContactRepository
                .Verify(c => c.Create(contact), Times.Once);
        }

        [TestMethod]
        public void Create_WithValidData_ReturnsNewContactWithContactId()
        {
            mockContactRepository
                .Setup(c => c.Create(contact))
                .Callback(() => contact.ContactId = Guid.NewGuid())
                .Returns(contact);

            sut = new ContactService(mockContactRepository.Object);

            var newContact = sut.Create(contact);

            Assert.IsTrue(newContact.ContactId != Guid.Empty);
        }

        [TestMethod]
        public void Create_WithBlankFirstName_ThrowsNameRequiredException()
        {
            contact.FirstName = "";

            Assert.ThrowsException<NameRequiredException>(()=> sut.Create(contact));
            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
        }

        [TestMethod]
        public void Create_WithBlankLastName_ThrowsNameRequiredException()
        {
            contact.LastName = "";
            Assert.ThrowsException<NameRequiredException>(() => sut.Create(contact));
            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
        }

        [TestMethod]
        public void Create_WithBlankMobilePhone_ThrowsMobilePhoneRequiredException()
        {
            contact.MobilePhone = "";
            Assert.ThrowsException<MobilePhoneRequiredException>(() => sut.Create(contact));
            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
        }

        [TestMethod]
        public void Create_WithBlankStreetAddress_ThrowsAddressRequiredException()
        {
            contact.StreetAddress = "";
            Assert.ThrowsException<AddressRequiredException>(() => sut.Create(contact));
            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
        }

        [TestMethod]
        public void Create_WithBlankCityAddress_ThrowsAddressRequiredException()
        {
            contact.CityAddress = "";
            Assert.ThrowsException<AddressRequiredException>(() => sut.Create(contact));
            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
        }

        [TestMethod]
        public void Create_WithNonPositiveNumberZip_ThrowsZipNegativeNumberException()
        {
            contact.ZipCode = -1;
            Assert.ThrowsException<ZipNegativeNumberException>(() => sut.Create(contact));
            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
        }

        [TestMethod]
        public void Create_WithBlankCountry_ThrowsAddressRequiredException()
        {
            contact.Country = "";
            Assert.ThrowsException<AddressRequiredException>(() => sut.Create(contact));
            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
        }
    }
}
