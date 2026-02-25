using EmployeeAPI.DTOs;
using EmployeeAPI.Interfaces;
using EmployeeAPI.Model;
using EmployeeAPI.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeUnitTest
{
    public class EmployeeServiceTest
    {
        [Fact]
        public void GetAllEmployees_ReturnEmployeeDtoList()
        {
            //Arrange
            List<Employee> employees = new List<Employee>()
            {
                new Employee(){Id = 1, FirstName = "Jack"},
                new Employee(){Id = 2, FirstName = "John"}
            };
            var repo = new Mock<IEmployeeRepository>();
            repo.Setup(x => x.GetAllEmployees()).Returns(employees);
            var service = new EmployeeService(repo.Object);

            //Act
            var result = service.GetAllEmployees();

            //Assert
            Assert.NotNull(result);
            var data = Assert.IsType<List<EmployeeDto>>(result);
            Assert.Equal(2, data.Count);
        }
        [Fact]
        public void GetEmployeeById_EmployeeExist_ReturnsOk()
        {
            //Arrange
            Employee employee = new Employee() { Id = 1, FirstName = "Peter" };
            EmployeeDto empdto = new EmployeeDto() { Id = 1, FirstName = "Peter" };

            var repo = new Mock<IEmployeeRepository>();
            repo.Setup(x => x.GetEmployeeById(1)).Returns(employee);
            var service = new EmployeeService(repo.Object);

            //Act
            var result = service.GetEmployeeById(1);

            //Assert
            var data = Assert.IsType<EmployeeDto>(result);
            Assert.Equal(empdto.FirstName, data.FirstName);
        }
        [Fact]
        public void GetEmployeeById_EmployeeNotExist_ReturnsNull()
        {
            Employee employee = null;
            var repo = new Mock<IEmployeeRepository>();
            repo.Setup(x => x.GetEmployeeById(1)).Returns(employee);
            var service = new EmployeeService(repo.Object);

            var result = service.GetEmployeeById(1);

            Assert.Null(result);
            repo.Verify(x => x.GetEmployeeById(1), Times.Once);
        }
        [Fact]
        public void AddEmployee_ValidData_CallRepositoryPerfectly()
        {
            // Arrange
            EmployeeCreateDto employeeDto = new EmployeeCreateDto
            {
                FirstName = "Jack",
                LastName = "John",
                Age = 30,
                DeptId = 1
            };

            var repo = new Mock<IEmployeeRepository>();
            var service = new EmployeeService(repo.Object);

            // Act
            service.AddEmployee(employeeDto);

            // Assert
            repo.Verify(x => x.AddEmployee(It.Is<Employee>(e =>
                e.FirstName == employeeDto.FirstName &&
                e.LastName == employeeDto.LastName &&
                e.Age == employeeDto.Age &&
                e.DeptId == employeeDto.DeptId
            )), Times.Once);
        }

        [Fact]
        public void UpdateEmployee_EmployeeExists_ReturnsTrue()
        {
            // Arrange
            var employeeDto = new EmployeeUpdateDto
            {
                Id = 1,
                FirstName = "Updated",
                LastName = "John",
                Age = 35,
                DeptId = 2
            };
            var existing = new Employee
            {
                Id = 1,
                FirstName = "Jack",
                LastName = "John",
                Age = 40,
                DeptId = 2
            }; ;
            var repo = new Mock<IEmployeeRepository>();
            repo.Setup(x => x.GetEmployeeById(1)).Returns(existing);

            var service = new EmployeeService(repo.Object);

            // Act
            var result = service.UpdateEmployee(employeeDto);

            // Assert
            Assert.True(result);
            repo.Verify(x => x.UpdateEmployee(existing), Times.Once);
        }
        [Fact]
        public void UpdateEmployee_EmployeeNotFound_ReturnsFalse()
        {
            //Arrange
            var employeeDto = new EmployeeUpdateDto { Id = 1 };
            Employee employee = null;
            var repo = new Mock<IEmployeeRepository>();
            repo.Setup(x => x.GetEmployeeById(1)).Returns(employee);
            var service = new EmployeeService(repo.Object);

            //Act
            var result = service.UpdateEmployee(employeeDto);

            //Assert
            Assert.False(result);
            repo.Verify(x => x.UpdateEmployee(employee), Times.Never);
        }
        [Fact]
        public void DeleteEmployee_EmployeeExists_ReturnsTrue()
        {
            //Arrange
            var employee = new Employee { Id = 1, FirstName = "Jack" };
            var repo = new Mock<IEmployeeRepository>();
            repo.Setup(x => x.GetEmployeeById(1)).Returns(employee);
            var service = new EmployeeService(repo.Object);

            //Act
            var result = service.DeleteEmployee(1);

            //Assert
            Assert.True(result);
            repo.Verify(x => x.DeleteEmployee(employee), Times.Once);
        }
        [Fact]
        public void DeleteEmployee_EmployeeNotFound_ReturnsFalse()
        {
            //Arrange
            Employee employee = null;
            var repo = new Mock<IEmployeeRepository>();
            repo.Setup(x => x.GetEmployeeById(1)).Returns(employee);
            var service = new EmployeeService(repo.Object);

            //Act
            var result = service.DeleteEmployee(1);

            //Assert
            Assert.False(result);
            repo.Verify(x => x.DeleteEmployee(employee), Times.Never);
        }
    }
}
