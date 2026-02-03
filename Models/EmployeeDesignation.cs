using System.ComponentModel.DataAnnotations;

namespace EmployeeInfo.Models
{
    public class EmployeeDesignation : BaseEntity
    {
        [Key]
        public int DesignationId { get; set; }

        [Required]
        [Display(Name = "Designation Name")]
        public string DesignationName { get; set; }
    }
}
