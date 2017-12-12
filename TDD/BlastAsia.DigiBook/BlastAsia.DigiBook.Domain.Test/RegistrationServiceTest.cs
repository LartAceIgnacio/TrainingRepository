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
            username = "deguzmanduane@gmail.com";
            password = "Bl@st123";
            mockRepository = new Mock<IAccountRepository>();
            sut = new RegistrationService(mockRepository.Object);
        }  

        [TestCleanup]
        public void CleanUpTest()
        {

        }

        [TestMethod]
        public void Register_ValidUserNameAndPassword_RegistersAccount()
        {
            // Arrange          
            var expectedResult = true;
            // Act
            bool actualResult = sut.Register(username, password);
            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void Register_BlankUserName_ThrowsUserNameRequiredException()
        {
            // Arrange
            var username = "";
            // Assert
            Assert.ThrowsException<UserNameRequiredException>(
                 () => sut.Register(username, password));
        }

        [DataTestMethod]
        [DataRow("deguzmanduanegmailcom")]
        [DataRow("deguzman.duanegmail.com")]
        [DataRow("deguzmanduane@gmailcom")]
        public void Register_UserNameNotValidEmail_ThrowsValidEmailRequiredException(string username)
        {
            // Assert
            Assert.ThrowsException<ValidEmailRequiredException>(
                () => sut.Register(username, password));
        }

        [TestMethod]
        public void Register_BlankPassword_ThrowsPasswordRequiredException()
        {
            // Arrange
            var password = "";
            // Assert
            Assert.ThrowsException<PasswordRequiredException>(
                 () => sut.Register(username, password));
        }

        [TestMethod]
        public void Register_PasswordLessThanMinimum_ThrowsPasswordMinimumLengthRequiredException()
        {
            // Arrange
            var password = "Bl@st12";
            // Assert
            Assert.ThrowsException<PasswordMinimumLengthRequiredException>(
                () => sut.Register(username, password));
        }

        [DataTestMethod]
        [DataRow("BL@ST123")]
        [DataRow("bl@st123")]
        [DataRow("Bl@stabc")]
        [DataRow("Blast123")]
        public void Register_PasswordNotStrong_ThrowsStrongPasswordRequiredException(string password)
        {

            // Assert
            Assert.ThrowsException<StrongPasswordRequiredException>(
                () => sut.Register(username, password));
        }

        [TestMethod]
        public void Register_WithValidUserNameandPassword_ShouldCallRepositoryCreate()
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
