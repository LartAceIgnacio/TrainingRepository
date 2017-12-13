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
        private Mock<IContactRepository> mockContactRepository;
        private ContactService sut;
        private Contact contact;

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

            sut = new ContactService(mockContactRepository.Object);

        }
        [TestCleanup]
        public void CleanUp()
        {

        }
        [TestMethod]
        public void Create_WithValidData_ShouldCallRepositoryCreate()
        {
            // Act
            var result = sut.Create(contact); // invoke sut method to invoke the depedencies for verifying
            // Assert
            mockContactRepository
                .Verify(cr => cr.Create(contact), Times.Once());
        }

        [TestMethod]
        public void Create_WithValidData_ReturnsNewContactWithCOntactId()
        {
            // act 
            var newContact = sut.Create(contact);
            // assert
            Assert.IsTrue(newContact.ContactId != Guid.Empty);

        }

        [TestMethod]
        [Description("Valid Account Testing")]
        public void Create_WithBlankFirstName_ThrowsNameRequiredException()
        {
            // arrange 
            contact.FirstName = "";
            // act

            // assert
            Assert.ThrowsException<NameRequiredException>(
                    () => sut.Create(contact)
                );

            mockContactRepository
         .Verify(cr => cr.Create(contact), Times.Never());
        }

        [TestMethod]
        [Description("Valid Account Testing")]
        public void Create_WithBlankLastName_ThrowsNameRequiredException()
        {
            // arrange
            contact.LastName = "";

            // assert
            Assert.ThrowsException<NameRequiredException>(
                () => sut.Create(contact)
                );

            mockContactRepository
                    .Verify(cr => cr.Create(contact), Times.Never());
        }

        [TestMethod]
        [Description("Valid Account Testing")]
        public void Create_WithBlankMobilePhone_ThrowsMobilePhoneRequiredException()
        {
            // arrange
            contact.MobilePhone = "";
            // assert
            Assert.ThrowsException<MobilePhoneRequiredException>(
                () => sut.Create(contact)
                );

            mockContactRepository
                    .Verify(cr => cr.Create(contact), Times.Never());
        }

        [TestMethod]
        [Description("Valid Account Testing")]
        public void Create_WithBlankStreetAddress_ThrowsStreetAddressRequiredException()
        {
            // arrange
            contact.StreetAddress = "";
            // assert
            Assert.ThrowsException<StreetAddressRequiredException>(
                () => sut.Create(contact)
                );
            mockContactRepository
                    .Verify(cr => cr.Create(contact), Times.Never());
        }

        [TestMethod]
        [Description("Valid Account Testing")]
        public void Create_WithBlankCityAddress_ThrowsCityAddressRequiredException()
        {
            // arrange
            contact.CityAddress = "";
            // assert
            Assert.ThrowsException<CityAddressRequiredException>(
                () => sut.Create(contact)
                );
            mockContactRepository
                    .Verify(cr => cr.Create(contact), Times.Never());
        }

        [TestMethod]
        [Description("Valid Account Testing")]
        public void Create_WithBlankZipcode_ThrowsZipCodeRequiredException()
        {
            // arrange
            contact.ZipCode = 0;
            // assert
            Assert.ThrowsException<ZipCodeRequiredException>(
                () => sut.Create(contact)
                );
            mockContactRepository
                    .Verify(cr => cr.Create(contact), Times.Never());
        }

        [TestMethod]
        [Description("Valid Account Testing")]
        public void Create_WithNegativeZipCode_ThrowsNegativeZipCodeException()
        {
            // arrange
            contact.ZipCode = -1;
            // assert
            Assert.ThrowsException<NegativeZipCodeException>(
                () => sut.Create(contact)
                );
            mockContactRepository
                    .Verify(cr => cr.Create(contact), Times.Never());
        }

        [TestMethod]
        [Description("Valid Account Testing")]
        public void Create_WithBlankCountry_ThrowsBlankCountryException()
        {
            // arrange
            contact.Country = " ";
            // assert
            Assert.ThrowsException<CountryRequiredException>(
                () => sut.Create(contact)
                );
            mockContactRepository
                    .Verify(cr => cr.Create(contact), Times.Never());
        }

        [DataTestMethod]
        [DataRow("emagadiablastasiacom")]
        [DataRow("emagadia@blastasia")]
        [DataRow("emagadia.blastasia.com")]
        [Description("Valid Account Testing")]
        public void Create_WithInvalidEmail_ThrowsInvalidEmailException(string email)
        {
            // arrange
            contact.EmailAddress = email;
            // assert
            Assert.ThrowsException<InvalidEmailException>(
                () => sut.Create(contact)
                );
            mockContactRepository
                    .Verify(cr => cr.Create(contact), Times.Never());
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [Description("Valid Account Testing")]
        public void Create_WithBlankEmail_ShouldInvokeRepoCreate(string email)
        {
            // arrange
            contact.EmailAddress = email;
            // assert
            var result = sut.Create(contact);

            mockContactRepository
                    .Verify(cr => cr.Create(contact), Times.Exactly(1));
        }

        [TestMethod]
        [Description("Valid Account Testing")]
        public void Create_WithNullDate_ShouldInvokeRepoCreate()
        {
            // arrange
            contact.EmailAddress = null;
            // assert
            var result = sut.Create(contact);

            mockContactRepository
                    .Verify(cr => cr.Create(contact), Times.Exactly(1));
        }

    }

}
