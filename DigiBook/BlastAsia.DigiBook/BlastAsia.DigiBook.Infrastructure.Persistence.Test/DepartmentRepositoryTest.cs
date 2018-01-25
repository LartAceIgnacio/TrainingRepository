using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Infrastructure.Persistence.Test
{
    [TestClass]
    public class DepartmentRepositoryTest
    {
        [TestMethod]
        public void Create_WithExistingDepartment_RemovesRecordFromDatabase()
        {
            // Arrange

            var connectionString 
                = @"Data Source=.;Database=DigiBookDb;Integrated Security=true;";

            var dbOptions = new DbContextOptions<DigiBookDbContext>();

            dbOptions = new DbContextOptionsBuilder<DigiBookDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            var DbContext = new DigiBookDbContext(dbOptions);
            DbContext.Database.EnsureCreated();

            // Act

            // Assert

            // Cleanup
        }
    }
}
