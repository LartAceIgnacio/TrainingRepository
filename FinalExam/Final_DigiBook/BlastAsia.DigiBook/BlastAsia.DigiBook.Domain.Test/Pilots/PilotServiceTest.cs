using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Test.Pilots
{
    [TestClass]
    public class PilotServiceTest
    {

        [TestMethod]
        public void Save_WithValidData_ShoulPilotRepositoryCreate()
        {
            //Arrange
            var pilot = new Pilot
            {
                PilotId = Guid.NewGuid(),
                FirstName = "Christoper",
                MiddleName = "Magdaleno",
                LastName = "Manuel",
                BirthDate = DateTime.Parse("Feb-01-1995"),
                YearsOfExperience = 10,
                DateActivated = DateTime.Today,
                PilotCode = "FFMMLLLLYYmmdd",
                DateCreated = DateTime.Now,
                DateModified = new Nullable<DateTime>()
            }

            var mockPilotRepository = new Mock<IPilotRepository>;
            var sut = new PilotService(mockPilotRepository.Object);
            //Act 
            sut.Save(pilot.PilotId, pilot);

            //Assert
            mockPilotRepository
                .Verify(c => c.Create(pilot), Times.Once());
        }
    }

}
