using BlastAsia.Digibook.Domain.Models.Venues;
using BlastAsia.Digibook.Infrastracture.Persistence;
using BlastAsia.Digibook.Infrastracture.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.Digibook.Infrastructure.Persistence.Test
{
    [TestClass]
    public class VenueRepositoryTest
    {
        [TestMethod]
        public void Create_WithValidData_ShouldSaveInTheDatabase()
        {
            string connectionString = @"Data Source=.;Database=DigiBookDb;Integrated Security=true;";
            DbContextOptions<DigiBookDbContext> dbContextOptions = new DbContextOptionsBuilder<DigiBookDbContext>()
                                                    .UseSqlServer(connectionString)
                                                    .Options;
            DigiBookDbContext digibookDbContext = new DigiBookDbContext(dbContextOptions);
            Venue venue = new Venue { VenueName = "My Venue", Description = "My Description"};
            VenueRepository sut = new VenueRepository(digibookDbContext);

            digibookDbContext.Database.EnsureDeleted();
            digibookDbContext.Database.EnsureCreated();

        }
    }
}
