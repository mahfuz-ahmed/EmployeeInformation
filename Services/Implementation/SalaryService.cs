using EmployeeInfo.Models;
using EmployeeInfo.Services.Interfaces;
using EmployeeInfo.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeInfo.Services.Implementation
{
    public class SalaryService : ISalaryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SalaryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Salary>> GetAllSalariesAsync()
        {
            return await _unitOfWork.Salaries.GetAll().ToListAsync();
        }

        public async Task<Salary?> GetSalaryByIdAsync(int id)
        {
            return await _unitOfWork.Salaries.GetByIdAsync(id);
        }

        public async Task CreateSalaryAsync(Salary salary)
        {
            await _unitOfWork.Salaries.CreateAsync(salary);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateSalaryAsync(Salary salary)
        {
            _unitOfWork.Salaries.Update(salary);
            await _unitOfWork.SaveAsync();
        }

        public async Task<bool> DeleteSalaryAsync(int id)
        {
            var isInUse = await _unitOfWork.EmployeeInfos.GetAll().AnyAsync(e => e.SalaryId == id);
            if (isInUse) return false;

            var salary = await _unitOfWork.Salaries.GetByIdAsync(id);
            if (salary != null)
            {
                _unitOfWork.Salaries.Delete(salary);
                await _unitOfWork.SaveAsync();
                return true;
            }
            return false;
        }

        public async Task<PagedResult<Salary>> GetSalariesPagedAsync(int pageNumber, int pageSize, string? searchTerm = null)
        {
            var query = _unitOfWork.Salaries.GetAll();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                string cleanedTerm = searchTerm.Replace(",", "").Replace("$", "").Trim();
                if (decimal.TryParse(cleanedTerm, out decimal searchAmount))
                {
                    query = query.Where(s => s.BasicSalary == searchAmount);
                }
            }

            int totalCount = await query.CountAsync();
            var data = await query
                .OrderBy(s => s.SalaryId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Salary>
            {
                Data = data,
                TotalCount = totalCount
            };
        }
    }
}
