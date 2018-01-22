using BlastAsia.DigiBook.Api.Controllers;
using BlastAsia.DigiBook.Domain.Models.Names;
using BlastAsia.DigiBook.Domain.Names;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Api.Test
{
    [TestClass]
    public class NameControllerTest
    {
        [TestMethod]
        public void GetName_WithExistingNameId_ReturnsOkObjectResult()
        {
            //Arrange
            Mock<INameRepository> mockNameRepository = new Mock<INameRepository>();
            Mock<INameService> mockNameService = new Mock<INameService>();
            NameController sut = new NameController(mockNameRepository.Object, mockNameService.Object);
            JsonPatchDocument patch = new JsonPatchDocument();
            Guid existingId = Guid.NewGuid();
            Name name = new Name
            {
                NameId = existingId,
                NameFirst = "Karl",
                NameLast = "Matencio"
            };
            //Act
            var result = sut.GetNames(name.NameId);
            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockNameRepository
                .Verify(n => n.Retrieve(name.NameId), Times.Once);

        }
        [TestMethod]
        public void GetName_WithNullNameId_ReturnsOkObjectResult()
        {
            //Arrange
            Mock<INameRepository> mockNameRepository = new Mock<INameRepository>();
            Mock<INameService> mockNameService = new Mock<INameService>();
            NameController sut = new NameController(mockNameRepository.Object, mockNameService.Object);
            JsonPatchDocument patch = new JsonPatchDocument();
            Guid nonExistingId = Guid.Empty;
            Name name = new Name
            {
                NameId = nonExistingId,
                NameFirst = "Karl",
                NameLast = "Matencio"
            };

            mockNameRepository
                .Setup(n => n.Retreive())
                .Returns(new List<Name>());
            //Act
            var result = sut.GetNames(null);
            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockNameRepository
                .Verify(n => n.Retreive(), Times.Once);
        }

        [TestMethod]
        public void CreateNames_WithNonExistingNameId_ReturnsCreatedAtAction()
        {
            //Arrange
            Mock<INameRepository> mockNameRepository = new Mock<INameRepository>();
            Mock<INameService> mockNameService = new Mock<INameService>();
            NameController sut = new NameController(mockNameRepository.Object, mockNameService.Object);
            JsonPatchDocument patch = new JsonPatchDocument();
            Guid nonExisting = Guid.Empty;
            Name name = new Name
            {
                NameId = nonExisting,
                NameFirst = "Karl",
                NameLast = "Matencio"
            };
            //Act
            var result = sut.CreateNames(name);
            //Assert
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
            mockNameService
                .Verify(n => n.Save(name.NameId, name), Times.Once);
        }

        [TestMethod]
        public void CreateNames_WithNullName_ReturnsBadRequest()
        {
            //Arrange
            Mock<INameRepository> mockNameRepository = new Mock<INameRepository>();
            Mock<INameService> mockNameService = new Mock<INameService>();
            NameController sut = new NameController(mockNameRepository.Object, mockNameService.Object);
            JsonPatchDocument patch = new JsonPatchDocument();
            Guid nonExisting = Guid.Empty;
            Name name = new Name
            {
                NameId = nonExisting,
                NameFirst = "Karl",
                NameLast = "Matencio"
            };

            name = null;
            //Act
            var result = sut.CreateNames(name);
            //Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            mockNameService
                .Verify(n => n.Save(nonExisting, name), Times.Never);
        }

        [TestMethod]
        public void DeleteNames_WithExistingNameId_ReturnsNoContent()
        {
            //Arrange
            Mock<INameRepository> mockNameRepository = new Mock<INameRepository>();
            Mock<INameService> mockNameService = new Mock<INameService>();
            NameController sut = new NameController(mockNameRepository.Object, mockNameService.Object);
            JsonPatchDocument patch = new JsonPatchDocument();
            Guid existingId = Guid.NewGuid();
            Name name = new Name
            {
                NameId = existingId,
                NameFirst = "Karl",
                NameLast = "Matencio"
            };

            mockNameRepository
                .Setup(n => n.Retrieve(existingId))
                .Returns(name);
            //Act
            var result = sut.DeleteNames(name.NameId);
            //Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
            mockNameRepository
                .Verify(n => n.Retrieve(existingId), Times.Once);
            mockNameRepository
                .Verify(n => n.Delete(name.NameId), Times.Once);
        }

        [TestMethod]
        public void DeleteNames_WithNonExistingNameId_ReturnsBadRequest()
        {
            //Arrange
            Mock<INameRepository> mockNameRepository = new Mock<INameRepository>();
            Mock<INameService> mockNameService = new Mock<INameService>();
            NameController sut = new NameController(mockNameRepository.Object, mockNameService.Object);
            JsonPatchDocument patch = new JsonPatchDocument();
            Guid nonExistingId = Guid.Empty;
            Name name = new Name
            {
                NameId = nonExistingId,
                NameFirst = "Karl",
                NameLast = "Matencio"
            };

            mockNameRepository
                .Setup(n => n.Retrieve(nonExistingId))
                .Returns<Name>(null);
            //Act
            var result = sut.DeleteNames(name.NameId);
            //Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            mockNameRepository
                .Verify(n => n.Retrieve(nonExistingId), Times.Once);
            mockNameRepository
                .Verify(n => n.Delete(name.NameId), Times.Never);
        }

        [TestMethod]
        public void UpdateNames_WithExistingId_ReturnsOkObjectResult()
        {
            //Arrange
            Mock<INameRepository> mockNameRepository = new Mock<INameRepository>();
            Mock<INameService> mockNameService = new Mock<INameService>();
            NameController sut = new NameController(mockNameRepository.Object, mockNameService.Object);
            JsonPatchDocument patch = new JsonPatchDocument();
            Guid existingId = Guid.NewGuid();
            Name name = new Name
            {
                NameId = existingId,
                NameFirst = "Karl",
                NameLast = "Matencio"
            };

            mockNameRepository
                .Setup(n => n.Retrieve(existingId))
                .Returns(name);
            //Act
            var result = sut.UpdateNames(name.NameId, name);
            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockNameRepository
                .Verify(n => n.Retrieve(existingId), Times.Once);
            mockNameService
                .Verify(n => n.Save(name.NameId, name), Times.Once);
        }

        [TestMethod]
        public void UpdateNames_WithNonExistingId_ReturnsNoContent()
        {
            //Arrange
            Mock<INameRepository> mockNameRepository = new Mock<INameRepository>();
            Mock<INameService> mockNameService = new Mock<INameService>();
            NameController sut = new NameController(mockNameRepository.Object, mockNameService.Object);
            JsonPatchDocument patch = new JsonPatchDocument();
            Guid nonExistingId = Guid.Empty;
            Name name = new Name
            {
                NameId = nonExistingId,
                NameFirst = "Karl",
                NameLast = "Matencio"
            };

            mockNameRepository
                .Setup(n => n.Retrieve(nonExistingId))
                .Returns<Name>(null);
            //Act
            var result = sut.UpdateNames(name.NameId, name);
            //Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            mockNameRepository
                .Verify(n => n.Retrieve(nonExistingId), Times.Once);
            mockNameService
                .Verify(n => n.Save(name.NameId, name), Times.Never);
        }

        [TestMethod]
        public void UpdateNames_WithNullName_ReturnsBadRequest()
        {
            //Arrange
            Mock<INameRepository> mockNameRepository = new Mock<INameRepository>();
            Mock<INameService> mockNameService = new Mock<INameService>();
            NameController sut = new NameController(mockNameRepository.Object, mockNameService.Object);
            JsonPatchDocument patch = new JsonPatchDocument();
            Guid nonExistingId = Guid.Empty;
            Name name = new Name
            {
                NameId = nonExistingId,
                NameFirst = "Karl",
                NameLast = "Matencio"
            };
            name = null;
            //Act
            var result = sut.UpdateNames(nonExistingId, name);
            //Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            mockNameRepository
                .Verify(n => n.Retrieve(nonExistingId), Times.Never);
            mockNameService
                .Verify(n => n.Save(nonExistingId, name), Times.Never);
        }

        [TestMethod]
        public void Patch_WithExistingNameId_ReturnsOkObjectResult()
        {
            //Arrange
            Mock<INameRepository> mockNameRepository = new Mock<INameRepository>();
            Mock<INameService> mockNameService = new Mock<INameService>();
            NameController sut = new NameController(mockNameRepository.Object, mockNameService.Object);
            JsonPatchDocument patch = new JsonPatchDocument();
            Guid existingId = Guid.NewGuid();
            Name name = new Name
            {
                NameId = existingId,
                NameFirst = "Karl",
                NameLast = "Matencio"
            };
            mockNameRepository.
                Setup(l => l.Retrieve(existingId))
                .Returns(name);
            //Act
            var result = sut.PatchUpdate(name.NameId, patch);
            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockNameRepository
                .Verify(l => l.Retrieve(existingId), Times.Once);
            mockNameService
                .Verify(l => l.Save(name.NameId, name), Times.Once);
           }

        [TestMethod]
        public void PatchName_WithNonExistingId_ReturnsNotFound()
        {
            //Arrange
            Mock<INameRepository> mockNameRepository = new Mock<INameRepository>();
            Mock<INameService> mockNameService = new Mock<INameService>();
            NameController sut = new NameController(mockNameRepository.Object, mockNameService.Object);
            JsonPatchDocument patch = new JsonPatchDocument();
            Guid nonExistingId = Guid.Empty;
            Name name = new Name
            {
                NameId = nonExistingId,
                NameFirst = "Karl",
                NameLast = "Matencio"
            };
            mockNameRepository.
                Setup(l => l.Retrieve(nonExistingId))
                .Returns<Name>(null);
            //Act
            var result = sut.PatchUpdate(name.NameId, patch);
            //Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            mockNameRepository
                .Verify(n => n.Retrieve(nonExistingId), Times.Once);
            mockNameService
                .Verify(n => n.Save(name.NameId, name), Times.Never);
        }

        [TestMethod]
        public void PatchName_WithNullName_ReturnsBadRequest()
        {
            //Arrange
            Mock<INameRepository> mockNameRepository = new Mock<INameRepository>();
            Mock<INameService> mockNameService = new Mock<INameService>();
            NameController sut = new NameController(mockNameRepository.Object, mockNameService.Object);
            JsonPatchDocument patch = new JsonPatchDocument();
            Guid existingId = Guid.NewGuid();
            Name name = new Name
            {
                NameId = existingId,
                NameFirst = "Karl",
                NameLast = "Matencio"
            };

            patch = null;
            //Act
            var result = sut.PatchUpdate(name.NameId, patch);
            //Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            mockNameRepository
                .Verify(n => n.Retrieve(existingId), Times.Never);
            mockNameService
                .Verify(n => n.Save(name.NameId, name), Times.Never);
        }
    }
}
