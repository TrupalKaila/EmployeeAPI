using EmployeeAPI.Interfaces;
using EmployeeAPI.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _service;
        public EmployeeController(IEmployeeService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var employees = _service.GetAllEmployees();
            return Ok(employees);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetEmployee(int id)
        {
            var emp = _service.GetEmployeeById(id);
            if (emp == null)
            {
                return NotFound("User Not Found");
            }
            return Ok(emp);
        }

        [HttpPost]
        public IActionResult AddEmployee(EmployeeCreateDto employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _service.AddEmployee(employee);
            return Ok("Employee Created");
        }

        [HttpPut]
        public IActionResult UpdateEmployee(EmployeeUpdateDto employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = _service.UpdateEmployee(employee);
            if (!result)
            {
                return NotFound();
            }
            return Ok("Employee Updated!");
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteEmployee(int id)
        {
            var result = _service.DeleteEmployee(id);
            if (!result)
            {
                return NotFound();
            }
            return Ok("Employee Deleted");
        }
    }
}
