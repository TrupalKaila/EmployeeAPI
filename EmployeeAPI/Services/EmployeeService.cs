using EmployeeAPI.Interfaces;
using EmployeeAPI.Model;
using EmployeeAPI.DTOs;
using System.Linq;

namespace EmployeeAPI.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public List<EmployeeDto> GetAllEmployees()
        {
            var employees = _employeeRepository.GetAllEmployees()
                .Select(e => new EmployeeDto
                {
                    Id = e.Id,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Age = e.Age,
                    DeptId = e.DeptId
                })
                .ToList();
            return employees;
        }

        public EmployeeDto GetEmployeeById(int id)
        {
            var emp = _employeeRepository.GetEmployeeById(id);
            if (emp == null)
            {
                return null;

            }
            var empDTO = new EmployeeDto
            {
                Id = emp.Id,
                FirstName = emp.FirstName,
                LastName = emp.LastName,
                Age = emp.Age,
                DeptId = emp.DeptId
            };
            return empDTO;
        }

        public void AddEmployee(EmployeeCreateDto employee)
        {
            var emp = new Employee
            {
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Age = employee.Age,
                DeptId = employee.DeptId
            };

            _employeeRepository.AddEmployee(emp);
        }

        public bool UpdateEmployee(EmployeeUpdateDto employee)
        {
            var existing = _employeeRepository.GetEmployeeById(employee.Id);
            if (existing == null)
            {
                return false;
            }
            existing.FirstName = employee.FirstName;
            existing.LastName = employee.LastName;
            existing.Age = employee.Age;
            existing.DeptId = employee.DeptId;

            _employeeRepository.UpdateEmployee(existing);
            return true;
        }

        public bool DeleteEmployee(int id)
        {
            var emp = _employeeRepository.GetEmployeeById(id);
            if (emp == null)
            {
                return false;
            }
            _employeeRepository.DeleteEmployee(emp);
            return true;
        }
    }
}