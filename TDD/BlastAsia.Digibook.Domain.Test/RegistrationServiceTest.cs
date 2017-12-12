using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BlastAsia.Digibook.Domain.Models;

namespace BlastAsia.Digibook.Domain.Test
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
            username = "gcano@blastasia.com";
            password = "Bl@st123";
            mockRepository = new Mock<IAccountRepository>();
            sut = new RegistrationService(mockRepository.Object);
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
            password = "";

            Assert.ThrowsException<PasswordRequiredException>(
                    () => sut.Register(username, password)
                );
        }

        [TestMethod]
        public void Register_PasswordLessThanMinimumLength_ThrowsMinimumLengthRequiredException()
        {
            password = "Bl@st12";

            Assert.ThrowsException<MinimumLengthRequiredException>(
                () => sut.Register(username, password));
        }

        [TestMethod]
        public void Register_BlankUsername_ThrowsUsernameRequiredException()
        {
            username = "";

            Assert.ThrowsException<UsernameRequiredException>(
                () => sut.Register(username, password)
                );
        }

        [DataTestMethod]
        [DataRow("gcanoblastasiacom")]
        [DataRow("gcano@blastasia")]
        [DataRow("gcano.blastasia.com")]
        public void Register_InvalidEmailFormat_ThrowsEmailInvalidException(string username)
        {
            Assert.ThrowsException<EmailInvalidException>(
                () => sut.Register(username,password));
        }

        [DataTestMethod]
        [DataRow("bl@st123")]
        [DataRow("BL@ST123")]
        [DataRow("Bl@stasia")]
        [DataRow("Blast123")]
        public void Register_PasswordMustBeStrong_ThrowsPasswordNotStrongException(string password)
        {
            Assert.ThrowsException<PasswordNotStrongException>(
                () => sut.Register(username, password));
        }

        [TestMethod]
        public void Register_WithValidUserNameAndPassword_ShouldCallRepositoryCreate()
        {
            sut.Register(username, password);

            mockRepository
                .Verify(r => r.Create(It.IsAny<Account>())
                    , Times.Once);
        }
    }
}
