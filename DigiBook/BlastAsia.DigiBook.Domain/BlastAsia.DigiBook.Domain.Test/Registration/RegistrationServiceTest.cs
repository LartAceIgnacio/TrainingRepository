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
        private Mock<IAccountRepository> mockRepository;
        RegistrationService sut;

        [TestInitialize]
        public void InitializeTest()            
        {
            username = "renznebran@gmail.com";
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
            //Arrange
          
            var expectedResult = true;
            //sut = system under test

            //Act 
          
            bool actualResult = sut.Register(username, password);

           
            //Assert
        
            Assert.AreEqual(expectedResult, actualResult);
            
         
        }

       
        [TestMethod]
        public void Register_BlankPassword_ThrowsPasswordRequiredException()
        {
            // Arrange
            password = "";
            
            
            // Assert
            Assert.ThrowsException<PasswordRequiredException>(
                () => sut.Register(username, password)
                );
        }


        [TestMethod]
        public void Register_MinimumLength_ThrowsPasswordMinimumLengthException()
        {
            // Arrange
            password = "Bl@st12";

            // Assert
            Assert.ThrowsException<MinimumLengthException>(
                () => sut.Register(username, password)
                );
        }

        [TestMethod]
        public void Register_BlankUsername_ThrowsUsernameRequiredException()
        {
            // Arrange
            username = "";
            

            // Assert
            Assert.ThrowsException<UsernameRequiredException>(
                () => sut.Register(username, password)
                );
        }

        // Invalid inputs
        [DataTestMethod]
        [DataRow("bl@st123")]
        [DataRow("BL@ST123")]
        [DataRow("Blast123")]
        [DataRow("Bl@stttt")]

        [TestMethod]
        public void Register_MustHaveStrongPassword_ThrowsStrongPasswordException(string password)
        {
            // Arrange
            

            // Assert
            Assert.ThrowsException<StrongPasswordException>(
                () => sut.Register(username, password)
                );
        }

        //Invalid inputs
        [DataTestMethod]
        [DataRow("renznebran")]
        [DataRow("renz.nebran.com")]
        [DataRow("renznebran@yahoo..com")]
        [DataRow("renznebran@yahoo.com.")]
        [DataRow("renznebran@@yahoo.com")]
        [TestMethod]
        public void Register_ValidEmail_ThrowsValidEmailException(string username)
        {
            // Arrange
            

            // Assert
            Assert.ThrowsException<ValidEmailException>(
                () => sut.Register(username, password)
                );
        }

        [TestMethod]
        public void Register_WithValidUsernameAndPassword_ShouldCallRepositoryCreate()
        {
            sut.Register(username, password);

            mockRepository
                .Verify(rp => rp.Create(It.IsAny<Account>()),
                    Times.Once());



        }
    }
}
