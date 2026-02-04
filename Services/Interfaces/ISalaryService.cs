using EmployeeInfo.Models;

namespace EmployeeInfo.Services.Interfaces
{
    public interface ISalaryService
    {
        Task<IEnumerable<Salary>> GetAllSalariesAsync();
        Task<Salary?> GetSalaryByIdAsync(int id);
        Task CreateSalaryAsync(Salary salary);
        Task UpdateSalaryAsync(Salary salary);
        Task<bool> DeleteSalaryAsync(int id);
        Task<int> DeleteMultipleSalariesAsync(List<int> ids);
        Task<PagedResult<Salary>> GetSalariesPagedAsync(int pageNumber,int pageSize,string? searchTerm = null,string? sortColumn = null,string? sortDir = "asc");
    }
}