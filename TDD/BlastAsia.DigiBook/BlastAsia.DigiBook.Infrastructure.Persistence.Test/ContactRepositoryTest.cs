using BlastAsia.DigiBook.Domain.Models.Contacts;
using BlastAsia.DigiBook.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Infrastructure.Persistence.Test
{   [TestClass]
    public class ContactRepositoryTest
    {
        [TestInitialize]
        public void InitializeTest()
        {

        }

        [TestCleanup]
        public void CleanupTest()
        {

        }

        [TestMethod]
        [TestProperty("TestType","Integration")]
        public void Create_WithValidData_SavesRecordInTheDatabase()
        {
            // Arrange
            var contact = new Contact
            {
                FirstName = "Duane", 
                LastName = "De Guzman", 
                MobilePhone = "09158959384", 
                StreetAddress = "264 Quezon st Cuyab", 
                CityAddress = "San Pedro, Laguna", 
                ZipCode = 4023, 
                Country = "Philippines",
                EmailAddress = "dlfdeguzman@outlook.com",
                IsActive = false,
                DateActivated = new Nullable<DateTime>()
            };
            var connectionString = @"Data Source=.;Database=DigiBookDb;Integrated Security=true;";
            var dbOptions = new DbContextOptionsBuilder<DigiBookDbContext>()
                .UseSqlServer(connectionString)
                .Options;

           var dbContext = new DigiBookDbContext(dbOptions); // ORM
            dbContext.Database.EnsureCreated();
           var sut = new ContactRepository(dbContext);

           // Act
           var newContact = sut.Create(contact);

           // Assert
           Assert.IsNotNull(newContact);
           Assert.IsTrue(newContact.ContactId != Guid.Empty);
           // Cleanup
           dbContext.Contacts.Remove(newContact);
           dbContext.SaveChanges();
        }
    }

    internal class DbContextOptionBuilder<T>
    {
        public DbContextOptionBuilder()
        {
        }
    }
}
