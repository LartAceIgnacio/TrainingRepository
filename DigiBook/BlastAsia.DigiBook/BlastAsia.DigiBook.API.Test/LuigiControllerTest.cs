using BlastAsia.DigiBook.API.Controllers;
using BlastAsia.DigiBook.Domain.Luigis;
using BlastAsia.DigiBook.Domain.Models.Luigis;
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
    public class LuigiControllerTest
    {
        [TestMethod]
        public void GetLuigis_WithEmptyLuigiId_ReturnsOkObjectResult()
        {
            // Arrange

            var mockLuigiRepository = new Mock<ILuigiRepository>();
            var mockLuigiService = new Mock<ILuigiService>();
            var sut = new LuigiController(mockLuigiRepository.Object
                , mockLuigiService.Object);
            var luigi = new Luigi();
            var patchedLuigi = new JsonPatchDocument();


            // Act

            var result = sut.GetLuigis(null);

            // Assert

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            mockLuigiRepository
                .Verify(c => c.Retrieve(), Times.Once);
        }
        [TestMethod]
        public void CreateLuigi_WithValidLuigiData_ReturnsCreatedAtActionResult()
        {
            // Arrange

            var mockLuigiRepository = new Mock<ILuigiRepository>();
            var mockLuigiService = new Mock<ILuigiService>();
            var sut = new LuigiController(mockLuigiRepository.Object
                , mockLuigiService.Object);
            var luigi = new Luigi();
            var patchedLuigi = new JsonPatchDocument();

            // Act

            var result = sut.CreateLuigi(luigi);

            // Assert

            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));

            mockLuigiService
                .Verify(c => c.Save(Guid.Empty, luigi), Times.Once);

        }
        [TestMethod]
        public void CreateLuigi_WithNullLuigiData_ReturnsBadRequestResult()
        {
            // Arrange

            var mockLuigiRepository = new Mock<ILuigiRepository>();
            var mockLuigiService = new Mock<ILuigiService>();
            var sut = new LuigiController(mockLuigiRepository.Object
                , mockLuigiService.Object);
            var luigi = new Luigi();
            var patchedLuigi = new JsonPatchDocument();

            luigi = null;

            // Act

            var result = sut.CreateLuigi(luigi);

            // Assert

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));

            mockLuigiService
                .Verify(c => c.Save(Guid.Empty, luigi), Times.Never);
        }
        [TestMethod]
        public void DeleteLuigi_WithExistingLuigiId_ReturnOkResult()
        {
            // Arrange

            var mockLuigiRepository = new Mock<ILuigiRepository>();
            var mockLuigiService = new Mock<ILuigiService>();
            var sut = new LuigiController(mockLuigiRepository.Object
                , mockLuigiService.Object);
            var luigi = new Luigi();
            var patchedLuigi = new JsonPatchDocument();

            var existingLuigiId = Guid.NewGuid();
            luigi.LuigiId = existingLuigiId;

            mockLuigiRepository
                .Setup(c => c.Retrieve(existingLuigiId))
                .Returns(luigi);

            // Act

            var result = sut.DeleteLuigi(luigi.LuigiId);

            // Assert

            Assert.IsInstanceOfType(result, typeof(OkResult));

            mockLuigiRepository
                .Verify(c => c.Delete(luigi.LuigiId), Times.Once);
        }

        [TestMethod]
        public void DeleteLuigi_WithNonExistingLuigiId_ReturnsOkObjectResult()
        {
            // Arrange

            var mockLuigiRepository = new Mock<ILuigiRepository>();
            var mockLuigiService = new Mock<ILuigiService>();
            var sut = new LuigiController(mockLuigiRepository.Object
                , mockLuigiService.Object);
            var luigi = new Luigi();
            var patchedLuigi = new JsonPatchDocument();

            var nonExistingLuigiId = Guid.Empty;
            luigi.LuigiId = nonExistingLuigiId;

            mockLuigiRepository
                .Setup(c => c.Retrieve(nonExistingLuigiId))
                .Returns<Luigi>(null);

            // Act

            var result = sut.DeleteLuigi(nonExistingLuigiId);

            // Assert

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));

            mockLuigiRepository
                .Verify(c => c.Delete(luigi.LuigiId), Times.Never);
        }

        [TestMethod]
        public void UpdateLuigi_WithExitingLuigiId_ReturnsOkObjectResult()
        {
            // Arrange

            var mockLuigiRepository = new Mock<ILuigiRepository>();
            var mockLuigiService = new Mock<ILuigiService>();
            var sut = new LuigiController(mockLuigiRepository.Object
                , mockLuigiService.Object);
            var luigi = new Luigi();
            var patchedLuigi = new JsonPatchDocument();

            var existingLuigiId = Guid.NewGuid();

            luigi.LuigiId = existingLuigiId;

            mockLuigiRepository
               .Setup(c => c.Retrieve(existingLuigiId))
               .Returns(luigi);

            // Act

            var result = sut.UpdateLuigi(luigi, luigi.LuigiId);

            // Assert

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            mockLuigiService
                .Verify(c => c.Save(luigi.LuigiId, luigi), Times.Once);
            mockLuigiRepository
                .Verify(c => c.Retrieve(luigi.LuigiId), Times.Once);
        }

        [TestMethod]
        public void UpdateLuigi_WithNonExistingLuigiId_ReturnsBadRequest()
        {
            // Arrange

            var mockLuigiRepository = new Mock<ILuigiRepository>();
            var mockLuigiService = new Mock<ILuigiService>();
            var sut = new LuigiController(mockLuigiRepository.Object
                , mockLuigiService.Object);
            var luigi = new Luigi();
            var patchedLuigi = new JsonPatchDocument();

            var nonExistingLuigiId = Guid.Empty;

            luigi.LuigiId = nonExistingLuigiId;

            mockLuigiRepository
                .Setup(c => c.Retrieve(nonExistingLuigiId))
                .Returns<Luigi>(null);

            // Act

            var result = sut.UpdateLuigi(luigi, luigi.LuigiId);

            // Assert

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));

            mockLuigiRepository
                .Verify(c => c.Retrieve(luigi.LuigiId), Times.Once);
            mockLuigiService
                .Verify(c => c.Save(luigi.LuigiId, luigi), Times.Never);
        }
        [TestMethod]
        public void PatchLuigi_WithValidPatchLuigiData_ReturnsOkObjectResult()
        {
            // Arrange

            var mockLuigiRepository = new Mock<ILuigiRepository>();
            var mockLuigiService = new Mock<ILuigiService>();
            var sut = new LuigiController(mockLuigiRepository.Object
                , mockLuigiService.Object);
            var luigi = new Luigi();
            var patchedLuigi = new JsonPatchDocument();
            var existingLuigiId = Guid.NewGuid();
            luigi.LuigiId = existingLuigiId;

            mockLuigiRepository
                .Setup(c => c.Retrieve(existingLuigiId))
                .Returns(luigi);

            // Act

            var result = sut.PatchLuigi(patchedLuigi, luigi.LuigiId);

            // Assert

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            mockLuigiService
                .Verify(c => c.Save(luigi.LuigiId, luigi), Times.Once);
        }

        [TestMethod]
        public void PatchLuigi_WithNonValidPatchLuigiData_ReturnsBadResult()
        {
            // Arrange

            var mockLuigiRepository = new Mock<ILuigiRepository>();
            var mockLuigiService = new Mock<ILuigiService>();
            var sut = new LuigiController(mockLuigiRepository.Object
                , mockLuigiService.Object);
            var luigi = new Luigi();
            var patchedLuigi = new JsonPatchDocument();
            patchedLuigi = null;

            // Act

            var result = sut.PatchLuigi(patchedLuigi, luigi.LuigiId);

            // Assert

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));

            mockLuigiService
                .Verify(c => c.Save(luigi.LuigiId, luigi), Times.Never);
        }

        [TestMethod]
        public void PatchLuigi_WithNonExistingLuigiId_ReturnsNotFound()
        {
            // Arrange

            var mockLuigiRepository = new Mock<ILuigiRepository>();
            var mockLuigiService = new Mock<ILuigiService>();
            var sut = new LuigiController(mockLuigiRepository.Object
                , mockLuigiService.Object);
            var luigi = new Luigi();
            var patchedLuigi = new JsonPatchDocument();
            var nonExistingLuigiId = Guid.Empty;

            luigi.LuigiId = nonExistingLuigiId;

            mockLuigiRepository
                .Setup(c => c.Retrieve(nonExistingLuigiId))
                .Returns<Luigi>(null);

            // Act

            var result = sut.PatchLuigi(patchedLuigi, luigi.LuigiId);

            // Assert

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));

            mockLuigiService
                .Verify(c => c.Save(luigi.LuigiId, luigi), Times.Never);
        }
    }
}
