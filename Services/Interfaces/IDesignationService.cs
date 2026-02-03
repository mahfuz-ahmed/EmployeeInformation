using EmployeeInfo.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeInfo.Services.Interfaces
{
    public interface IDesignationService
    {
        Task<IEnumerable<EmployeeDesignation>> GetAllDesignationsAsync();
        Task<EmployeeDesignation?> GetDesignationByIdAsync(int id);
        Task CreateDesignationAsync(EmployeeDesignation designation);
        Task UpdateDesignationAsync(EmployeeDesignation designation);
        Task<bool> DeleteDesignationAsync(int id);
        Task<PagedResult<EmployeeDesignation>> GetDesignationsPagedAsync(int pageNumber, int pageSize, string? searchTerm = null);
    }
}
