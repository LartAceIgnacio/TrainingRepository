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
        RegistrationService sut;
        private Mock<IAccountRepository> mockRepository;

        [TestInitialize]
        public void Initialize()
        {
            username = "abolarte@outlook.com";
            password = "Bl@st123";

            mockRepository = new Mock<IAccountRepository>();
            sut = new RegistrationService(mockRepository.Object);           
        }

        [TestCleanup]
        public void Cleanup()
        {

        }

        [TestMethod]
        public void Register_ValidUserNameAndPassword_RegistersAccount()
        {
            // Arrange
            var expectedResult = true;

            // Act - just an invocation
            bool actualResult = sut.Register(username, password);

            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void Register_BlankUserName_ThrowsUserNameRequiredException()
        {
            //Arrange
            username = "";

            //Assert
            Assert.ThrowsException<UserNameRequiredException>(
                () => sut.Register(username, password)
                );
        }

        [DataTestMethod]
        [DataRow("abolarteoutlookcom")]
        [DataRow("abolarte@outlookcom")]
        [DataRow("abolarte.outlook.com")]

        [TestMethod]
        public void Register_UserNameNotValidEmail_ThrowsInvalidEmailException(string username)
        {
            //Assert
            Assert.ThrowsException<InvalidEmailException>(
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
                () => sut.Register(username, password)
                );
        }



        [TestMethod]
        public void Register_PasswordLessThanMinimum_ThrowsMinimumLengthRequiredException()
        {
            //Arrange
            var password = "Bl@st12";

            //Assert
            Assert.ThrowsException<MinimumLengthRequiredException>(
                () => sut.Register(username, password)
                );
        }

        [DataTestMethod]
        [DataRow("bl@st123")]
        [DataRow("BL@ST123")]
        [DataRow("Blast123")]
        [DataRow("Bl@stabc")]

        [TestMethod]
        public void Register_PasswordNotStrongEnough_ThrowsStrongPasswordRequiredException(string password)
        {
            //Assert
            Assert.ThrowsException<StrongPasswordRequiredException>(
                () => sut.Register(username, password)
                );
        }

        [TestMethod]
        public void Register_WithValidUserNameAndPassword_ShouldCallRepository()
        {
            //Arrange

            //Act
            sut.Register(username, password);

            // Assert
            mockRepository
                .Verify(r => r.Create(It.IsAny<Account>()),
                    Times.Once());        
        }

    }
}
