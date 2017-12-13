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
        public void Initializetest()
        {
            username = "anjacelis21@gmail.com";
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
            //Arrange
            var expectedResult = true;
            
            //Act
            bool actualResult = sut.Register(username,password);

            //Assert
            Assert.AreEqual(expectedResult,actualResult);
        }

        [TestMethod]
        public void Register_BlankUsername_ThrowsUsernameRequiredException()
        {
            //Arrange
            var username = "";

            //assert
            Assert.ThrowsException<UsernameRequiredException>(
                 () => sut.Register(username, password)
                );

        }

        [DataTestMethod]
        [DataRow("anjacelis21gmailcom")]
        [DataRow("angelou21@gmail")]
        [DataRow("angelou21.gmail.com")]

        public void Register_ValidEmail_ThrowsValidEmailRequiredException(string username)
        {
            //Arrange


            //assert
            Assert.ThrowsException<ThrowsValidEmailRequiredException>(
                 () => sut.Register(username, password)
                );

        }

        [TestMethod]
        public void Register_BlankPassword_ThrowsPasswordRequiredException()
        {
            //Arrange
            var password = "";



            //Assert
            Assert.ThrowsException<PasswordRequiredException>(
                 ()=> sut.Register(username,password)
                );
        }
        [TestMethod]
        public void Register_PasswordLessThanMininum_ThrowsPasswordMinimumLengthException()
        {
            //Arrange
            var password = "Bl@st12";



            //Assert
            Assert.ThrowsException<PasswordMinimunLenghtRequiredException>(
                 () => sut.Register(username, password)
                );
        }
        [DataTestMethod]
        [DataRow("bl@st123")]
        [DataRow("bl@st123")]
        [DataRow("BL@ST123")]
        [DataRow("Blast123")]
        [DataRow("Bl@stzxc")]
        public void Register_PasswordMustHaveStrongPassword_ThrowStrongPasswordException(string password)
        {
            //Arrange


            //Assert
            Assert.ThrowsException<PasswordStrongRequiredException>(
                 () => sut.Register(username, password)
                );
        }

        [TestMethod]
        public void Register_WithValidUserNameAndPassword_ShouldCallRepository()
        {

            //Arrange


            //Act
            sut.Register(username, password);

            //Assert
            mockRepository
                .Verify(r => r.Create(It.IsAny<Account>()),
                    Times.Once());

        }
    }
}
