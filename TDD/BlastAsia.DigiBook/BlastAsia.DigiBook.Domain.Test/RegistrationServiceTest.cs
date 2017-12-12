using System;
using System.Collections.Generic;
using System.Text;
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
        RegistrationService sut;
        private Mock<IAccountRepository> mockRepository;

        [TestInitialize] // Initialize values
        public void InitializeTest()
        {
            username = "luigiabille@gmail.com";
            password = "Bl@st123";
            mockRepository = new Mock<IAccountRepository>();
            sut = new RegistrationService(mockRepository.Object); //System under test
            
        }
        [TestCleanup] // Cleanup TestInitialize
        public void cleanUp()
        {

        }

        [TestMethod] //Valid Username & Password
        public void Register_ValidUserNameAndPassword_RegistersAccount()
        {
            // Arrange

            
            var expecterResult = true;

            // Act

            bool actualResult = sut.Register(username, password);

            // Assert

            Assert.AreEqual(expecterResult, actualResult);
        }
        [TestMethod] // Blank Username
        public void Register_BlankUsername_ThrowsUsernameRequiredException()
        {

            //Arrange

            var username = "";

            // Act

            // Assert

            Assert.ThrowsException<UsernameRequiredException>(
                () => sut.Register(username, password));
        }


        [DataTestMethod] // Valid Email
        [DataRow("luigiabillegmailcom")]
        [DataRow("luigiabille@gmailcom")]
        [DataRow("luigiabillegmail.com")]
        public void Register_ValidEmail_ThrowsEmailRequiredException(string username)
        {
            // Arrange

            // Act

            // Assert
            Assert.ThrowsException<EmailRequiredException>(
                () => sut.Register(username, password));
        }

        [TestMethod] // Blank Password
        public void Register_BlankPassword_ThrowsPasswordRequiredException()
        {
            // Arrange

            password = "";

            // Act

            // Assert

            Assert.ThrowsException<PasswordRequiredException>(
                () => sut.Register(username, password));
        }

        [TestMethod] // Less than 8 characters
        public void Register_PasswordLessThanMinimum_ThrowsMinimumLengthException()
        {
            // Arrange
            password = "Bl@st12";

            // Act

            // Assert
            Assert.ThrowsException<PasswordMinimumLenghtException>(
                () => sut.Register(username, password));

        }
        
        [DataTestMethod] // Password needs Upper Case , Lower Case , Special Character, Number
        [DataRow("Bl@stabxw")]
        [DataRow("Blast123")]
        [DataRow("BLAST123")]
        [DataRow("bl@st123")]
        public void Register_PasswordNeedsUpperCaseLowerCaseSpecialCharacterAndNumberException_ThrowsStrongPasswordException(string password)
        {
            Assert.ThrowsException<StrongPasswordException>(
                () => sut.Register(username, password));
        }
        [TestMethod]
        public void Register_WithValidUsernameAndPassword_ShouldCallRepository()
        {
            // Arrange

            // Act
            sut.Register(username, password);

            // Assert
            mockRepository
                .Verify(r => r.Create(It.IsAny<Account>()),
                Times.Once);

        }

    }
}
