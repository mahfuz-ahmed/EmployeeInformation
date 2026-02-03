using EmployeeInfo.Repositories.Interfaces;

namespace EmployeeInfo.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IEmployeeInfoRepository EmployeeInfos { get; }
        IDesignationRepository Designations { get; }
        ISalaryRepository Salaries { get; }
        Task<int> SaveAsync();
    }
}
