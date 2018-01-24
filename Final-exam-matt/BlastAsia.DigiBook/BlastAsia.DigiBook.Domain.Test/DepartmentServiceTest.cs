using System;
using System.Collections.Generic;
using System.Text;
using BlastAsia.DigiBook.Domain.Departments;
using BlastAsia.DigiBook.Domain.Models.Departments;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BlastAsia.DigiBook.Domain.Test
{
    [TestClass]
    public class DepartmentServiceTest
    {
        
        Mock<IDepartmentRepository> _mockDepartmentRepo;
        Department department;
        DepartmentService _sut;

        [TestInitialize]
        public void Initialize() { }

        [TestMethod]
        [TestProperty("Service Test", "Department")]
        public void Save_WithValidData_ShouldCallCreate()
        {
            _mockDepartmentRepo = new Mock<IDepartmentRepository>();

            _sut = new DepartmentService(_mockDepartmentRepo.Object);
            
            department = new Department();
            _mockDepartmentRepo.Setup(repo => repo.Retrieve(department.Id))
                .Returns(department);


            _sut.Save(Guid.Empty, department);

            Assert.IsNotNull(department);
            _mockDepartmentRepo.Verify(repo => repo.Create(department), Times.Once);
            _mockDepartmentRepo.Verify(repo => repo.Retrieve(department.Id), Times.Once);
        }
    }
}
