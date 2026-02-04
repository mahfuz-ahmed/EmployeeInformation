using EmployeeInfo.Models;
using EmployeeInfo.Services.Interfaces;
using EmployeeInfo.UnitOfWork;
using Microsoft.EntityFrameworkCore;

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

        public async Task<int> DeleteMultipleSalariesAsync(List<int> ids)
        {
            var inUseIds = await _unitOfWork.EmployeeInfos.GetAll()
                .Where(e => ids.Contains(e.SalaryId))
                .Select(e => e.SalaryId)
                .Distinct()
                .ToListAsync();

            var idsToDelete = ids.Except(inUseIds).ToList();

            if (idsToDelete.Any())
            {
                var entities = await _unitOfWork.Salaries.GetAll()
                    .Where(s => idsToDelete.Contains(s.SalaryId))
                    .ToListAsync();

                _unitOfWork.Salaries.DeleteRange(entities);
                await _unitOfWork.SaveAsync();
                return entities.Count;
            }
            return 0;
        }

        public async Task<PagedResult<Salary>> GetSalariesPagedAsync(
        int pageNumber,
        int pageSize,
        string? searchTerm = null,
        string? sortColumn = null,
        string? sortDir = "asc")
        {
            var query = _unitOfWork.Salaries.GetAll();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                string cleanedTerm = searchTerm.Replace(",", "").Replace("$", "").Trim();
                if (decimal.TryParse(cleanedTerm, out decimal searchAmount))
                {
                    query = query.Where(s => s.BasicSalary == searchAmount || s.Allowances == searchAmount || s.Deductions == searchAmount || (s.BasicSalary + s.Allowances - s.Deductions) == searchAmount);
                }
            }

            if (!string.IsNullOrEmpty(sortColumn))
            {
                bool isDesc = sortDir == "desc";
                switch (sortColumn.ToLower())
                {
                    case "basicsalary":
                        query = isDesc ? query.OrderByDescending(s => s.BasicSalary) : query.OrderBy(s => s.BasicSalary);
                        break;
                    case "allowances":
                        query = isDesc ? query.OrderByDescending(s => s.Allowances) : query.OrderBy(s => s.Allowances);
                        break;
                    case "deductions":
                        query = isDesc ? query.OrderByDescending(s => s.Deductions) : query.OrderBy(s => s.Deductions);
                        break;
                    case "netsalary":
                        query = isDesc ? query.OrderByDescending(s => s.BasicSalary + s.Allowances - s.Deductions) : query.OrderBy(s => s.BasicSalary + s.Allowances - s.Deductions);
                        break;
                    default:
                        query = query.OrderBy(s => s.SalaryId);
                        break;
                }
            }
            else
            {
                query = query.OrderBy(s => s.SalaryId);
            }

            int totalCount = await query.CountAsync();
            var data = await query
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
