using EmployeeInfo.Models;
using EmployeeInfo.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeInfo.Controllers
{
    public class DesignationController : Controller
    {
        private readonly IDesignationService _designationService;
        private readonly ILogger<DesignationController> _logger;

        public DesignationController(IDesignationService designationService,ILogger<DesignationController> logger)
        {
            _designationService = designationService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            return View(new List<EmployeeDesignation>());
        }

        [HttpGet]
        public async Task<IActionResult> GetDesignations(
           int page = 1,
           int pageSize = 10,
           string? searchTerm = null,
           string? sortColumn = null,
           string? sortDir = "asc")
        {
            _logger.LogInformation($"GetDesignations Search: page={page}, size={pageSize}, search={searchTerm}, sort={sortColumn} {sortDir}");
            var result = await _designationService.GetDesignationsPagedAsync(page, pageSize, searchTerm, sortColumn, sortDir);
            return Json(new
            {
                data = result.Data,
                totalCount = result.TotalCount
            });
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeDesignation designation)
        {
            if (ModelState.IsValid)
            {
                await _designationService.CreateDesignationAsync(designation);
                return RedirectToAction(nameof(Index));
            }
            return View(designation);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var designation = await _designationService.GetDesignationByIdAsync(id);
            if (designation == null)
            {
                return NotFound();
            }
            return View(designation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EmployeeDesignation designation)
        {
            if (id != designation.DesignationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _designationService.UpdateDesignationAsync(designation);
                return RedirectToAction(nameof(Index));
            }
            return View(designation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            bool success = await _designationService.DeleteDesignationAsync(id);
            if (!success)
            {
                TempData["ErrorMessage"] = "Cannot delete this designation because it is assigned to one or more employees.";
            }
            else
            {
                TempData["SuccessMessage"] = "Designation deleted successfully.";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMultiple([FromBody] List<int> ids)
        {
            if (ids == null || !ids.Any())
                return Json(new { success = false, message = "No items selected." });

            int deletedCount = await _designationService.DeleteMultipleDesignationsAsync(ids);

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
