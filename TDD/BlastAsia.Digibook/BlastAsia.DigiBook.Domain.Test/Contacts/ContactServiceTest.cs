using BlastAsia.DigiBook.Domain.Contacts;
using BlastAsia.DigiBook.Domain.Contacts.Exception;
using BlastAsia.DigiBook.Domain.Models.Contacts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;


namespace BlastAsia.DigiBook.Domain.Test.Contacts
{
    [TestClass]
    public class ContactServiceTest
    {
        // global variables
        private Mock<IContactRepository> mockContactRepository;
        private ContactService sut;
        private Contact contact;

        private Guid existingContactId = Guid.NewGuid();
        private Guid nonExistingContactId = Guid.Empty;

        [TestInitialize]
        public void Initialize()
        {
            contact = new Contact
            {
                FirstName = "Emem",
                LastName = "Magadia",
                MobilePhone = "09751918607",
                StreetAddress = "#245, Mayuro Rosario Batangas",
                CityAddress = "Batangas City",
                ZipCode = 4225,
                Country = "Philippines",
                EmailAddress = "emmanuelmagadia@outlook.com",
                IsActive = false,
                DateActivated = new Nullable<DateTime>()
            };

            mockContactRepository = new Mock<IContactRepository>(); // mock the depedencies

            mockContactRepository
                .Setup(c => c.Create(contact))
                    .Callback(() =>
                    {
                        contact.ContactId = Guid.NewGuid();
                        contact.DateActivated = DateTime.Now;
                    })
                    .Returns(contact);
            // if  existing
            mockContactRepository
                .Setup(c => c.Retrieve(existingContactId))
                    .Returns(contact);
            // if non existing
            mockContactRepository   
                .Setup(c => c.Retrieve(nonExistingContactId))
                    .Returns<Contact>(null);


            sut = new ContactService(mockContactRepository.Object); // set up the System under test



        }
        [TestCleanup]
        public void CleanUp()
        {

        }
        [TestMethod]
        public void Save_NewContactWithValidData_ShouldCallRepositoryCreate()
        {
            // Act
            var result = sut.Save(contact); // invoke sut method to invoke the depedencies for verifying
            // Assert
            mockContactRepository
                .Verify(cr => cr.Retrieve(nonExistingContactId), Times.Once());

            mockContactRepository
                .Verify(cr => cr.Create(contact), Times.Once());
        }
        [TestMethod]
        public void Save_WithExistingContact_ShouldCallsRepositoryUpdate()
        {
            // Arrange
            contact.ContactId = existingContactId;
            // act
            sut.Save(contact);
            // assert
            mockContactRepository
                .Verify(cr => cr.Retrieve(existingContactId), Times.Once);

            mockContactRepository
                .Verify(cr => cr.Update(existingContactId, contact), Times.Once);
        }
        [TestMethod]
        public void Save_WithValidData_ReturnsNewContactWithCOntactId()
        {
            // act 
            var newContact = sut.Save(contact);
            // assert
            Assert.IsTrue(newContact.ContactId != Guid.Empty);

        }

        [TestMethod]
        [Description("Valid Account Testing")]
        public void Save_WithBlankFirstName_ThrowsNameRequiredException()
        {
            // arrange 
            contact.FirstName = "";
            // act

            // assert
            Assert.ThrowsException<NameRequiredException>(
                    () => sut.Save(contact)
                );

            mockContactRepository
                .Verify(cr => cr.Create(contact), Times.Never());

        }

        [TestMethod]
        [Description("Valid Account Testing")]
        public void Save_WithBlankLastName_ThrowsNameRequiredException()
        {
            // arrange
            contact.LastName = "";

            // assert
            Assert.ThrowsException<NameRequiredException>(
                () => sut.Save(contact)
                );

            mockContactRepository
                    .Verify(cr => cr.Create(contact), Times.Never());
        }

        [TestMethod]
        [Description("Valid Account Testing")]
        public void Save_WithBlankMobilePhone_ThrowsMobilePhoneRequiredException()
        {
            // arrange
            contact.MobilePhone = "";
            // assert
            Assert.ThrowsException<MobilePhoneRequiredException>(
                () => sut.Save(contact)
                );

            mockContactRepository
                    .Verify(cr => cr.Create(contact), Times.Never());
        }

        [TestMethod]
        [Description("Valid Account Testing")]
        public void Save_WithBlankStreetAddress_ThrowsStreetAddressRequiredException()
        {
            // arrange
            contact.StreetAddress = "";
            // assert
            Assert.ThrowsException<StreetAddressRequiredException>(
                () => sut.Save(contact)
                );
            mockContactRepository
                    .Verify(cr => cr.Create(contact), Times.Never());
        }

        [TestMethod]
        [Description("Valid Account Testing")]
        public void Save_WithBlankCityAddress_ThrowsCityAddressRequiredException()
        {
            // arrange
            contact.CityAddress = "";
            // assert
            Assert.ThrowsException<CityAddressRequiredException>(
                () => sut.Save(contact)
                );
            mockContactRepository
                    .Verify(cr => cr.Create(contact), Times.Never());
        }

        [TestMethod]
        [Description("Valid Account Testing")]
        public void Save_WithBlankZipcode_ThrowsZipCodeRequiredException()
        {
            // arrange
            contact.ZipCode = 0;
            // assert
            Assert.ThrowsException<ZipCodeRequiredException>(
                () => sut.Save(contact)
                );
            mockContactRepository
                    .Verify(cr => cr.Create(contact), Times.Never());
        }

        [TestMethod]
        [Description("Valid Account Testing")]
        public void Save_WithNegativeZipCode_ThrowsNegativeZipCodeException()
        {
            // arrange
            contact.ZipCode = -1;
            // assert
            Assert.ThrowsException<NegativeZipCodeException>(
                () => sut.Save(contact)
                );
            mockContactRepository
                    .Verify(cr => cr.Create(contact), Times.Never());
        }

        [TestMethod]
        [Description("Valid Account Testing")]
        public void Save_WithBlankCountry_ThrowsBlankCountryException()
        {
            // arrange
            contact.Country = " ";
            // assert
            Assert.ThrowsException<CountryRequiredException>(
                () => sut.Save(contact)
                );
            mockContactRepository
                    .Verify(cr => cr.Create(contact), Times.Never());
        }

        [DataTestMethod]
        [DataRow("emagadiablastasiacom")]
        [DataRow("emagadia@blastasia")]
        [DataRow("emagadia.blastasia.com")]
        [Description("Valid Account Testing")]
        public void Save_WithInvalidEmail_ThrowsInvalidEmailException(string email)
        {
            // arrange
            contact.EmailAddress = email;
            // assert
            Assert.ThrowsException<InvalidEmailException>(
                () => sut.Save(contact)
                );
            mockContactRepository
                    .Verify(cr => cr.Create(contact), Times.Never());
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [Description("Valid Account Testing")]
        public void Save_WithBlankEmail_ShouldInvokeRepoCreate(string email)
        {
            // arrange
            contact.EmailAddress = email;
            // assert
            var result = sut.Save(contact);

            mockContactRepository
                    .Verify(cr => cr.Create(contact), Times.Exactly(1));
        }

        [TestMethod]
        [Description("Valid Account Testing")]
        public void Save_WithNullDate_ShouldInvokeRepoCreate()
        {
            // arrange
            contact.DateActivated = null;
            // assert
            var result = sut.Save(contact);

            mockContactRepository
                    .Verify(cr => cr.Create(contact), Times.Exactly(1));
        }

    }

}
