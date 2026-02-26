using EmployeeAPI.Database;
using EmployeeAPI.Interfaces;
using EmployeeAPI.Model;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Eventing.Reader;

namespace EmployeeAPI.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EmployeeDBContext _context;
        public EmployeeRepository(EmployeeDBContext context)
        {
            _context = context;
        }

        public List<Employee> GetAllEmployees()
        {
            // Include Department navigation so consumers can access department data
            return _context.Employees
                .Include(e => e.Department)
                .ToList();
        }

        public Employee GetEmployeeById(int id)
        {
            // Include Department navigation
            return _context.Employees
                .Include(e => e.Department)
                .FirstOrDefault(x => x.Id == id);
        }

        public void AddEmployee(Employee employee)
        {
            _context.Employees.Add(employee);
            _context.SaveChanges();
        }

        public void UpdateEmployee(Employee employee)
        {
            _context.Employees.Update(employee);
            _context.SaveChanges();
        }

        public void DeleteEmployee(Employee employee)
        {
            _context.Employees.Remove(employee);
            _context.SaveChanges();
        }
    }
}
