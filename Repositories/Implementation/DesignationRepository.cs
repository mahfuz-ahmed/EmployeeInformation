using EmployeeInfo.Models;
using EmployeeInfo.Persistence;
using EmployeeInfo.Repositories.Interfaces;

namespace EmployeeInfo.Repositories.Implementation
{
    public class DesignationRepository : GenericRepository<EmployeeDesignation>, IDesignationRepository
    {
        public DesignationRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
