using BlastAsia.DigiBook.Domain.Models.Contacts;
using BlastAsia.DigiBook.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Infrastructure.Persistence.Test
{
    [TestClass]
    public class ContactRepositoryTest
    {
        private Contact contact = null;
        private string connectionString;
        private DbContextOptions<DigiBookDbContext> dbOptions;
        private DigiBookDbContext dbContext;
        private ContactRepository sut;

        [TestInitialize]
        public void Initialize()
        {
            contact = new Contact
            {
                Firstname = "Matt",
                Lastname = "Mendez",
                MobilePhone = "09293235700",
                StreetAddress = "318 Saint Michael St., Brgy. Holy Spirit",
                CityAddress = "Quezon City",
                ZipCode = 1127,
                EmailAddress = "mmendez@blastasia.com",
                IsActive = false,
                DateActivated = new Nullable<DateTime>()
            };

            connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=DigiBookDb;Trusted_Connection=True;";
            dbOptions = new DbContextOptionsBuilder<DigiBookDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            dbContext = new DigiBookDbContext(dbOptions);

            sut = new ContactRepository(dbContext);
            dbContext.Database.EnsureCreated();
        }

        [TestCleanup]
        public void CleanUp()
        {
            dbContext.Dispose();
            dbContext = null;
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Create_WithValidContactData_SavesRecordInTheDatabase ()
        {
            // Arrange
            
            
            // Act
            var newContact = sut.Create(contact);

            // Assert
            Assert.IsNotNull(newContact);
            Assert.IsTrue(newContact.ContactId != Guid.Empty);

            //Cleanup
            sut.Delete(newContact.ContactId);
            
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Delete_WithExistingContact_RemovesDataFromDatabase()
        {
            var newContact = sut.Create(contact);

            sut.Delete(contact.ContactId);

            // Assert
            contact = sut.Retrieve(newContact.ContactId);
            Assert.IsNull(contact);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Retrieve_WithExistingContactId_ReturnsRecordFromDb()
        {
            // Arrange
            var newcontact = sut.Create(contact);

            // Act
            var found = sut.Retrieve(contact.ContactId);

            // Assert
            Assert.IsNotNull(found);

            //CleanUp
            sut.Delete(found.ContactId);


        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Update_WithExistingContactId_ShouldUpdateRecordFromDb()
        {

            var newcontact = sut.Create(contact);
            var expectedFirstName = "James"; //Matt
            var expectedLastName = "Montemagno"; //Mendez
            var expectedEmail = "jmontemagno@xamarin.com"; //mmendez@blastasia.com

            newcontact.Firstname = expectedFirstName;
            newcontact.Lastname = expectedLastName;
            newcontact.EmailAddress = expectedEmail;

            // Act
            sut.Update(newcontact.ContactId, contact);

            //Assert
            var updatedContact = sut.Retrieve(newcontact.ContactId);
            Assert.AreEqual(expectedFirstName, updatedContact.Firstname);
            Assert.AreEqual(expectedLastName, updatedContact.Lastname);
            Assert.AreEqual(expectedEmail, updatedContact.EmailAddress);

            // CleanUp
            sut.Delete(updatedContact.ContactId);

        }
    }
}
