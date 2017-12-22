using BlastAsia.Digibook.API.Controllers;
using BlastAsia.Digibook.Domain.Employees;
using BlastAsia.Digibook.Domain.Models.Employees;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.Digibook.API.Test
{
    [TestClass]
    public class EmployeeControllerTest
    {
        [TestMethod]
        public void GetEmployee_WithoutContactId_ReturnsEmployeeList()
        {
            Mock<IEmployeeService> mockEmployeeService = new Mock<IEmployeeService>();
            Mock<IEmployeeRepository> mockEmployeeRepository = new Mock<IEmployeeRepository>();
            EmployeeController sut = new EmployeeController(mockEmployeeService.Object, mockEmployeeRepository.Object);
            Employee employee;

            mockEmployeeRepository
                .Setup(r => r.Retrieve())
                .Returns(new List<Employee>());

            var result = sut.GetEmployee(null);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }
    }
}
