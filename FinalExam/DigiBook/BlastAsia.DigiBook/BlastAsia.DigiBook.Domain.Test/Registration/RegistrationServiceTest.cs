using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BlastAsia.DigiBook.Domain.Models.Registration;

namespace BlastAsia.DigiBook.Domain.Test
{
    [TestClass]
    public class RegistrationServiceTest
    {
        private string username;
        private string password;

        private Mock<IAccountRepository> mockRepository;
        RegistrationService sut;

        [TestInitialize]
        public void InitializeTest()
        {
            username = "eravina@blastasia.com";
            password = "Bl@st123";

            mockRepository = new Mock<IAccountRepository>();
            sut = new RegistrationService(mockRepository.Object);
        }
        
        [TestCleanup]

        [TestMethod]
        public void Register_ValidUserNameAndPassword_RegistersAccount()
        {
       
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
         
            var password = "";

            // Assert
            Assert.ThrowsException<PasswordRequiredException>(
                () => sut.Register(username,password));
        }
        [TestMethod]
        public void Register_PasswordLessThanMinimumLength_ThrowsMinimumRequired()
        {
            // Arrange 
           
            var password = "Bl@st12";

            // Assert
            Assert.ThrowsException<MinimumLengthRequiredException>(
                () => sut.Register(username, password));

        }

        [TestMethod]
        public void Register_BlankUserName_ThrowsUserNameRequiredException()
        {
            //Arrange
            var username = "";
 
            // Assert
            Assert.ThrowsException<UserNameRequiredException>(
                () => sut.Register(username, password));

        }

        [DataTestMethod]
        [DataRow("eugenejhonravinacom")]
        [DataRow("ejravina@blastasia")]
        [DataRow("ejravina.blastasia.com")]
        [DataRow("ejravina@.com")]
        [DataRow("ejravina@yahoo..com")]
        public void Register_ValidUsername_RegisterAccount(string username)
        {
 
            // Assert
            Assert.ThrowsException<UsernameValidEmailRequired>(
              () => sut.Register(username, password));

        }
        [DataTestMethod]
        [DataRow("bl@st123")]
        [DataRow("BL@ST123")]
        [DataRow("Blast123")]
        [DataRow("Bl@stabc")]
        public void Register_NotStrongPasword_ThrowsStrongPasswordRequired(string password)
        {

            // Assert
            Assert.ThrowsException<StrongPasswordException>(
                () => sut.Register(username, password));
        }

        [TestMethod]
        public void Register_WithValidUsernameAndPassword_ShouldCallRepositoryCreate()
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
