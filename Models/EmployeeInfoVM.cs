using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmployeeInfo.Models
{
    public class EmployeeInfoVM
    {
        public int EmployeeId { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;

        public string? Gender { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Designation")]
        public int? DesignationId { get; set; }

        [Required]
        [Display(Name = "Salary")]
        public int? SalaryId { get; set; }

        [Required]
        [Display(Name = "Joining Date")]
        [DataType(DataType.Date)]
        public DateTime JoiningDate { get; set; } = DateTime.Now;


        public string? DesignationName { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal NetSalary { get; set; }


        public IEnumerable<SelectListItem>? Designations { get; set; }
        public IEnumerable<SelectListItem>? Salaries { get; set; }
    }
}
