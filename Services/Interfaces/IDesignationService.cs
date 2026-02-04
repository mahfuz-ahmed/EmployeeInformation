using EmployeeInfo.Models;

namespace EmployeeInfo.Services.Interfaces
{
    public interface IDesignationService
    {
        Task<IEnumerable<EmployeeDesignation>> GetAllDesignationsAsync();
        Task<EmployeeDesignation?> GetDesignationByIdAsync(int id);
        Task CreateDesignationAsync(EmployeeDesignation designation);
        Task UpdateDesignationAsync(EmployeeDesignation designation);
        Task<bool> DeleteDesignationAsync(int id);
        Task<int> DeleteMultipleDesignationsAsync(List<int> ids);
        Task<PagedResult<EmployeeDesignation>> GetDesignationsPagedAsync(int pageNumber,int pageSize,string? searchTerm = null,string? sortColumn = null,string? sortDir = "asc");
    }
}
