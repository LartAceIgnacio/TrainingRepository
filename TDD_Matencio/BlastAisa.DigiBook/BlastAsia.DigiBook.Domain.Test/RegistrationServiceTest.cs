using BlastAsia.DigiBook.Domain.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BlastAsia.DigiBook.Domain.Test
{
    [TestClass]
    public class RegitrationServiceTest
    {

        private string username;
        private string password;
        private Mock<IAccountRepository> mockRepository;

        RegistrationService sut;

        [TestInitialize]

        public void TestInitialize()
        {
            username = "jhnkrl15@gmail.com";
            password = "Bl@st123";
            mockRepository = new Mock<IAccountRepository>();
            sut = new RegistrationService(mockRepository.Object);
        }

        [TestCleanup]

        public void CleanupTeset()
        {

        }

        //Test Method Naming Convention: MethodToBeTested_Condition_ThrowsException/Expectation

        // Happy path
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
        public void Register_BlankPassword_ThrowsPasswordRequiredException()
        {
            // Arrange
            password = "";
            // Assert
            Assert.ThrowsException<PasswordRequiredException>(() => sut.Register(username, password));
        }

        [TestMethod]
        public void Register_BlankUsername_ThrowsUsernameRequiredException()
        {
            // Arrange
            username = "";
            // Assert
            Assert.ThrowsException<UsernameRequiredException>(() => sut.Register(username, password));
        }

        [TestMethod]
        public void Register_PasswordLessThanLength_ThrowsMinimumLengthRequiredException()
        {
            // Arange
            password = "Bl@st12";
            // Assert
            Assert.ThrowsException<MinimumLengthRequiredException>(() => sut.Register(username, password));
        }

        [DataTestMethod]
        [DataRow("jhnkrl15gmailcom")]
        [DataRow("jhnkrl15gmail.com")]
        [DataRow("jhnkrl15@gmailcom")]
        public void Register_ValidEmailAddress_ThrowValidEmailException(string username)
        {
            // Assert
            Assert.ThrowsException<StrongPasswordRequired>(() => sut.Register(username, password));
        }

        [DataTestMethod]
        [DataRow("BL@ST123")]
        [DataRow("bl@st123")]
        [DataRow("Blast123")]
        [DataRow("Bl@stasd")]
        public void Register_InvalidPassword_ThrowsStrongPasswordRequired(string password)
        {
            // Assert
            Assert.ThrowsException<StrongPasswordRequired>(() => sut.Register(username, password));
        }

        [TestMethod]
        public void Register_WithValidUsernameAndPassword_ShouldCallRepository()
        {
            // Arrange

            // Act
            sut.Register(username,password);
            // Assert
            
            mockRepository.
                Verify(r => r.Create(It.IsAny<Account>()),
                    Times.Once());      
        }
    }
}
