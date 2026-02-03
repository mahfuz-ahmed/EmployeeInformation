using EmployeeInfo.Models;

namespace EmployeeInfo.Repositories.Interfaces
{
    public interface IEmployeeInfoRepository : IGenericRepository<EmployeeInformation>
    {
        IQueryable<EmployeeInformation> GetAllWithDetails();
    }
}
