using BlastAsia.DigiBook.Domain.Models.Contacts;
using BlastAsia.DigiBook.Infrastracture.Persistence.Repositories;
using BlastAsia.DigiBook.Insfrastracture.Persistence;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using System;

namespace BlastAsia.DigiBook.Infrastracture.Persistence.Test
{
    [TestClass]
    public class ContactRepositoryTest
    {
        private Contact contact;
        private DbContextOptions<DigiBookDbContext> dbOptions;
        private DigiBookDbContext dbContext;
        private readonly string connectionString = @"Data Source=.; Database=DigiBookDb; Integrated Security=true;";
        private ContactRepository sut;

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
                IsActive = true,
                DateActivated = DateTime.Now
            };

            dbOptions = new DbContextOptionsBuilder<DigiBookDbContext>()
                                   .UseSqlServer(connectionString)
                                   .Options;

            dbContext = new DigiBookDbContext(dbOptions); // ORM
            dbContext.Database.EnsureCreated();
            sut = new ContactRepository(dbContext); // System under test

        }

        [TestCleanup]
        public void Cleanup()
        {
            dbContext.Dispose();
            dbContext = null;
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Create_WithValidData_SavesRecordToDatabase()
        {
            // arrange
            // var sut = new ContactRepository(dbContext); // System under test

            // act 
            var newContact = sut.Create(contact);

            // assert
            Assert.IsNotNull(newContact);
            Assert.IsTrue(newContact.ContactId != Guid.Empty);

            // Cleanup
            sut.Delete(newContact.ContactId);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Delete_WithAnExistingContact_RemovesRecordFromDatabase()
        {
            // arrange 
            // var sut = new ContactRepository(dbContext); // System under test
            var newContact = sut.Create(contact);

            // act 
            sut.Delete(newContact.ContactId);
            // assert
            contact = sut.Retrieve(newContact.ContactId);
            Assert.IsNull(contact);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Retrieve_WithExistingContactId_ReturnsRecordFromDatabase()
        {
            // arrange
            var newContact = sut.Create(contact);
            //act
            var found = sut.Retrieve(newContact.ContactId);
            // assert 
            Assert.IsNotNull(found);

            sut.Delete(newContact.ContactId);
        }

        [TestMethod]
        public void Retrieve_WithPaginationWithValidData_ReturnsRecordFromDatabase()
        {
            // arrange
            var newContact = sut.Create(contact);
            var pageNumber = 1;
            var recordNumber = 5;
            var keyWord = "em";
            // act 
            var found = sut.Retrieve(pageNumber,recordNumber, keyWord);
            // assert
            Assert.IsNotNull(found);

            sut.Delete(newContact.ContactId);
        }


        [TestMethod]
        public void Retrieve_WithInvalidKeyWord_ReturnsDefaultRecordFromDataBase()
        {
            // arrange
            var newContact = sut.Create(contact);
            var pageNumber = 1;
            var recordNumber = 5;
            var keyWord = "";
            // act 
            var found = sut.Retrieve(pageNumber, recordNumber, keyWord);
            // assert
            Assert.IsNotNull(found);

            sut.Delete(newContact.ContactId);
        }


        [TestMethod]
        public void Retrieve_WithInvalidPageNumber_ReturnsDefaultRecordFromDataBase()
        {
            // arrange
            var newContact = sut.Create(contact);
            var pageNumber = -1;
            var recordNumber = 5;
            var keyWord = "em";
            // act 
            var found = sut.Retrieve(pageNumber, recordNumber, keyWord);
            // assert
            Assert.IsNotNull(found);

            sut.Delete(newContact.ContactId);
        }

        [TestMethod]
        public void Retrieve_WithInvalidRecordNumber_ReturnsDefaultRecordFromDataBase()
        {
            // arrange
            var newContact = sut.Create(contact);
            var pageNumber = 1;
            var recordNumber = -5;
            var keyWord = "em";
            // act 
            var found = sut.Retrieve(pageNumber, recordNumber, keyWord);
            // assert
            Assert.IsNotNull(found);

            sut.Delete(newContact.ContactId);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Update_WithExistingContactId_SaveAndUpdateInDatabase()
        {
            //arrange
            var newContact = sut.Create(contact);

            var expectedFirstName = "Kyrie"; // from emem
            var expectedLastName = "Irving";
            var expectedEmail = "kyrie@yahoo.com";
            var expectedMobileNumber = "09279528841";
            var expectedStreetAddress = "#246, Mayuro Rosario Batangas";
            var expectedCityAddress = "Makati City";
            var expectedZipCode = 4226;
            var expectedCountry = "USA";
            var expectedIsActive = false;
            var expectedDateActivated = DateTime.Now.AddHours(1);

            newContact.FirstName = expectedFirstName;
            newContact.LastName = expectedLastName;
            newContact.EmailAddress = expectedEmail;
            newContact.MobilePhone = expectedMobileNumber;
            newContact.StreetAddress = expectedStreetAddress;
            newContact.CityAddress = expectedCityAddress;
            newContact.ZipCode = expectedZipCode;
            newContact.Country = expectedCountry;
            newContact.IsActive = expectedIsActive;
            newContact.DateActivated = expectedDateActivated;
            // act
            sut.Update(newContact.ContactId, newContact);

            var UpdatedContact = sut.Retrieve(newContact.ContactId);
            // assert 
            Assert.AreEqual(UpdatedContact.FirstName, expectedFirstName);
            Assert.AreEqual(UpdatedContact.LastName , expectedLastName);
            Assert.AreEqual(UpdatedContact.EmailAddress , expectedEmail);
            Assert.AreEqual(UpdatedContact.MobilePhone , expectedMobileNumber);
            Assert.AreEqual(UpdatedContact.StreetAddress , expectedStreetAddress);
            Assert.AreEqual(UpdatedContact.CityAddress , expectedCityAddress);
            Assert.AreEqual(UpdatedContact.ZipCode , expectedZipCode);
            Assert.AreEqual(UpdatedContact.Country , expectedCountry);
            Assert.AreEqual(UpdatedContact.IsActive , expectedIsActive);
            Assert.AreEqual(UpdatedContact.DateActivated , expectedDateActivated);

            // cleanup
            sut.Delete(UpdatedContact.ContactId);
        }
    }
}
