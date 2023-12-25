using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Models
{
    public class Employees
    {
        [Key]
        [Display(Name ="Mananger ID")]
        public int EmployeesId { get; set; }

        public string? EmployeeName { get; set; }
        public string? EmployeeSurname { get; set; }

        public string? UserId { get; set; }
    }
}
