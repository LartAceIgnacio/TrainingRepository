using BlastAsia.DigiBook.Api.Controllers;
using BlastAsia.DigiBook.Domain.Employees;
using BlastAsia.DigiBook.Domain.Models.Employees;
using BlastAsia.DigiBook.Domain.Models.Pagination;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;

namespace BlastAsia.DigiBook.Api.Test
{
    [TestClass]
    public class EmployeesControllerTest
    {
        private static Mock<IEmployeeRepository> mockRepo;
        private static Mock<IEmployeeService> mockService;

        private EmployeesController sut;
        private Employee employee;
        private JsonPatchDocument patch;


        private readonly Guid existingId = Guid.NewGuid();
        private readonly Guid nonExistingId = Guid.Empty;


        [TestInitialize]
        public void Initialize()
        {

            employee = new Employee
            {
                EmployeeId = new Guid(),
                FirstName = "Emmanuel",
                LastName = "Magadia",
                MobilePhone = "09279528841",
                EmailAddress = "emagadia@blastasia.com",
                Photo = new MemoryStream(),
                OfficePhone = "123-123-123",
                Extension = "asdasd"
            };

            patch = new JsonPatchDocument();

            mockService = new Mock<IEmployeeService>();
            mockRepo = new Mock<IEmployeeRepository>();


            mockRepo
                .Setup(
                    r => r.Retrieve(existingId)
                )
                .Returns(employee);

            mockRepo
              .Setup(
                  r => r.Retrieve(nonExistingId)
              )
              .Returns<Employee>(null);

            mockService
               .Setup(
                   s => s.Save(existingId, employee)
               )
               .Returns(employee);


            sut = new EmployeesController(mockRepo.Object, mockService.Object);
        }
        [TestCleanup]
        public void Cleanup()
        {

        }
        // Get
        [TestMethod]
        public void GetEmployee_WithNoId_ShouldReturnOkObjectValue()
        {
            // arrange

            mockRepo
                .Setup(
                    r => r.Retrieve()
                )
                .Returns(new List<Employee>());

            // act
            var result = sut.GetEmployee(null);

            // assert

            mockRepo
               .Verify(
                   r => r.Retrieve(), Times.Once()
               );

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

        }


        [TestMethod]
        public void GetEmployee_WithId_ShouldReturnOkObjectValue()
        {
            // arrange
            var id = Guid.NewGuid();
            mockRepo
                .Setup(
                    r => r.Retrieve(id)
                )
                .Returns(new Employee());

            // act
            var result = sut.GetEmployee(id);
            // assert
            mockRepo
               .Verify(
                   r => r.Retrieve(id), Times.Once()
               );

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void GetEmployee_WithPaginationWithValidData_ShouldReturnOkObjectValue()
        {
            // arrange
            var pageNumber = 1;
            var recordNumber = 3;
            var keyWord = "em";

            mockRepo
                .Setup(
                    r => r.Retrieve(pageNumber, recordNumber, keyWord)
                )
                .Returns(new Pagination<Employee>());

            // act 
            var result = sut.GetEmployee(pageNumber, recordNumber, keyWord);

            // assert
            mockRepo
                .Verify(
                    r => r.Retrieve(pageNumber, recordNumber, keyWord), Times.Once
                );

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

        }

        [TestMethod]
        public void GetEmployee_WithPaginationWithInvalidPageNumber_ShouldReturnOkObjectValue()
        {
            // arrange
            var pageNumber = -1;
            var recordNumber = 3;
            var keyWord = "em";

            mockRepo
                .Setup(
                    r => r.Retrieve(pageNumber, recordNumber, keyWord)
                )
                .Returns(new Pagination<Employee>());

            // act 
            var result = sut.GetEmployee(pageNumber, recordNumber, keyWord);

            // assert
            mockRepo
                .Verify(
                    r => r.Retrieve(pageNumber, recordNumber, keyWord), Times.Once
                );

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

        }

        [TestMethod]
        public void GetEmployee_WithPaginationWIthInvalidRecordNumber_ShouldReturnOkObjectValue()
        {
            // arrange
            var pageNumber = 1;
            var recordNumber = -3;
            var keyWord = "em";

            mockRepo
                .Setup(
                    r => r.Retrieve(pageNumber, recordNumber, keyWord)
                )
                .Returns(new Pagination<Employee>());

            // act 
            var result = sut.GetEmployee(pageNumber, recordNumber, keyWord);

            // assert
            mockRepo
                .Verify(
                    r => r.Retrieve(pageNumber, recordNumber, keyWord), Times.Once
                );

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

        }

        [TestMethod]
        public void GetEmployee_WithPaginationWithInvalidKeyWord_ShouldReturnOkObjectValue()
        {
            // arrange
            var pageNumber = 1;
            var recordNumber = 3;
            var keyWord = "";

            mockRepo
                .Setup(
                    r => r.Retrieve(pageNumber, recordNumber, keyWord)
                )
                .Returns(new Pagination<Employee>());

            // act 
            var result = sut.GetEmployee(pageNumber, recordNumber, keyWord);

            // assert
            mockRepo
                .Verify(
                    r => r.Retrieve(pageNumber, recordNumber, keyWord), Times.Once
                );

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

        }


        // Post
        [TestMethod]
        public void CreateEmployee_WithNullEmployee_ShouldReturnBadRequest()
        {
            // arrange
            employee = null;

            // act 
            var result = sut.CreateEmployee(employee);

            // assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));

            mockService
                .Verify(
                    r => r.Save(Guid.Empty, employee), Times.Never()
                );
        }

        [TestMethod]
        public void CreateEmployee_WithInvalidEmployee_ShouldReturnBadRequest()
        {
            // arrange
            employee.FirstName = "";

            mockService
                .Setup(
                    s => s.Save(employee.EmployeeId, employee)
                )
                .Returns(() => throw new Exception());

            // act
            var result = sut.CreateEmployee(employee);

            // assert
            mockService
            .Verify(
                r => r.Save(employee.EmployeeId, employee), Times.Once()
            );

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));

        }

