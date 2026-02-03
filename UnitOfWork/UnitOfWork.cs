using EmployeeInfo.Persistence;
using EmployeeInfo.Repositories.Implementation;
using EmployeeInfo.Repositories.Interfaces;

namespace EmployeeInfo.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        
        public IEmployeeInfoRepository EmployeeInfos { get; private set; }
        public IDesignationRepository Designations { get; private set; }
        public ISalaryRepository Salaries { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            EmployeeInfos = new EmployeeInfoRepository(_context);
            Designations = new DesignationRepository(_context);
            Salaries = new SalaryRepository(_context);
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
