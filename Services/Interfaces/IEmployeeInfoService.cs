using EmployeeInfo.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeInfo.Services.Interfaces
{
    public interface IEmployeeInfoService
    {
        Task<IEnumerable<EmployeeInfoVM>> GetAllEmployeesAsync();
        Task<EmployeeInfoVM?> GetEmployeeByIdAsync(int id);
        Task CreateEmployeeAsync(EmployeeInfoVM employeeVm);
        Task UpdateEmployeeAsync(EmployeeInfoVM employeeVm);
        Task DeleteEmployeeAsync(int id);
        Task<IEnumerable<EmployeeDesignation>> GetDesignationsAsync();
        Task<IEnumerable<Salary>> GetSalariesAsync();
        Task<PagedResult<EmployeeInfoVM>> GetEmployeesPagedAsync(int pageNumber, int pageSize, string? searchTerm = null);
        Task<bool> IsEmailExistsAsync(string email, int? excludeId = null);
    }
}
