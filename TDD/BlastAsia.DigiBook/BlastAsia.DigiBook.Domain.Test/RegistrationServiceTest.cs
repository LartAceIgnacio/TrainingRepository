using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BlastAsia.DigiBook.Domain.Models;

namespace BlastAsia.DigiBook.Domain.Test
{
    [TestClass]
    public class RegistrationServiceTest
    {
        private string username;
        private string password;
        private RegistrationService sut;
        private Mock<IAccountRepository> mockRepository;

        [TestInitialize]
        public void InitializeTest()
        {
            username = "oribelloryan@gmail.com";
            password = "Bl@st123";

            mockRepository = new Mock<IAccountRepository>();
            sut = new RegistrationService(mockRepository.Object);
        }

        [TestCleanup]
        public void CleanupTest()
        {

        }

        [TestMethod]
        public void Register_ValidUsernameAndPassword_RegistersAccount()
        {
            // ARRANGE
            var expectedResult = true;
            // ACT

            bool actualResult = sut.Register(username, password);
            // ASSERT
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void Register_BlankUsername_ThrowsUsernameRequiredException()
        {
            // Act
            username = "";
            // Assert
            Assert.ThrowsException<UsernameRequiredException>(
                () => sut.Register(username, password));
        }

        [DataTestMethod]
        [DataRow("oribelloryanyahoo.com")]
        [DataRow("oribelloryan@yahoo")]
        [DataRow("oribelloryanyahoo.com")]
        [DataRow("oribello_ryanyahoo.com")]
        public void Register_EmailInvalidFormat_ThrowsEmailInvalidFormatException(string username)
        {
            Assert.ThrowsException<EmailInvalidFormatException>(
                () => sut.Register(username, password));
        }

        [TestMethod]
        public void Register_BlankPassword_ThrowsPasswordRequiredException()
        {
            password = "";
            Assert.ThrowsException<PasswordRequiredException>(
                ()=> sut.Register(username, password));
        }

        [TestMethod]
        public void Register_PasswordLessthanMinimum_ThrowsMinimumLengthException()
        {
            // Arrange
            password = "Bl@st12";
            //Assert
            Assert.ThrowsException<MinimumPasswordLengthException>(
                () => sut.Register(username, password));
        }

        [DataTestMethod]
        [DataRow("BL@ST123")]
        [DataRow("Blast123")]
        [DataRow("Bl@stabc")]
        [DataRow("bl@st123")]
        //[DataRow("Bl@st123")]
        public void Register_StrongPasswordRequired_ThrowsStrongPasswordException(string password)
        {
            Assert.ThrowsException<StrongPasswordException>(
                () => sut.Register(username, password));
        }

        [TestMethod]
        public void Register_WithValidUsernamePassword_ShouldCallRepository()
        {
            // Arrange

            // Act
            sut.Register(username, password);
            // Assert
            mockRepository
                .Verify(r => r.Create(It.IsAny<Account>()),
                Times.Once());

        }
    }
}
