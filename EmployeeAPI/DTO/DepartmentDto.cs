using System.ComponentModel.DataAnnotations;

namespace EmployeeAPI.DTOs
{
    public class DepartmentDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        public string DepartmentName { get; set; }
    }
}