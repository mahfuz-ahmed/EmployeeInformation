using EmployeeInfo.Models;
using EmployeeInfo.Persistence;
using EmployeeInfo.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeInfo.Repositories.Implementation
{
    public class EmployeeInfoRepository : GenericRepository<EmployeeInformation>, IEmployeeInfoRepository
    {
        public EmployeeInfoRepository(ApplicationDbContext context) : base(context)
        {
        }

        public IQueryable<EmployeeInformation> GetAllWithDetails()
        {
            return _context.EmployeeInformations
                .Include(e => e.Designation)
                .Include(e => e.Salary)
                .AsNoTracking();
        }
    }
}
