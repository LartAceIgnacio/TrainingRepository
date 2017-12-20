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
        private Contact contact;
        private string connectionString;
        private DbContextOptions<DigiBookDbContext> dbOptions = null;
        private ContactRepository sut;
        private DigiBookDbContext dbContext;
       // private Contact newContact;
        [TestInitialize()]
        public void Initialize()
        {
            contact = new Contact
            {
                FirstName = "Ryan",
                LastName = "Oribello",
                MobilePhone = "09264709989",
                StreetAddress = "43 Nueva Vizcaya St. Bago-Bantay Quezon City",
                CityAddress = "Quezon City",
                ZipCode = 1105,
                Country = "Philippines",
                EmailAddress = "oribelloryan@gmail.com",
                IsActive = false,
                DateActivated = new Nullable<DateTime>()
            };

            connectionString =
               @"Data Source=.; Database=DigiBookDb; Integrated Security=true";

            dbOptions = new DbContextOptionsBuilder<DigiBookDbContext>()
                .UseSqlServer(connectionString)
                .Options;
            dbContext = new DigiBookDbContext(dbOptions);
            dbContext.Database.EnsureCreated();
            sut = new ContactRepository(dbContext);

       
        }
        [TestCleanup()]
        public void CleanUp()
        {

        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Create_WithValidData_SaveRecordInTheDatabase()
        {
            //Arrange

            //Act
            var newContact = sut.Create(contact);

            //Assert
            Assert.IsNotNull(contact);
            Assert.IsTrue(newContact.ContactId != Guid.Empty);

            //CleanUp
            sut.Delete(newContact.ContactId);

        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Delete_WithAnExistingContact_RemovesRecordFromDatabase()
        {
            var newContact = sut.Create(contact);
            //Act
            sut.Delete(newContact.ContactId);

            contact = sut.Retrieve(newContact.ContactId);
            Assert.IsNull(contact);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Retrieve_WithExistingContactId_ReturnsRecordFromDb()
        {
  
           var newContact = sut.Create(contact);

           var found = sut.Retrieve(newContact.ContactId);

            Assert.IsNotNull(found);
            sut.Delete(newContact.ContactId);

        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Update_WithValidData_SaveUpdatesInDb()
        {
            var newContact = sut.Create(contact);
            var expectedFirstName = "Linus";
            var expectedLastName = "Paul";
            var expectedEmail = "linusPauling@gmail.com";
            var expectedCityAddress = "Dadadasdad sdffass";
            var expectedCountry = "Uganda";
            


            newContact.FirstName = expectedFirstName;
            newContact.LastName = expectedLastName;
            newContact.EmailAddress = expectedEmail;

            sut.Update(newContact.ContactId, newContact);

            var updatedContract = sut.Retrieve(newContact.ContactId);
            Assert.AreEqual(expectedFirstName, updatedContract.FirstName);
            Assert.AreEqual(expectedLastName, updatedContract.LastName);
            Assert.AreEqual(expectedEmail, updatedContract.EmailAddress);


            sut.Delete(updatedContract.ContactId);
        }
    }
}
