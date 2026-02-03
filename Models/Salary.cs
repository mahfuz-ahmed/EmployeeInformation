using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeInfo.Models
{
    public class Salary : BaseEntity
    {
        [Key]
        public int SalaryId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Basic Salary")]
        public decimal BasicSalary { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Allowances { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Deductions { get; set; }

        [NotMapped]
        public decimal NetSalary => BasicSalary + Allowances - Deductions;
    }
}
