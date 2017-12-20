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

        private Mock<IContactRepository> mockContactRepository;
        private ContactService sut;
        private Contact contact;
        private Guid existingContactId = Guid.NewGuid();
        private Guid nonExistingContactId = Guid.Empty;

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

            mockContactRepository
                .Setup(c => c.Create(contact))
                .Callback(() =>
                {
                    contact.ContactId = Guid.NewGuid();
                    contact.DateActive = DateTime.Now;
                })
                .Returns(contact);

            mockContactRepository
                .Setup(c => c.Retrieve(existingContactId))
                    .Returns(contact);
            // if non existing
            mockContactRepository
                .Setup(c => c.Retrieve(nonExistingContactId))
                    .Returns<Contact>(null);
        }

        [TestMethod]
        public void Save_NewContactWithValidData_ShouldCallRepositoryCreate()
        {
            var result = sut.Save(nonExistingContactId,contact);
            
            mockContactRepository
                .Verify(c => c.Retrieve(nonExistingContactId), Times.Once);

            mockContactRepository
                .Verify(c => c.Create(contact), Times.Once);
        }

        [TestMethod]
        public void Save_WithExistingContact_CallsRepositoryUpdate()
        {
            contact.ContactId = existingContactId;
            sut.Save(existingContactId,contact);

            mockContactRepository
                .Verify(c => c.Retrieve(contact.ContactId), Times.Once);
            mockContactRepository
                .Verify(c => c.Update(contact.ContactId, contact), Times.Once);

        }

        [TestMethod]
        public void Save_WithValidData_ReturnsNewContactWithContactId()
        {
            mockContactRepository
                .Setup(c => c.Create(contact))
                .Callback(() => contact.ContactId = Guid.NewGuid())
                .Returns(contact);

            sut = new ContactService(mockContactRepository.Object);

            var newContact = sut.Save(nonExistingContactId,contact);

            Assert.IsTrue(newContact.ContactId != Guid.Empty);
        }

        [TestMethod]
        public void Save_WithBlankFirstName_ThrowsNameRequiredException()
        {
            contact.FirstName = "";

            Assert.ThrowsException<NameRequiredException>(()=> sut.Save(existingContactId,contact));
            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
        }

        [TestMethod]
        public void Save_WithBlankLastName_ThrowsNameRequiredException()
        {
            contact.LastName = "";
            Assert.ThrowsException<NameRequiredException>(() => sut.Save(existingContactId,contact));
            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
        }

        [TestMethod]
        public void Save_WithBlankMobilePhone_ThrowsMobilePhoneRequiredException()
        {
            contact.MobilePhone = "";
            Assert.ThrowsException<MobilePhoneRequiredException>(() => sut.Save(existingContactId,contact));
            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
        }

        [TestMethod]
        public void Save_WithBlankStreetAddress_ThrowsAddressRequiredException()
        {
            contact.StreetAddress = "";
            Assert.ThrowsException<AddressRequiredException>(() => sut.Save(existingContactId,contact));
            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
        }

        [TestMethod]
        public void Save_WithBlankCityAddress_ThrowsAddressRequiredException()
        {
            contact.CityAddress = "";
            Assert.ThrowsException<AddressRequiredException>(() => sut.Save(existingContactId,contact));
            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
        }

        [TestMethod]
        public void Save_WithNonPositiveNumberZip_ThrowsZipNegativeNumberException()
        {
            contact.ZipCode = -1;
            Assert.ThrowsException<ZipNegativeNumberException>(() => sut.Save(existingContactId,contact));
            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
        }

        [TestMethod]
        public void Save_WithBlankCountry_ThrowsAddressRequiredException()
        {
            contact.Country = "";
            Assert.ThrowsException<AddressRequiredException>(() => sut.Save(existingContactId,contact));
            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never());
        }
    }
}
