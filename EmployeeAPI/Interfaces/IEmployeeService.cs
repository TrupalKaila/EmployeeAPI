using EmployeeAPI.DTOs;
using EmployeeAPI.Model;

namespace EmployeeAPI.Interfaces
{
    public interface IEmployeeService
    {
        List<EmployeeDto> GetAllEmployees();
        EmployeeDto GetEmployeeById(int id);
        void AddEmployee(EmployeeCreateDto employee);
        bool UpdateEmployee(EmployeeUpdateDto employee);
        bool DeleteEmployee(int id);
    }
}
