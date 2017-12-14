using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlastAsia.DigiBook.Domain;
using System.Text.RegularExpressions;
using Moq;
using BlastAsia.DigiBook.Domain.Models;
using BlastAsia.DigiBook.Domain.Registration;
using BlastAsia.DigiBook.Domain.Registration.RegistrationExceptions;
    
namespace BlastAsia.DigiBook.Domain.Test.RegistrationTest

{
    [TestClass]
    public class RegistrationServiceTest
    {
        private string _username;
        private string _password;
        private RegistrationService _sut;
        private Mock<IAccountRepository> mockRepository;
        

        [TestInitialize]
        public void InitializeTest()
        {
            _username = "mmendez@blastasia.com";
            _password = "Bl@st123";
            mockRepository = new Mock<IAccountRepository>();
            _sut = new RegistrationService(mockRepository.Object);

            
        }

        [TestCleanup]
        public void Cleanup() { }        

        [TestMethod]
        public void Register_ValidUsernamePassword_RegistersAccount()
        {
            // ARRANGE
           
            var expectedResult = true;

            // ACT
            bool actualResult = _sut.Register(_username, _password);

            // ASSERT
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void Register_BlankPassword_ThrowsPpasswordRequiredException()
        {
            // ARRANGE
            _password = "";

            // ACT
            // ASSERT
            Assert.ThrowsException<PasswordRequiredExeption>( () => _sut.Register(_username, _password));
        }

        [TestMethod]
        public void Register_BlankUserName_ThrowsUsernameRequiredException()
        {
            // ARRANGE
            _username = "";

            // ACT
            // ASSERT
            Assert.ThrowsException<UsernameRequiredException>(
                    () => _sut.Register(_username, _password));
        }

        [TestMethod]
        public void Register_passwordLessThanMinimum_ThrowsMinimumLengthException()
        {
            // ARRANGE
            _password = "Bl@st12";

            // ACT

            // ASSERT

            Assert.ThrowsException<MinimumLengthRequiredException>(
                    () => _sut.Register(_username, _password));
        }
        
        [DataTestMethod]
        [DataRow("mmendezblastasia.com")]
        [DataRow("mmendezblastasia.com")]
        [DataRow("mmendez.blastasia.com")]
        public void Register_UserNameNotValidEmail_RegistersAccount(string username)
        {
            // ACT
            // ASSERT
            Assert.ThrowsException<InvalidUserNameFormatException>(
                    () => _sut.Register(username, _password));
        }

        [DataTestMethod]
        //[DataRow("bl@st123")]
        [DataRow("BL@ST123")]
        [DataRow("Bl@stttt")]
        [DataRow("Blast123")]
        public void Register_NotStrongPassword_ThrowsStrongPasswordRequired(string password)
        {
            // ASSERT
            Assert.ThrowsException<StrongPasswordRequiredException>(
                () => _sut.Register(_username, password));
        }

        
        [TestMethod]
        public void Register_WithValidUserNameAndPassword_ShouldCallRepository()
        {
            // ARRANGE

            
            // ACT
            _sut.Register(_username, _password);

            // ASSERT
            mockRepository.Verify(r => r.Create(It.IsAny<Account>()), Times.Once);

        }

    }
}
