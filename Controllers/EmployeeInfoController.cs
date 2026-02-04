using EmployeeInfo.Models;
using EmployeeInfo.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmployeeInfo.Controllers
{
    public class EmployeeInfoController : Controller
    {
        private readonly IEmployeeInfoService _service;
        private readonly ILogger<EmployeeInfoController> _logger;

        public EmployeeInfoController(IEmployeeInfoService service, ILogger<EmployeeInfoController> logger)
        {
            _service = service;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployees(
        int page = 1,
        int pageSize = 10,
        string? searchTerm = null,
        string? sortColumn = null,
        string? sortDir = "asc")
        {
            try
            {
                _logger.LogInformation($"GetEmployees Search: page={page}, size={pageSize}, search={searchTerm}, sort={sortColumn} {sortDir}");
                var result = await _service.GetEmployeesPagedAsync(page, pageSize, searchTerm, sortColumn, sortDir);
                _logger.LogInformation($"Found {result.TotalCount} employees");
                return Json(new
                {
                    data = result.Data,
                    totalCount = result.TotalCount,
                    page = page,
                    pageSize = pageSize,
                    maxSalary = result.MaxSalary,
                    minSalary = result.MinSalary
                });
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error getting employees during search");
                return StatusCode(500, new { message = "Search Error: " + ex.Message });
            }
        }

        public async Task<IActionResult> Create()
        {
            await PopulateDropdowns();
            return View(new EmployeeInfoVM());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeInfoVM vm)
        {
            if (ModelState.IsValid)
            {
                if (await _service.IsEmailExistsAsync(vm.Email))
                {
                    ModelState.AddModelError("Email", "This email already exists.");
                }
                else
                {
                    await _service.CreateEmployeeAsync(vm);
                    return RedirectToAction(nameof(Index));
                }
            }
            
            foreach (var value in ModelState.Values)
            {
                foreach (var error in value.Errors)
                {
                    _logger.LogError($"Validation Error: {error.ErrorMessage}");
                }
            }

            await PopulateDropdowns();
            return View(vm);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var vm = await _service.GetEmployeeByIdAsync(id);
            if (vm == null) return NotFound();

            await PopulateDropdowns();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EmployeeInfoVM vm)
        {
             if (ModelState.IsValid)
            {
                if (await _service.IsEmailExistsAsync(vm.Email, vm.EmployeeId))
                {
                    ModelState.AddModelError("Email", "This email already exists.");
                }
                else
                {
                    await _service.UpdateEmployeeAsync(vm);
                    return RedirectToAction(nameof(Index));
                }
            }
            await PopulateDropdowns();
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteEmployeeAsync(id);
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMultiple([FromBody] List<int> ids)
        {
            if (ids == null || !ids.Any())
            {
                return Json(new { success = false, message = "No employees selected." });
            }

            await _service.DeleteMultipleEmployeesAsync(ids);
            return Json(new { success = true });
        }

        private async Task PopulateDropdowns()
        {
            var designations = await _service.GetDesignationsAsync();
            var salaries = await _service.GetSalariesAsync();

            ViewBag.Designations = new SelectList(designations, "DesignationId", "DesignationName");
            ViewBag.Salaries = new SelectList(salaries, "SalaryId", "BasicSalary"); 
        }
    }
}
