
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
        private DbContextOptions<DigiBookDbContext> dbOptions = null;
        private DigiBookDbContext dbContext = null;
        private String connectionString = null;
        private ContactRepositories sut;

        [TestInitialize]
        public void Initialize()
        {
            contact = new Contact
            {
                FirstName = "Eugene",
                LastName = "Ravina",
                MobilePhone = "09277109530",        
                EmailAddress = "eravina@blastasia.com",
             
            };

            connectionString =
               @"Data Source=.;Database=DigiBookDb;Integrated Security=true;";
            dbOptions = new DbContextOptionsBuilder<DigiBookDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            dbContext = new DigiBookDbContext(dbOptions);
            dbContext.Database.EnsureCreated();

            sut = new ContactRepositories(dbContext);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            dbContext.Dispose();
            dbContext = null;
        }

        [TestMethod]
        [TestProperty("TestType","Integration")]
        public void Create_WithValidData_SavesRecordInTheDatabase()
        {
           
            //Act
            var newContact = sut.Create(contact);

            //Assert
            Assert.IsNotNull(newContact);
            Assert.IsTrue(newContact.ContactId != Guid.Empty);

            //Cleanup
            sut.Delete(newContact.ContactId);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Delete__WithAnExistingContact_RemovesRecordFromDatabase()
        {
         
             sut = new ContactRepositories(dbContext);
            var newContact = sut.Create(contact);

            // Act

            sut.Delete(newContact.ContactId);

            // Assert
            contact = sut.Retrieve(newContact.ContactId);
            Assert.IsNull(contact);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Retrieve_WithExistingContactId_ReturnsRecordFromDb()
        {
            //Arrange
            var newContact = sut.Create(contact);
           

            //Act
            var found = sut.Retrieve(newContact.ContactId);

            //Assert
            Assert.IsNotNull(found);

            //Cleanup
            sut.Delete(found.ContactId);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Update_WithValidData_SavesUpdatesInDb()
        {
            //Arrange
            var newContact = sut.Create(contact);
            var expectedFirstName = "Vincent"; //from Eugene
            var expectedLastName = "Taguro"; //from Ravina
            var expectedMobilePhone = "064521";
            var expectedEmailAddress = "eugenejhonravina@yahoo.com"; // from eravina@blastasia.com

            newContact.FirstName = expectedFirstName;
            newContact.LastName = expectedLastName;
            newContact.MobilePhone = expectedMobilePhone;
            newContact.EmailAddress = expectedEmailAddress;

            //Act
            sut.Update(newContact.ContactId, newContact);


            //Assert
            var updatedContact = sut.Retrieve(newContact.ContactId);
            Assert.AreEqual(expectedFirstName, updatedContact.FirstName);
            Assert.AreEqual(expectedLastName, updatedContact.LastName);
            Assert.AreEqual(expectedMobilePhone, updatedContact.MobilePhone);
            Assert.AreEqual(expectedEmailAddress, updatedContact.EmailAddress);

            //Cleanup
            sut.Delete(updatedContact.ContactId);
        }
    }
}
