using EmployeeInfo.Models;
using EmployeeInfo.Persistence;
using EmployeeInfo.Repositories.Interfaces;

namespace EmployeeInfo.Repositories.Implementation
{
    public class SalaryRepository : GenericRepository<Salary>, ISalaryRepository
    {
        public SalaryRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
