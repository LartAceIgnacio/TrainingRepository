using BlastAsia.DigiBook.Domain.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

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
            username = "jmagdaleno@blastasia.com";
            password = "Bl@st123";

            mockRepository = new Mock<IAccountRepository>();
            sut = new RegistrationService(mockRepository.Object);
        }

        [TestCleanup]
        public void CleanUp()
        {

        }

        [TestMethod]
        public void Register_ValidUserNameAndPassword_RegistersAccount()
        {
            //Arrange 
            var expectedResult = true;

            //Act
            bool actualResult = sut.Register(username, password);

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void Register_BlankPassword_ThrowsPasswordRequiredException()
        {
            //Arrange
            password = "";
            
            //Assert
            Assert.ThrowsException<PasswordRequiredException>(
                () => sut.Register(username,password));
        }

        [TestMethod]
        public void Register_BlankUsername_ThrowsUsernameRequiredException()
        {
            //Arrange
            username = "";

            //Assert
            Assert.ThrowsException<UsernameRequiredException>(
                () => sut.Register(username, password));
        }

        [TestMethod]
        public void Register_PasswordLessThanMinimumLength_ThrowsPasswordMinimumLengthRequiredException()
        {
            //Arrange
            password = "Bl@st12";

            //Assert
            Assert.ThrowsException<PasswordMinimumLengthRequiredException>(
                () => sut.Register(username,password));
        }

        [DataTestMethod]
        [DataRow("bl@st123")]
        [DataRow("BL@ST123")]
        [DataRow("Blast123")]
        [DataRow("Bl@stabc")]
        public void Register_PasswordNotStrong_ThrowsPasswordStrongTypedRequiredException(string password)
        {
            //Assert
            Assert.ThrowsException<PasswordStrongTypedRequired>(
                () => sut.Register(username,password));
        }

        [DataTestMethod]
        [DataRow("jmagdalenoblastasiacom")]
        [DataRow("jmagdaleno@blastasiacom")]
        [DataRow("jmagdaleno.blastasia.com")]
        public void Register_UserNameNotValid_ThrowsValidUserNameRequiredException(string username)
        {
            //Assert
            Assert.ThrowsException<ValidUserNameRequired>(
                () => sut.Register(username,password));
        }

        [TestMethod]
        public void Register_WithValidUserNameAndPassword_ShouldCallRepositoryCreate()
        {
            //Act
            sut.Register(username, password);

            //Assert
            mockRepository
                .Verify(r => r.Create(It.IsAny<Account>()),
                        Times.Once());

        }
    }
}
