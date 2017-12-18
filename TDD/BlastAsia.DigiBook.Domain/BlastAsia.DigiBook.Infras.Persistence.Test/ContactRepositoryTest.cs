using BlastAsia.DigiBook.Domain.Models.Contacts;
using BlastAsia.DigiBook.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BlastAsia.DigiBook.Infrastracture.Persistence.Test
{
    [TestClass]
    public class ContactRepositoryTest
    {
        private string connectionString;
        
        private Contact contact;
        private ContactRepository sut;
        private DigiBookDbContext dbContext = null;
        private DbContextOptions<DigiBookDbContext> dbOptions = null;

        [TestInitialize]
        public void Initialize()
        {
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
                DateActivated = DateTime.Now
            };


            connectionString = @"Data Source=.;Database=DigiBookDb;Integrated Security=true;";
            dbOptions = new DbContextOptionsBuilder<DigiBookDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            dbContext = new DigiBookDbContext(dbOptions); // ORM
            dbContext.Database.EnsureCreated();
             sut = new ContactRepository(dbContext);

        }

        [TestCleanup]
        public void Cleanup()
        {
            dbContext.Dispose();
            dbContext = null;
        }

        [TestMethod]
        [TestProperty("TestType", "Contact")]
        public void Create_WithValidData_SavesRecordToDatabase()
        {
            // arrange
           


            // act 
            var newContact = sut.Create(contact);



            // assert
            Assert.IsNotNull(newContact);
            Assert.IsTrue(newContact.ContactId != Guid.Empty);

            // Cleanup
            sut.Delete(newContact.ContactId);
        }


        
        [TestMethod]
        [TestProperty("TestType", "Contact")]
        public void Delete_WithAnExistingContactId_RemovesRecordFromDatabase()
        {
            //Arrange
            var newContact = sut.Create(contact);

            //Act
            sut.Delete(newContact.ContactId);

            //Assert

            contact = sut.Retrieve(newContact.ContactId);
            Assert.IsNull(contact);
        }

        [TestMethod]
        [TestProperty("TestType", "Contact")]
        public void Retrieve_WithAnExistingContactId_ReturnsContactRecordsFromDb()
        {
            var newContact = sut.Create(contact);

            var retrieveContact = sut.Retrieve(newContact.ContactId);

            Assert.IsNotNull(retrieveContact);

            sut.Delete(newContact.ContactId);
        }

        [TestMethod]
        [TestProperty("TestType", "Contact")]
        public void Update_WithAnExistingContactId_ShouldUpdateContactRecordsFromDb()
        {
            //Arrange
            var newContact = sut.Create(contact);
            var expectedFirstName = "Julius";

            //changing the value of properties
            newContact.FirstName = expectedFirstName;
            //Act
            sut.Update(newContact.ContactId, newContact);
            var updatedContact = sut.Retrieve(newContact.ContactId);
         
            //Assert   
            Assert.AreEqual(updatedContact.FirstName,expectedFirstName);

            //cleanup
            sut.Delete(updatedContact.ContactId);
        }
    }
}