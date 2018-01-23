using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Test.Pilots
{

    [TestClass]
    public class PilotServiceTest
    {

        [TestInitialize]
        public void Initialize()
        {

        }

        [TestCleanup]
        public void Cleanup()
        {

        }

        [TestMethod]
        public void Save_WithValidData_ShouldCallRepositorySave()
        {
            // arrange
            var Pilot = new Pilot()
            {
                PilotId = Guid.NewGuid(),
                FirstName = "Emmanuel",
                MiddleName = "Pararuan",
                LastName = "Magadia",
                DateOfBirth = DateTime.Now,
                YearsOfExperience = DateTime.Now,
                DateActivated = DateTime.Now,
                PilotCode = "",
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now
            };

            var mockRepo = new Mock<IPilotService>();

            var sut = new PilotService(mockRepo.Object);

            // act 

            // assert
        }

    }
}
