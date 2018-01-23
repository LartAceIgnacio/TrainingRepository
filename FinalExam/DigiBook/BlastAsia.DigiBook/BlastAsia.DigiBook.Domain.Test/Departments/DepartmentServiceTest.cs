using BlastAsia.DigiBook.Domain.Departments;
using BlastAsia.DigiBook.Domain.Employees;
using BlastAsia.DigiBook.Domain.Models.Departments;
using BlastAsia.DigiBook.Domain.Models.Employees;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Test.Departments
{
    [TestClass]
    public class DepartmentServiceTest
    {
        private Department department;
        private Employee employee;
        private Mock<IDepartmentRepository> mockDepartmentRepository;
        private Mock<IEmployeeRepository> mockEmployeeRepository;

        private DepartmentService sut;

        private Guid existingDepartmentId = Guid.NewGuid();
        private Guid nonExistingDepartmentId = Guid.Empty;

        private Guid existingEmployeeId = Guid.NewGuid();
        private Guid nonExistingEmployeeId = Guid.Empty;

        [TestInitialize]
        public void Initialize()
        {
            department = new Department
            {
                DepartmentName = "Department A",
                DeparmentMemberId = existingEmployeeId
            };

            mockDepartmentRepository = new Mock<IDepartmentRepository>();
            mockEmployeeRepository = new Mock<IEmployeeRepository>();

            sut = new DepartmentService(mockDepartmentRepository.Object,
                mockEmployeeRepository.Object);

            mockDepartmentRepository
            .Setup(d => d.Create(department))
            .Callback(() => department.DepartmentId = Guid.NewGuid())
            .Returns(department);

            mockEmployeeRepository
              .Setup(e => e.Retrieve(existingEmployeeId))
              .Returns(new Employee());

            mockDepartmentRepository
                .Setup(d => d.Retrieve(existingDepartmentId))
                .Returns(department);

        }
        [TestMethod]
        public void Save_NewDepartmentWithValidData_ShouldCallRepositoryCreate()
        {
            //Arrange
            department.DeparmentMemberId = existingEmployeeId;
            department.DepartmentId = nonExistingDepartmentId;
            //Act

            var result = sut.Save(department.DepartmentId,department);

            //Assert
            //mockDepartmentRepository
            //    .Verify(d => d.Retrieve(department.DepartmentId), Times.Once());

            mockDepartmentRepository
                .Verify(d => d.Create(department), Times.Once());
        }
        [TestMethod]
        public void Save_DepartmentWithDepartmentMemberId_ThrowsDepartmentNameIdRequiredException()
        {
            //Arrange
            department.DeparmentMemberId = Guid.Empty;

            //Assert
            Assert.ThrowsException<DepartmentMemberIdRequiredException>
                (() => sut.Save(department.DeparmentMemberId, department));

            mockDepartmentRepository
                .Verify(a => a.Retrieve(It.IsAny<Guid>()), Times.Never);
        }
        [TestMethod]
        public void Save_WithExistingDepartment_CallsRepositoryUpdate()
        {
            //Arrange
            department.DepartmentId = existingDepartmentId;

            //Act
            sut.Save(department.DepartmentId,department);

            //Assert
            mockDepartmentRepository
                .Verify(d => d.Retrieve(existingDepartmentId), Times.Once);

            mockDepartmentRepository
                .Verify(d => d.Update(existingDepartmentId,department), Times.Once);
        }

        [TestMethod]
        public void Save_WithBlankDepartmentName_ThrowsDepartmentNameRequiredException()
        {
            //Arrange
            department.DepartmentName = "";

            //Act
           
            //Assert
            Assert.ThrowsException<DepartmentNameRequiredException>(
                () => sut.Save(department.DepartmentId,department));
        }
        //[TestMethod]
        //public void Save_WithBlankRole_ThrowsRoleRequiredException()
        //{
        //    //Arrange
        //    department.Role = "";

        //    //Act
             
        //    //Assert
        //    Assert.ThrowsException<RoleRequiredException>(
        //        () => sut.Save(department));
        //}
    }
}
