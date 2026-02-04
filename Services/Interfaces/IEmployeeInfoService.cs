using EmployeeInfo.Models;

namespace EmployeeInfo.Services.Interfaces
{
    public interface IEmployeeInfoService
    {
        Task<IEnumerable<EmployeeInfoVM>> GetAllEmployeesAsync();
        Task<EmployeeInfoVM?> GetEmployeeByIdAsync(int id);
        Task CreateEmployeeAsync(EmployeeInfoVM employeeVm);
        Task UpdateEmployeeAsync(EmployeeInfoVM employeeVm);
        Task DeleteEmployeeAsync(int id);
        Task DeleteMultipleEmployeesAsync(List<int> ids);
        Task<IEnumerable<EmployeeDesignation>> GetDesignationsAsync();
        Task<IEnumerable<Salary>> GetSalariesAsync();
        Task<PagedResult<EmployeeInfoVM>> GetEmployeesPagedAsync(int pageNumber,int pageSize,string? searchTerm = null,string? sortColumn = null,string? sortDir = "asc");
        Task<bool> IsEmailExistsAsync(string email, int? excludeId = null);
    }
}