        [TestMethod]
        public void CreateEmployee_WithvalidEmployee_ShouldReturnOkObjectRsult()
        {
            // arrange
            mockService
               .Setup(
                   s => s.Save(employee.EmployeeId, employee)
               )
               .Returns(new Employee());
            // act
            var result = sut.CreateEmployee(employee);
            // assert
            mockService
                .Verify(
                    s => s.Save(Guid.Empty, employee), Times.Once()
                );

            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
        }

        [TestMethod]
        public void DeleteEmployee_WithNonExistingEmployeeId_SohouldReturnNotFound()
        {
            // arrange
            // act 
            var result = sut.DeleteEmployee(nonExistingId);

            // assert

            mockRepo
                .Verify(
                    r => r.Retrieve(nonExistingId), Times.Once()
                );

            mockRepo
              .Verify(
                  r => r.Delete(nonExistingId), Times.Never()
              );

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void DeleteEmployee_WithExistingEmployeeId_SohouldReturnNoContent()
        {
            // arrange
            // act 
            var result = sut.DeleteEmployee(existingId);

            // assert

            mockRepo
                .Verify(
                    r => r.Retrieve(existingId), Times.Once()
                );

            mockRepo
             .Verify(
                 r => r.Delete(existingId), Times.Once()
             );

            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }


        [TestMethod]
        public void UpdateEmployee_WithNullEmployee_ShouldReturnBadRequest()
        {
            // arrange
            employee = null;
            var id = existingId;

            // act 
            var result = sut.UpdateEmployee(employee, id);

            // assert
            mockRepo
                .Verify(
                    r => r.Retrieve(id), Times.Never()
                );

            mockService
                .Verify(
                    r => r.Save(id, employee), Times.Never()
                );

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void UpdateEmployee_WithNonExistingEmployeeid_ShouldReturnNotFoundResult()
        {
            // act
            var result = sut.UpdateEmployee(employee, nonExistingId);
            // assert 
            mockRepo
               .Verify(
                   r => r.Retrieve(nonExistingId), Times.Once()
               );

            mockService
                .Verify(
                    r => r.Save(nonExistingId, employee), Times.Never()
                );

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void UpdateEmployee_WithValidData_ShouldReturnOkResult()
        {
            // act
            var result = sut.UpdateEmployee(employee, existingId);


            // assert 

            mockRepo
               .Verify(
                   r => r.Retrieve(existingId), Times.Once()
            );

            mockService
                .Verify(
                    s => s.Save(existingId, employee), Times.Once()
                );

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void PatchEmployee_WithNullPatchEmployee_ShouldReturnBadRequest()
        {
            // arrange
            patch = null;

            var id = existingId;

            // act 
            var result = sut.PatchEmployee(patch, id);

            // assert
            mockRepo
                .Verify(
                    r => r.Retrieve(id), Times.Never()
                );

            mockService
                .Verify(
                    r => r.Save(id, employee), Times.Never()
                );

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }


        [TestMethod]
        public void PatchEmployee_WithNonExistingEmployeeid_ShouldReturnNotFoundResult()
        {
            // act
            var result = sut.PatchEmployee(patch, nonExistingId);
            // assert 
            mockRepo
               .Verify(
                   r => r.Retrieve(nonExistingId), Times.Once()
               );

            mockService
                .Verify(
                    r => r.Save(nonExistingId, employee), Times.Never()
                );

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void PatchEmployee_WithValidData_ShouldReturnOkResult()
        {
            // act
            var result = sut.PatchEmployee(patch, existingId);
            // assert 

            mockRepo
               .Verify(
                   r => r.Retrieve(existingId), Times.Once()
            );

            mockService
                .Verify(
                    s => s.Save(existingId, employee), Times.Once()
                );

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

        }
    }
}


