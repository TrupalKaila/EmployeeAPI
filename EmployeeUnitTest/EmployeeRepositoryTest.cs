using EmployeeAPI.Database;
using EmployeeAPI.Model;
using EmployeeAPI.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeUnitTest
{
    public class EmployeeRepositoryTest
    {
        public static EmployeeDBContext CreateContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<EmployeeDBContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
            return new EmployeeDBContext(options);
        }

        [Fact]
        public void GetAllEmployees_ReturnsEmployeeList()
        {
            string dbName = Guid.NewGuid().ToString();
            using (var context = CreateContext(dbName))
            {
                context.Departments.AddRange(
                    new Department { Id = 1, DepartmentName = "IT" },
                    new Department { Id = 2, DepartmentName = "HR" }
                );
                context.Employees.AddRange(
                    new Employee { Id = 1, FirstName = "Peter", LastName = "Parker", Age = 22, DeptId = 1 },
                    new Employee { Id = 2, FirstName = "Tony", LastName = "Stark", Age = 44, DeptId = 2 }
                );
                context.SaveChanges();
            }

            using (var context = CreateContext(dbName))
            {
                var repo = new EmployeeRepository(context);
                var returnedList = repo.GetAllEmployees();

                Assert.NotNull(returnedList);
                Assert.Equal(2, returnedList.Count);
            }
        }
        [Fact]
        public void GetEmployeeById_ReturnEmployee()
        {
            string dbName = Guid.NewGuid().ToString();
            using (var context = CreateContext(dbName))
            {
                context.Departments.Add(new Department { Id = 1, DepartmentName = "IT" });
                context.Employees.Add(new Employee { Id = 1, FirstName = "Peter", LastName = "Parker", Age = 22, DeptId = 1 });
                context.SaveChanges();
            }
            using (var context = CreateContext(dbName))
            {
                var repo = new EmployeeRepository(context);
                var returnedEmployee = repo.GetEmployeeById(1);

                Assert.NotNull(returnedEmployee);
                Assert.NotNull(returnedEmployee.Department); 
                Assert.Equal(1, returnedEmployee.Id);
                Assert.Equal("Peter", returnedEmployee.FirstName);
                Assert.Equal("IT", returnedEmployee.Department.DepartmentName); 
            }
        }

        [Fact]
        public void AddEmployee_AddSuccessfully()
        {
            string dbName = Guid.NewGuid().ToString();
            Employee employee = new Employee() { Id = 1, FirstName = "Peter", LastName = "Parker", Age = 22, DeptId = 1 };

            using (var context = CreateContext(dbName))
            {
                var repo = new EmployeeRepository(context);
                repo.AddEmployee(employee);

                var stored = context.Employees.FirstOrDefault(e => e.Id == employee.Id);
                Assert.Equal("Peter", stored.FirstName);
            }
        }
        [Fact]
        public void DeleteEmployee_DeletedSuccessfully()
        {
            string dbName = Guid.NewGuid().ToString();
            Employee employee = new Employee() { Id = 1, FirstName = "Peter", LastName = "Parker", Age = 22, DeptId = 1 };

            using (var context = CreateContext(dbName))
            {
                context.Employees.Add(employee);
                context.SaveChanges();
            }

            using (var context = CreateContext(dbName))
            {
                var repo = new EmployeeRepository(context);
                repo.DeleteEmployee(employee);

                var exist = context.Employees.FirstOrDefault(e => e.Id == 1);

                Assert.Null(exist);
            }
        }
        [Fact]
        public void UpdateEmployee_SuccessfullyUpdated()
        {
            string dbName = Guid.NewGuid().ToString();

            using (var context = CreateContext(dbName))
            {
                context.Employees.Add(new Employee()
                {
                    Id = 1,
                    FirstName = "Peter",
                    LastName = "Parker",
                    Age = 22,
                    DeptId = 1
                });
                context.SaveChanges();
            }

            using (var context = CreateContext(dbName))
            {
                var repo = new EmployeeRepository(context);
                var employee = context.Employees.First(e => e.Id == 1);
                employee.FirstName = "Tony";
                repo.UpdateEmployee(employee);
            }

            using (var context = CreateContext(dbName))
            {
                var updatedEmployee = context.Employees.First(e => e.Id == 1);

                Assert.Equal("Tony", updatedEmployee.FirstName);
            }
        }
    }
}