using EmployeeInfo.Models;
using EmployeeInfo.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeInfo.Controllers
{
    public class SalaryController : Controller
    {
        private readonly ISalaryService _salaryService;
        private readonly ILogger<SalaryController> _logger;

        public SalaryController(ISalaryService salaryService, ILogger<SalaryController> logger)
        {
            _salaryService = salaryService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            return View(new List<Salary>());
        }

        [HttpGet]
        public async Task<IActionResult> GetSalaries(
        int page = 1,
        int pageSize = 10,
        string? searchTerm = null,
        string? sortColumn = null,
        string? sortDir = "asc")
        {
            try
            {
                _logger.LogInformation($"GetSalaries Search: page={page}, size={pageSize}, search={searchTerm}, sort={sortColumn} {sortDir}");
                var result = await _salaryService.GetSalariesPagedAsync(page, pageSize, searchTerm, sortColumn, sortDir);
                return Json(new
                {
                    data = result.Data,
                    totalCount = result.TotalCount
                });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Salary salary)
        {
            if (ModelState.IsValid)
            {
                await _salaryService.CreateSalaryAsync(salary);
                return RedirectToAction(nameof(Index));
            }
            return View(salary);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var salary = await _salaryService.GetSalaryByIdAsync(id);
            if (salary == null)
            {
                return NotFound();
            }
            return View(salary);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Salary salary)
        {
            if (id != salary.SalaryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _salaryService.UpdateSalaryAsync(salary);
                return RedirectToAction(nameof(Index));
            }
            return View(salary);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var salary = await _salaryService.GetSalaryByIdAsync(id);
            if (salary == null)
            {
                return NotFound();
            }
            return View(salary);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool success = await _salaryService.DeleteSalaryAsync(id);
            if (!success)
            {
                TempData["ErrorMessage"] = "Cannot delete this salary record because it is assigned to one or more employees.";
            }
            else
            {
                TempData["SuccessMessage"] = "Salary record deleted successfully.";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMultiple([FromBody] List<int> ids)
        {
            if (ids == null || !ids.Any())
                return Json(new { success = false, message = "No items selected." });

            int deletedCount = await _salaryService.DeleteMultipleSalariesAsync(ids);

            if (deletedCount < ids.Count)
            {
                return Json(new
                {
                    success = true,
                    message = $"{deletedCount} items deleted. {ids.Count - deletedCount} items were skipped because they are in use."
                });
            }

            return Json(new { success = true, message = "Selected items deleted successfully." });
        }
    }
}
