using EmployeeAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace EmployeeAPI.Database
{
    public class EmployeeDBContext : DbContext
    {
        public EmployeeDBContext(DbContextOptions options) : base(options) { }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Employee>().HasData(
                new Employee() { Id = 1, FirstName = "Peter", LastName = "Parker", Age = 22, DeptId = 1 },
                new Employee() { Id = 2, FirstName = "Tony", LastName = "Stark", Age = 25, DeptId = 2 },
                new Employee() { Id = 3, FirstName = "Steve", LastName = "Rogers", Age = 45, DeptId = 3 }
                );
            modelBuilder.Entity<Department>().HasData(
                new Department() { Id = 1, DepartmentName = "IT" },
                new Department() { Id = 2, DepartmentName = "HR" },
                new Department() { Id = 3, DepartmentName = "Finance" }
                );
        }
    }
}
