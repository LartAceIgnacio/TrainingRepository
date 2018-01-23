using BlastAsia.DigiBook.API.Controllers;
using BlastAsia.DigiBook.Domain.Models.Pilots;
using BlastAsia.DigiBook.Domain.Pilots;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.API.Test
{
    [TestClass]
    public class PilotsControllerTest
    {
        private Pilot pilot;
        private Mock<IPilotRepository> mockpilotRepository;
        private Mock<IPilotService> mockpilotService;
        private JsonPatchDocument patchedpilot;

        private PilotsController sut;

        private Guid existingpilotId = Guid.NewGuid();
        private Guid nonExistingpilotId = Guid.Empty;

        [TestInitialize]
        public void Initialize()
        {
            mockpilotRepository = new Mock<IPilotRepository>();
            mockpilotService = new Mock<IPilotService>();
            patchedpilot = new JsonPatchDocument();

            pilot = new Pilot
            {
                PilotId = Guid.NewGuid()
            };

            sut = new PilotsController(mockpilotService.Object,
                mockpilotRepository.Object);

            mockpilotRepository
                .Setup(d => d.Retrieve())
                .Returns(() => new List<Pilot>{
                    new Pilot() });

            mockpilotRepository
                .Setup(d => d.Retrieve(pilot.PilotId))
                .Returns(pilot);
            mockpilotRepository
                .Setup(d => d.Retrieve(existingpilotId))
                .Returns(pilot);
        }

     
    }
}
