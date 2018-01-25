using BlastAsia.DigiBook.Domain.Luigis;
using BlastAsia.DigiBook.Domain.Models.Luigis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Test.Luigis
{
    [TestClass]
    public class LuigiServiceTest
    {
        [TestMethod]
        public void Create_NewLuigiWithValidData_ShouldCallRepositoryCreate()
        {
            // Arrange

            var mockLuigiRepository = new Mock<ILuigiRepository>();
            var sut = new LuigiService(mockLuigiRepository.Object);

            var luigi = new Luigi
            {
                FirstName = "Luigi",
                LastName = "Abille",
            };

            var nonExistingLuigiId = Guid.Empty;

            mockLuigiRepository
                .Setup(c => c.Retrieve(nonExistingLuigiId))
                .Returns<Luigi>(null);

            // Act

            var result = sut.Save(luigi.LuigiId, luigi);

            // Assert

            mockLuigiRepository
                .Verify(c => c.Retrieve(nonExistingLuigiId), Times.Once);

            mockLuigiRepository
                .Verify(c => c.Create(luigi), Times.Once);
        }
        [TestMethod]
        public void Update_WithExistingLuigi_ShouldCallRepositoryUpdate()
        {
            // Arrange

            var mockLuigiRepository = new Mock<ILuigiRepository>();

            var sut = new LuigiService(mockLuigiRepository.Object);

            var luigi = new Luigi
            {
                FirstName = "Luigi",
                LastName = "Abille"
            };

            var existingLuigiId = Guid.NewGuid();

            luigi.LuigiId = existingLuigiId;

            mockLuigiRepository
                .Setup(c => c.Retrieve(existingLuigiId))
                .Returns(luigi);

            // Act

            var result = sut.Save(luigi.LuigiId, luigi);

            // Assert

            mockLuigiRepository
                .Verify(c => c.Retrieve(existingLuigiId), Times.Once);
            mockLuigiRepository
                .Verify(c => c.Update(existingLuigiId, luigi), Times.Once);

        }

        [TestMethod]
        public void Save_WithValidData_ReturnsNewLuigiWithLuigiId()
        {
            // Arrange

            var mockLuigiRepository = new Mock<ILuigiRepository>();
            var sut = new LuigiService(mockLuigiRepository.Object);

            var luigi = new Luigi
            {
                FirstName = "Luigi",
                LastName = "Abille"
            };

            mockLuigiRepository
                .Setup(c => c.Create(luigi))
                .Callback(() => luigi.LuigiId = Guid.NewGuid())
                .Returns(luigi);

            // Act

            var newLuigi = sut.Save(luigi.LuigiId, luigi);

            // Assert

            Assert.IsTrue(newLuigi.LuigiId != Guid.Empty);

        }

        [TestMethod]
        public void Save_WithBlankFirstName_ThrowsNameRequiredException()
        {
            // Arrange

            var luigi = new Luigi
            {
                FirstName = ""
            };

            var mockLuigiRepository = new Mock<ILuigiRepository>();

            var sut = new LuigiService(mockLuigiRepository.Object);

            // Act

            // Assert

            mockLuigiRepository
                .Verify(c => c.Create(luigi), Times.Never());

            Assert.ThrowsException<FirstNameRequired>
                (() => sut.Save(luigi.LuigiId, luigi));
        }
    }
}
