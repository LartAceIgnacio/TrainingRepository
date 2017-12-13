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
        private Mock<IContactRepository> mockContactRepository;
        private ContactService sut;
        private Contact contact;
        [TestInitialize]
        public void InitializeTest()
        {

            mockContactRepository = new Mock<IContactRepository>();
            sut = new ContactService(mockContactRepository.Object);

            contact = new Contact
                {
                    FirstName = "Renz",
                    LastName = "Saberon",
                    MobilePhone = "09123456789",
                    StreetAddress = "35 Bataan Road, Garcia Heights, Holy Spirit",
                    CityAddress = "Quezon City",
                    ZipCode = 1127,
                    Country = "Philippines",
                    EmailAddress = "renznebran@gmail.com",
                    IsActive = false,
                    DateActivated = new Nullable<DateTime>()
                };

        }
      

        [TestMethod]
        public void Create_WithValidData_ShouldCallRepositoryCreate()
        {
          
            //Act
            var result = sut.Create(contact);

            mockContactRepository
                .Verify(c => c.Create(contact)
                , Times.Once());

            //Assertl
        }

        [TestMethod]
        public void Create_WithValidData_ReturnsNewContactWithContactId()
        {
            //Act

            mockContactRepository
                .Setup(c => c.Create(contact))
                .Callback(() => contact.ContactId = Guid.NewGuid())
                  .Returns(contact);
            var newContact = sut.Create(contact);


            //Assert
            Assert.IsTrue(newContact.ContactId != Guid.Empty);

        }

        [TestMethod]
        public void Create_WithBlankFirstName_ThrowsNameRequiredException()
        {
            

            contact.FirstName = "";
           
            Assert.ThrowsException<NameRequiredException>(
                ()=> sut.Create(contact));

            mockContactRepository
                        .Verify(c => c.Create(contact), Times.Never());


        }
        [TestMethod]
        public void Create_WithBlankLastName_ThrowsNameRequiredException()
        {
            contact.LastName = "";

            Assert.ThrowsException<NameRequiredException>(
               ()=> sut.Create(contact));

            mockContactRepository
                    .Verify(c => c.Create(contact), Times.Never);
        }
        [TestMethod]
        public void Create_WithBlankMobilePhone_ThrowsMobilePhoneRequiredExecption()
        {
            contact.MobilePhone = "";

            Assert.ThrowsException<MobilePhoneRequiredException>(
                ()=> sut.Create(contact));

            mockContactRepository
                .Verify(c => c.Create(contact), Times.Never);
        }

        [TestMethod]
        public void Create_WithBlankStreetAddress_ThrowsAddressRequiredException()
        {
            contact.StreetAddress = "";

            Assert.ThrowsException<AddressRequiredException>(
                ()=> sut.Create(contact));

            mockContactRepository
                .Verify(cp => cp.Create(contact), Times.Never);
        }

        [TestMethod]
        public void Create_WithNegativeZipCode_ThrowsAddressRequiredException()
        {
            contact.ZipCode =-12;

            Assert.ThrowsException<AddressRequiredException>(
                ()=> sut.Create(contact));

            mockContactRepository
                .Verify(cp => cp.Create(contact), Times.Never);
        }

        [TestMethod]
        public void Create_WithBlankCountry_ThrowsAddressRequiredException()
        {
            contact.Country = "";

            Assert.ThrowsException<AddressRequiredException>(
                ()=> sut.Create(contact));

            mockContactRepository
                .Verify(cp => cp.Create(contact), Times.Never);
        }

        [DataTestMethod]
        [DataRow("renznebran.com")]
        [DataRow("renznebran@yahoo..com")]
        [DataRow("renznebran@yahoo.com.")]
        [DataRow("renznebran@@yahoo.com")]
        [DataRow("renznebran@yahoocom")]
        [TestMethod]
        public void Create_WithInvalidEmailAddress_ThrowsInvalidEmailRequiredException(string EmailAddress)
        {

            contact.EmailAddress = EmailAddress;
            Assert.ThrowsException<InvalidEmailAddressException>(
                ()=>sut.Create(contact));

            mockContactRepository
                .Verify(cp => cp.Create(contact), Times.Never);
        }

        [DataTestMethod]
        [DataRow(" ")]
        [DataRow("")]
        [DataRow(null)]
        [TestMethod]
        public void Create_WithBlankEmailAddress_ShouldStillContinue(string emailAdd)
        {
            contact.EmailAddress = emailAdd;

            sut.Create(contact);

            mockContactRepository
                .Verify(cp => cp.Create(contact), Times.AtLeastOnce);
        }

        [TestMethod]
        public void Create_WithNullDateActivated_ShouldStillContinue()
        {
            contact.DateActivated = null;

            sut.Create(contact);

            mockContactRepository
                .Verify(cp => cp.Create(contact), Times.AtLeastOnce);
        }
    }
}
