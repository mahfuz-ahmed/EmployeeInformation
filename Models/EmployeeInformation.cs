using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeInfo.Models
{
    public class EmployeeInformation : BaseEntity
    {
        [Key]
        public int EmployeeId { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;

        public string Gender { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Designation")]
        public int DesignationId { get; set; }
        public virtual EmployeeDesignation? Designation { get; set; }

        [Required]
        [Display(Name = "Salary")]
        public int SalaryId { get; set; }
        public virtual Salary? Salary { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Joining Date")]
        public DateTime JoiningDate { get; set; }
    }
}
