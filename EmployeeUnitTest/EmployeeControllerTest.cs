using EmployeeAPI.Controllers;
using EmployeeAPI.DTOs;
using EmployeeAPI.Interfaces;
using EmployeeAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeUnitTest
{
    public class EmployeeControllerTest
    {
        [Fact]
        public void GetAllEmployees_ReturnsOk()
        {
            //Arrange
            List<EmployeeDto> employees = new List<EmployeeDto>()
            {
                new EmployeeDto(){Id = 1 , FirstName = "Jack"},
                new EmployeeDto(){Id = 2 , FirstName = "John"}
            };
            var service = new Mock<IEmployeeService>();
            service.Setup(x => x.GetAllEmployees()).Returns(employees);
            var controller = new EmployeeController(service.Object);

            //Act
            var result = controller.GetAll();

            //Assert
            var OkResultObj = Assert.IsType<OkObjectResult>(result);
            var data = Assert.IsAssignableFrom<List<EmployeeDto>>(OkResultObj.Value);
            service.Verify(x => x.GetAllEmployees(), Times.Once());
            Assert.Equal(2, data.Count);
        }
        [Fact]
        public void GetEmployee_EmployeeExist_ReturnsOK()
        {
            //Arrange
            EmployeeDto employee = new EmployeeDto() { Id = 1, FirstName = "Jack" };
            var service = new Mock<IEmployeeService>();
            service.Setup(x => x.GetEmployeeById(1)).Returns(employee);
            var controller = new EmployeeController(service.Object);

            //Act
            var result = controller.GetEmployee(1);

            //Assert
            var OkResultObj = Assert.IsType<OkObjectResult>(result);
            var data = Assert.IsAssignableFrom<EmployeeDto>(OkResultObj.Value);
            Assert.Equal(employee, data);
        }
        [Fact]
        public void GetEmployee_EmployeeNotExist_ReturnNotFound()
        {
            //Arrange
            EmployeeDto emp = null;
            var service = new Mock<IEmployeeService>();
            service.Setup(x => x.GetEmployeeById(1)).Returns(emp);
            var controller = new EmployeeController(service.Object);

            //Act
            var result = controller.GetEmployee(1);

            //Assert
            var NotFoundObject = Assert.IsType<NotFoundObjectResult>(result);
            var data = Assert.IsAssignableFrom<string>(NotFoundObject.Value);
            Assert.Equal("User Not Found", data);
        }
        [Fact]
        public void AddEmployee_ValidEmployee_ReturnsOk()
        {
            //Arrange
            EmployeeCreateDto emp = new EmployeeCreateDto() { FirstName = "Jack" };
            var service = new Mock<IEmployeeService>();
            var controller = new EmployeeController(service.Object);

            //Act
            var result = controller.AddEmployee(emp);

            //Assert
            var OkResultObject = Assert.IsType<OkObjectResult>(result);
            var data = Assert.IsAssignableFrom<string>(OkResultObject.Value);
            Assert.Equal("Employee Created", data);
            service.Verify(x => x.AddEmployee(emp), Times.Once());
        }
        [Fact]
        public void AddEmployee_InvalidEmployee_ReturnBadRequest()
        {
            //Arrange
            EmployeeCreateDto employee = new EmployeeCreateDto();
            var service = new Mock<IEmployeeService>();
            var controller = new EmployeeController(service.Object);
            controller.ModelState.AddModelError("LastName", "Required");

            //Act
            var result = controller.AddEmployee(employee);

            //Assert 
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            service.Verify(x => x.AddEmployee(It.IsAny<EmployeeCreateDto>()), Times.Never());
        }
        [Fact]
        public void UpdateEmployee_InvalidModel_ReturnsBadRequest()
        {
            //Arrange
            EmployeeUpdateDto employee = new EmployeeUpdateDto();
            var service = new Mock<IEmployeeService>();
            var controller = new EmployeeController(service.Object);
            controller.ModelState.AddModelError("LastName", "Required");

            //Act
            var result = controller.UpdateEmployee(employee);

            //Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            service.Verify(x => x.UpdateEmployee(employee), Times.Never());
        }
        [Fact]
        public void UpdateEmployee_EmployeeNotFound_ReturnsNotFound()
        {
            //Arrange
            EmployeeUpdateDto employee = new EmployeeUpdateDto() { Id = 1, FirstName = "Jack" };
            var service = new Mock<IEmployeeService>();
            service.Setup(x => x.UpdateEmployee(employee)).Returns(false);
            var controller = new EmployeeController(service.Object);

            //Act
            var result = controller.UpdateEmployee(employee);

            //Assert
            var notFound = Assert.IsType<NotFoundResult>(result);
            service.Verify(x => x.UpdateEmployee(employee), Times.Once());
        }
        [Fact]
        public void UpdateEmployee_Success_ReturnsOk()
        {
            //Arrange
            EmployeeUpdateDto employee = new EmployeeUpdateDto() { Id = 1, FirstName = "Jack" };
            var service = new Mock<IEmployeeService>();
            service.Setup(x => x.UpdateEmployee(employee)).Returns(true);
            var controller = new EmployeeController(service.Object);

            //Act
            var result = controller.UpdateEmployee(employee);

            //Assert
            var okResultObject = Assert.IsType<OkObjectResult>(result);
            var data = Assert.IsAssignableFrom<string>(okResultObject.Value);
            Assert.Equal("Employee Updated!", data);
        }
        [Fact]
        public void DeleteEmployee_Success_ReturnsOk()
        {
            //Arrange
            var service = new Mock<IEmployeeService>();
            service.Setup(x => x.DeleteEmployee(1)).Returns(true);
            var controller = new EmployeeController(service.Object);

            //Act
            var result = controller.DeleteEmployee(1);

            //Assert
            var okResultObject = Assert.IsType<OkObjectResult>(result);
            var data = Assert.IsAssignableFrom<string>(okResultObject.Value);
            Assert.Equal("Employee Deleted", data);
        }
        [Fact]
        public void DeleteEmployee_EmployeeNotFound_ReturnNotFound()
        {
            //Arrange
            var service = new Mock<IEmployeeService>();
            service.Setup(x => x.DeleteEmployee(1)).Returns(false);
            var controller = new EmployeeController(service.Object);

            //Act
            var result = controller.DeleteEmployee(1);

            //Assert
            var notFound = Assert.IsType<NotFoundResult>(result);
            service.Verify(x => x.DeleteEmployee(1), Times.Once); 
        }
    }
}
