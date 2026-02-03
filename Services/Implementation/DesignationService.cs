using EmployeeInfo.Models;
using EmployeeInfo.Services.Interfaces;
using EmployeeInfo.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace EmployeeInfo.Services.Implementation
{
    public class DesignationService : IDesignationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DesignationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<EmployeeDesignation>> GetAllDesignationsAsync()
        {
            return await _unitOfWork.Designations.GetAll().ToListAsync();
        }

        public async Task<EmployeeDesignation?> GetDesignationByIdAsync(int id)
        {
            return await _unitOfWork.Designations.GetByIdAsync(id);
        }

        public async Task CreateDesignationAsync(EmployeeDesignation designation)
        {
            await _unitOfWork.Designations.CreateAsync(designation);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateDesignationAsync(EmployeeDesignation designation)
        {
            _unitOfWork.Designations.Update(designation);
            await _unitOfWork.SaveAsync();
        }

        public async Task<bool> DeleteDesignationAsync(int id)
        {
            var isInUse = await _unitOfWork.EmployeeInfos.GetAll().AnyAsync(e => e.DesignationId == id);
            if (isInUse) return false;

            var designation = await _unitOfWork.Designations.GetByIdAsync(id);
            if (designation != null)
            {
                _unitOfWork.Designations.Delete(designation);
                await _unitOfWork.SaveAsync();
                return true;
            }
            return false;
        }

        public async Task<PagedResult<EmployeeDesignation>> GetDesignationsPagedAsync(int pageNumber, int pageSize, string? searchTerm = null)
        {
            var query = _unitOfWork.Designations.GetAll();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(d => d.DesignationName.Contains(searchTerm));
            }

            int totalCount = await query.CountAsync();
            var data = await query
                .OrderBy(d => d.DesignationName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<EmployeeDesignation>
            {
                Data = data,
                TotalCount = totalCount
            };
        }
    }
}
