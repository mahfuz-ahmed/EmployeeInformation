using EmployeeInfo.Models;
using EmployeeInfo.Services.Interfaces;
using EmployeeInfo.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeInfo.Services.Implementation
{
    public class EmployeeInfoService : IEmployeeInfoService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeInfoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<EmployeeInfoVM>> GetAllEmployeesAsync()
        {
            var employees = await _unitOfWork.EmployeeInfos.GetAllWithDetails().ToListAsync();
            return employees.Select(ToVM).ToList();
        }

        public async Task<EmployeeInfoVM?> GetEmployeeByIdAsync(int id)
        {
            var emp = await _unitOfWork.EmployeeInfos.GetByIdAsync(id);
            if (emp == null) return null;        
            
            return ToVM(emp);
        }

        public async Task CreateEmployeeAsync(EmployeeInfoVM vm)
        {
            var entity = new EmployeeInformation
            {
                Title = vm.Title,
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                Gender = vm.Gender,
                Email = vm.Email,
                PhoneNumber = vm.PhoneNumber,
                DesignationId = vm.DesignationId ?? 0,
                SalaryId = vm.SalaryId ?? 0,
                JoiningDate = vm.JoiningDate,
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now
            };

            await _unitOfWork.EmployeeInfos.CreateAsync(entity);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateEmployeeAsync(EmployeeInfoVM vm)
        {
            var entity = await _unitOfWork.EmployeeInfos.GetByIdAsync(vm.EmployeeId);
            if (entity != null)
            {
                entity.Title = vm.Title;
                entity.FirstName = vm.FirstName;
                entity.LastName = vm.LastName;
                entity.Gender = vm.Gender ?? "";
                entity.Email = vm.Email;
                entity.PhoneNumber = vm.PhoneNumber;
                entity.DesignationId = vm.DesignationId ?? 0;
                entity.SalaryId = vm.SalaryId ?? 0;
                entity.JoiningDate = vm.JoiningDate;
                entity.UpdatedOn = DateTime.Now;

                _unitOfWork.EmployeeInfos.Update(entity);
                await _unitOfWork.SaveAsync();
            }
        }

        public async Task DeleteEmployeeAsync(int id)
        {
            var entity = await _unitOfWork.EmployeeInfos.GetByIdAsync(id);
            if (entity != null)
            {
                _unitOfWork.EmployeeInfos.Delete(entity);
                await _unitOfWork.SaveAsync();
            }
        }

        public async Task DeleteMultipleEmployeesAsync(List<int> ids)
        {
            var entities = await _unitOfWork.EmployeeInfos.FindByCondition(e => ids.Contains(e.EmployeeId)).ToListAsync();
            if (entities.Any())
            {
                _unitOfWork.EmployeeInfos.DeleteRange(entities);
                await _unitOfWork.SaveAsync();
            }
        }

        public async Task<IEnumerable<EmployeeDesignation>> GetDesignationsAsync()
        {
            return await _unitOfWork.Designations.GetAll().ToListAsync();
        }

        public async Task<IEnumerable<Salary>> GetSalariesAsync()
        {
            return await _unitOfWork.Salaries.GetAll().ToListAsync();
        }

        public async Task<PagedResult<EmployeeInfoVM>> GetEmployeesPagedAsync(
           int pageNumber,
           int pageSize,
           string? searchTerm = null,
           string? sortColumn = null,
           string? sortDir = "asc")
        {
            var query = _unitOfWork.EmployeeInfos.GetAllWithDetails();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.Trim();
                string cleanedAmount = searchTerm.Replace(",", "").Replace("$", "");
                bool isNumeric = decimal.TryParse(cleanedAmount, out decimal amount);

                query = query.Where(e =>
                    e.FirstName.Contains(searchTerm) ||
                    e.LastName.Contains(searchTerm) ||
                    e.Email.Contains(searchTerm) ||
                    e.Title.Contains(searchTerm) ||
                    (e.Designation != null && e.Designation.DesignationName.Contains(searchTerm)) ||
                    (isNumeric && e.Salary != null && e.Salary.BasicSalary == amount)
                );
            }

            if (!string.IsNullOrEmpty(sortColumn))
            {
                bool isDesc = sortDir == "desc";
                switch (sortColumn.ToLower())
                {
                    case "title":
                        query = isDesc ? query.OrderByDescending(e => e.Title) : query.OrderBy(e => e.Title);
                        break;
                    case "firstname":
                        query = isDesc ? query.OrderByDescending(e => e.FirstName).ThenByDescending(e => e.LastName)
                                       : query.OrderBy(e => e.FirstName).ThenBy(e => e.LastName);
                        break;
                    case "email":
                        query = isDesc ? query.OrderByDescending(e => e.Email) : query.OrderBy(e => e.Email);
                        break;
                    case "phonenumber":
                        query = isDesc ? query.OrderByDescending(e => e.PhoneNumber) : query.OrderBy(e => e.PhoneNumber);
                        break;
                    case "designationname":
                        query = isDesc ? query.OrderByDescending(e => e.Designation.DesignationName) : query.OrderBy(e => e.Designation.DesignationName);
                        break;
                    case "basicsalary":
                        query = isDesc ? query.OrderByDescending(e => e.Salary.BasicSalary) : query.OrderBy(e => e.Salary.BasicSalary);
                        break;
                    case "netsalary":
                        query = isDesc ? query.OrderByDescending(e => e.Salary.BasicSalary + e.Salary.Allowances - e.Salary.Deductions)
                                       : query.OrderBy(e => e.Salary.BasicSalary + e.Salary.Allowances - e.Salary.Deductions);
                        break;
                    case "joiningdate":
                        query = isDesc ? query.OrderByDescending(e => e.JoiningDate) : query.OrderBy(e => e.JoiningDate);
                        break;
                    default:
                        query = query.OrderByDescending(e => e.CreatedOn);
                        break;
                }
            }
            else
            {
                query = query.OrderByDescending(e => e.CreatedOn);
            }

            var allSalariesQuery = _unitOfWork.EmployeeInfos.GetAllWithDetails()
                .Where(e => e.Salary != null)
                .Select(e => e.Salary!.BasicSalary);

            decimal maxSalary = 0;
            decimal minSalary = 0;

            if (await allSalariesQuery.AnyAsync())
            {
                maxSalary = await allSalariesQuery.MaxAsync();
                minSalary = await allSalariesQuery.MinAsync();
            }

            int totalCount = await query.CountAsync();

            var pagedData = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<EmployeeInfoVM>
            {
                Data = pagedData.Select(ToVM).ToList(),
                TotalCount = totalCount,
                MaxSalary = maxSalary,
                MinSalary = minSalary
            };
        }

        public async Task<bool> IsEmailExistsAsync(string email, int? excludeId = null)
        {
            var query = _unitOfWork.EmployeeInfos.GetAll();
            if (excludeId.HasValue)
            {
                query = query.Where(e => e.EmployeeId != excludeId.Value);
            }
            return await query.AnyAsync(e => e.Email == email);
        }

        private EmployeeInfoVM ToVM(EmployeeInformation emp)
        {
            return new EmployeeInfoVM
            {
                EmployeeId = emp.EmployeeId,
                Title = emp.Title,
                FirstName = emp.FirstName,
                LastName = emp.LastName,
                Gender = emp.Gender,
                Email = emp.Email,
                PhoneNumber = emp.PhoneNumber,
                DesignationId = emp.DesignationId,
                SalaryId = emp.SalaryId,
                JoiningDate = emp.JoiningDate,
                DesignationName = emp.Designation?.DesignationName,
                BasicSalary = emp.Salary?.BasicSalary ?? 0,
                NetSalary = emp.Salary?.NetSalary ?? 0
            };
        }
    }
}
