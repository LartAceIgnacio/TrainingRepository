using BlastAsia.DigiBook.Domain.Models.Names;
using BlastAsia.DigiBook.Domain.Names;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Test.Names
{
    [TestClass]
    public class NameServiceTest
    {
        private Mock<INameRepository> mockNameRepository;
        private NameService sut;
        private Guid existingId;
        private Guid nonExistingId;
        private Name name;

        [TestInitialize]
        public void TestInitialize()
        {
            mockNameRepository = new Mock<INameRepository>();
            sut = new NameService(mockNameRepository.Object);
            existingId = Guid.NewGuid();
            nonExistingId = Guid.Empty;
            name = new Name
            {
                NameId = Guid.NewGuid(),
                NameFirst = "Karl",
                NameLast = "Matencio"
            };

            mockNameRepository
                .Setup(n => n.Retrieve(existingId))
                .Returns(name);

            mockNameRepository
                .Setup(n => n.Retrieve(nonExistingId))
                .Returns<Name>(null);
        }

        [TestCleanup]
        public void TestCleanup()
        {

        }

        [TestMethod]
        public void Save_WithValidData_ShoulCallRepositoryCreate()
        {
            //Arrange
            
            //Act
            var result = sut.Save(name.NameId, name);
            //Assert
            mockNameRepository
                .Verify(n => n.Create(name), Times.Once);
        }

        [TestMethod]
        public void Save_WithNullNameFirst_ThrowsNullFirstNameException()
        {
            //Arrange
            name.NameFirst = "";
            //Act

            //Assert
            Assert.ThrowsException<FirstNameRequiredException>(
                () => sut.Save(name.NameId, name));
        }
        
        [TestMethod]
        public void Save_WithNullNameLast_ThrowsLastNameRequiredException()
        {
            //Arrange
            name.NameLast = "";
            //ACt

            //Assert
            Assert.ThrowsException<LastNameRequiredException>(
                () => sut.Save(name.NameId, name));
        }

        [TestMethod]
        public void Save_WithExistingNameId_ShouldCallRepositoryUpdate()
        {
            //Arrange
            name.NameId = existingId;
            //Act
            var result = sut.Save(name.NameId, name);
            //Assert
            mockNameRepository
                .Verify(n => n.Retrieve(existingId), Times.Once);
            mockNameRepository
                .Verify(n => n.Update(name.NameId, name), Times.Once);
        }

        [TestMethod]
        public void Save_WithNonExistingNameId_ShouldCallRepositoryCreate()
        {
            //Arrange
            name.NameId = nonExistingId;
            //Act
            var result = sut.Save(name.NameId, name);
            //Assert
            mockNameRepository
                .Verify(n => n.Retrieve(nonExistingId), Times.Once);
            mockNameRepository
                .Verify(n => n.Create(name), Times.Once);
        }
    }
}
