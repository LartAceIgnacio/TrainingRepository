using BlastAsia.DigiBook.Domain.Contacts;
using BlastAsia.DigiBook.Domain.Contacts.Exceptions;
using BlastAsia.DigiBook.Domain.Contacts.Interfaces;
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
        //Declaring global variables 
        private Mock<IContactRepository> mockContactRepository; 
        private ContactService sut;
        private Contact contact;
        private Guid existingContactId = Guid.NewGuid();
        private Guid nonExistingContactId = Guid.Empty;

        [TestInitialize]
        public void InitializeTest()
        {
            //Instantiating the objects
            mockContactRepository = new Mock<IContactRepository>();
            sut = new ContactService(mockContactRepository.Object);

            contact = new Contact
                {
                    FirstName = "Renz",
                    LastName = "Nebran",
                    MobilePhone = "09123456789",
                    StreetAddress = "35 Bataan Road, Garcia Heights, Holy Spirit",
                    CityAddress = "Quezon City",
                    ZipCode = 1127,
                    Country = "Philippines",
                    EmailAddress = "renznebran@gmail.com",
                    IsActive = false,
                    DateActivated = new Nullable<DateTime>()
                };

            mockContactRepository
                .Setup(c => c.Retrieve(existingContactId))
                .Returns(contact);

            mockContactRepository
                .Setup(c => c.Retrieve(nonExistingContactId))
                .Returns<Contact>(null);

        }

        #region TestMethod_Save

        [TestMethod]
        public void Save_NewContactWithValidData_ShouldCallRepositoryCreate()
        {
            
            //Act
            var result = sut.Save(contact);

            mockContactRepository
                .Verify(c => c.Retrieve(nonExistingContactId)
                , Times.Once);
            
            mockContactRepository
                .Verify(c => c.Create(contact)
                , Times.Once());

            //Assertl
        }

        [TestMethod]
        public void Save_WithExistingContact_ShouldCallRepositoryUpdate()
        {
            //Arrange
            contact.ContactId = existingContactId;

            //Act
            sut.Save(contact);

            //Assert
            mockContactRepository
                .Verify(c => c.Retrieve(existingContactId)
                , Times.Once);

            mockContactRepository
                .Verify(c => c.Update(existingContactId, contact)
                , Times.Once);

        }

        [TestMethod]
        public void Save_WithValidData_ReturnsNewContactWithContactId()
        {
            //Act

            mockContactRepository
                .Setup(c => c.Create(contact))
                .Callback(() => contact.ContactId = Guid.NewGuid())
                    .Returns(contact);
            var newContact = sut.Save(contact);


            //Assert
            Assert.IsTrue(newContact.ContactId != Guid.Empty);

        }

        [TestMethod]
        public void Save_WithBlankFirstName_ThrowsNameRequiredException()
        {
            

            contact.FirstName = "";
           
            Assert.ThrowsException<NameRequiredException>(
                ()=> sut.Save(contact));

            mockContactRepository
                        .Verify(c => c.Create(contact), Times.Never());


        }
        [TestMethod]
        public void Save_WithBlankLastName_ThrowsNameRequiredException()
        {
            contact.LastName = "";

            Assert.ThrowsException<NameRequiredException>(
               ()=> sut.Save(contact));

            mockContactRepository
                    .Verify(c => c.Create(contact), Times.Never);
        }
        [TestMethod]
        public void Save_WithBlankMobilePhone_ThrowsMobilePhoneRequiredExecption()
        {
            contact.MobilePhone = "";

            Assert.ThrowsException<MobilePhoneRequiredException>(
                ()=> sut.Save(contact));

            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never);
        }

        [TestMethod]
        public void Save_WithBlankStreetAddress_ThrowsAddressRequiredException()
        {
            contact.StreetAddress = "";

            Assert.ThrowsException<AddressRequiredException>(
                ()=> sut.Save(contact));

            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never);

        }

        [TestMethod]
        public void Save_WithNegativeZipCode_ThrowsAddressRequiredException()
        {
            contact.ZipCode =-12;

            Assert.ThrowsException<AddressRequiredException>(
                ()=> sut.Save(contact));

            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never);
        }

        [TestMethod]
        public void Save_WithBlankCountry_ThrowsAddressRequiredException()
        {
            contact.Country = "";

            Assert.ThrowsException<AddressRequiredException>(
                ()=> sut.Save(contact));

            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never);
        }

        [DataTestMethod]
        [DataRow("renznebran.com")]
        [DataRow("renznebran@yahoo..com")]
        [DataRow("renznebran@yahoo.com.")]
        [DataRow("renznebran@@yahoo.com")]
        [DataRow("renznebran@yahoocom")]
        [TestMethod]
        public void Save_WithInvalidEmailAddress_ThrowsInvalidEmailRequiredException(string EmailAddress)
        {

            contact.EmailAddress = EmailAddress;
            Assert.ThrowsException<InvalidEmailAddressException>(
                ()=>sut.Save(contact));

            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never);
        }

        [DataTestMethod]
        [DataRow(" ")]
        [DataRow("")]
        [DataRow(null)]
        [TestMethod]
        public void Save_WithBlankEmailAddress_ShouldStillContinue(string emailAdd)
        {
            contact.EmailAddress = emailAdd;

            sut.Save(contact);

            mockContactRepository
                .Verify(c => c.Create(contact), Times.AtLeastOnce);
        }

        [TestMethod]
        public void Save_WithNullDateActivated_ShouldStillContinue()
        {
            contact.DateActivated = null;

            sut.Save(contact);

            mockContactRepository
                .Verify(c => c.Create(contact), Times.AtLeastOnce);
        }

        #endregion
    }
}
