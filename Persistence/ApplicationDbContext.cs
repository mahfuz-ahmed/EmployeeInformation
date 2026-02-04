using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using EmployeeInfo.Models;

namespace EmployeeInfo.Persistence
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<EmployeeDesignation> EmployeeDesignations { get; set; }
        public DbSet<Salary> Salaries { get; set; }
        public DbSet<EmployeeInformation> EmployeeInformations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Insert Data When start migration
            modelBuilder.Entity<EmployeeDesignation>().HasData(
                new EmployeeDesignation { DesignationId = 1, DesignationName = "Software Engineer", CreatedOn = DateTime.Now, UpdatedOn = DateTime.Now },
                new EmployeeDesignation { DesignationId = 2, DesignationName = "Senior Software Engineer", CreatedOn = DateTime.Now, UpdatedOn = DateTime.Now },
                new EmployeeDesignation { DesignationId = 3, DesignationName = "Team Lead", CreatedOn = DateTime.Now, UpdatedOn = DateTime.Now },
                new EmployeeDesignation { DesignationId = 4, DesignationName = "Project Manager", CreatedOn = DateTime.Now, UpdatedOn = DateTime.Now }
            );

            modelBuilder.Entity<Salary>().HasData(
                new Salary { SalaryId = 1, BasicSalary = 30000, Allowances = 5000, Deductions = 1000, CreatedOn = DateTime.Now, UpdatedOn = DateTime.Now },
                new Salary { SalaryId = 2, BasicSalary = 50000, Allowances = 10000, Deductions = 2000, CreatedOn = DateTime.Now, UpdatedOn = DateTime.Now },
                new Salary { SalaryId = 3, BasicSalary = 80000, Allowances = 15000, Deductions = 3000, CreatedOn = DateTime.Now, UpdatedOn = DateTime.Now }
            );
        }
    }
}
